using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NancyApi.Domain
{
    public interface IProductsCatalogue
    {
        IList<Product> GetProducts(int? id=null);
        IList<Product> GetProducts(bool stockFilter);
        int AddProduct(Product p);
        Task<int> AddProductAsync(Product p);
        Product UpdateProduct(Product p);
    }
}
