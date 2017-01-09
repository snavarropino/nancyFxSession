using Nancy;
using NancyApi.Domain;


namespace NancyApi
{
    public class UIModule: NancyModule
    {
        public UIModule(IProductsCatalogue pc)
        {
            Get("/UI", _ =>
            {
                var prodList = pc.GetProducts();
                return View["ProductList.html", prodList];
            });
        }
    }
}
