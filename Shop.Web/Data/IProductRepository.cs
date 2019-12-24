using Shop.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public interface IProductRepository
    {
        void AddProduct(Product product);

        Product GetProductById(int id);
        
        IEnumerable<Product> GetProducts();
        
        bool ProductExistById(int id);
        
        void RemoveProduct(Product product);
        
        Task<bool> SaveAllAsync();
        
        void UpdateProduct(Product product);
    }
}