using Nancy;
using Nancy.ModelBinding;
using NancyApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NancyApi
{
    public class CatalogueModule: NancyModule
    {
        public CatalogueModule(IProductsCatalogue pc) //:base ("/Products")
        {            
            Get("/Products", _ =>
            {
                return pc.GetProducts(null);
                //return Response.AsJson(pc.GetProducts(null));
            });

            Get("/Products", _ =>
            {
                var stockFilter = (bool)Request.Query["Stock"];
                return pc.GetProducts(stockFilter);
            }, ctx => ctx.Request.Query["Stock"] != null); //Condition

            Get("/Products/{id:int}", p =>
            {
                return pc.GetProducts((int)p.id);
            });

            //Post sincrono

            //Post("/Products", p =>
            //{
            //    //var prod = new Product()
            //    //{
            //    //    Name = p.Name,
            //    //    Stock = p.Stock
            //    //};

            //    var prod = this.Bind<Product>();
            //    prod.Id = pc.AddProduct(prod);

            //    var url = string.Format("{0}/{1}", Request.Url, prod.Id);
            //    var res = new Response()
            //    {
            //        StatusCode = HttpStatusCode.Accepted,
            //    }.WithHeader("Location", url);
            //    return res;
            //    //return prod;
            //});

            Post("/Products", async (p,ct) =>
            {            
                var prod = this.Bind<Product>();
                prod.Id=await pc.AddProductAsync(prod);

                var url = string.Format("{0}/{1}", Request.Url, prod.Id);
                var res = new Response()
                {
                    StatusCode= HttpStatusCode.Accepted,                                     
                }.WithHeader("Location", url);
                return res;
            });

            Put("/Products", p =>
            {
                var prod = this.Bind<Product>();
                prod=  pc.UpdateProduct(prod);
                if (prod == null)
                {
                    return new NotFoundResponse();
                }
                else
                    return prod;
            });

            Before += ctx =>
            {                
                //Return null or response object
                (ctx as NancyContext).Items["timestamp"] = DateTime.UtcNow;
                return null;
            };
            
            After += ctx =>
            {
                DateTime end = DateTime.UtcNow;
                var start = (ctx.Items["timestamp"]);
                var elapsed = (end - (DateTime)start).TotalMilliseconds;

                //Modify response
                (ctx as NancyContext).Response.Headers.Add("XX-PROCESSING-TIME", elapsed.ToString());
            };
        }
    }
}
