using Shop.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public class SeedDB
    {
        private readonly AppDataContext _context;
        private Random random;
        public SeedDB(AppDataContext context)
        {
            _context = context;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
             await _context.Database.EnsureCreatedAsync();

            if (!_context.Products.Any())
            {
                AddProduct("iPhone x");
                AddProduct("Magic Mause");
                AddProduct("iWatch Series 4");
                AddProduct("Laptop Toshiba");
                AddProduct("Sunglasses oracle");

                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string product)
        {
            _context.Products.Add(new Product 
            {
                Name = product,
                Price = random.Next(1000),
                IsAvailabe = true,
                Stock  = random.Next(100),
            });
        }
    }
}
