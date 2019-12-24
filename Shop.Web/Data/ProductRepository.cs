using Shop.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDataContext _context;

        public ProductRepository(AppDataContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts() => _context.Products.OrderBy(p => p.Name);

        public Product GetProductById(int id) => _context.Products.Find(id);

        public void AddProduct(Product product) => _context.Products.Add(product);

        public void UpdateProduct(Product product) => _context.Update(product);

        public void RemoveProduct(Product product) => _context.Products.Remove(product);

        public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;

        public bool ProductExistById(int id) => _context.Products.Any(p => p.Id == id);
    }
}
