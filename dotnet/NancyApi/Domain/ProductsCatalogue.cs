using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NancyApi.Domain
{
    public class ProductsCatalogue : IProductsCatalogue
    {       

        public int AddProduct(Product p)
        {
            var id = 0;

            if(products.Any())
                id=products.Max(a=> a.Id) + 1;

            p.Id = id;
            products.Add(p);

            return id;
        }

        public IList<Product> GetProducts(bool stockFilter)
        {
            if(stockFilter)
                return products.Where(a => a.Stock > 0).ToList();
            else
                return products.Where(a => a.Stock == 0).ToList();

        }

        public IList<Product> GetProducts(int? id = null)
        {
            if (id.HasValue)
                return products.Where(a => a.Id ==id.Value).ToList();
            else
                return products;
        
        }

        public Product UpdateProduct(Product p)
        {
            var existantPOroduct = products.Where(a => a.Id == p.Id).FirstOrDefault();
            if (existantPOroduct != null)
            {
                existantPOroduct.Name = p.Name;
                existantPOroduct.Stock = p.Stock;
                return existantPOroduct;
            }
            else
                return null;            
        }

        public static List<Product> products = new List<Product>()
        {
            new Product()
            {
                Id=1,
                Name="Mahou",
                Stock=1
            },
            new Product()
            {
                Id=2,
                Name="San Miguel",
                Stock=1
            },
            new Product()
            {
                Id=3,
                Name="Cruzcampo",
                Stock=0
            }
        };

        public async Task<int> AddProductAsync(Product p)
        {
            var t = Task.Factory.StartNew(() =>
              {
                  var id = 0;

                  if (products.Any())
                      id = products.Max(a => a.Id) + 1;

                  p.Id = id;
                  products.Add(p);

                  return id;
              });

            return await t;
        }
    }
}
