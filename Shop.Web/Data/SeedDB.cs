using Microsoft.AspNetCore.Identity;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public class SeedDB
    {
        private readonly AppDataContext _context;
        private readonly IUserHelper _userHelper;
        private Random random;
        public SeedDB(AppDataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
             await _context.Database.EnsureCreatedAsync();

            var user = await _userHelper.GetUserBiEmailAsync("barrera_emilio@hotmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Emilio",
                    LastName = "Barrera",
                    Email = "barrera_emilio@hotmail.com",
                    UserName = "barrera_emilio@hotmail.com",
                    PhoneNumber = "+447907951284",
                };
            }

            var result = await _userHelper.AddUserAsync(user,"Eabs123.");
            if (result != IdentityResult.Success)
            {

                throw new InvalidOperationException("Could not create the user in Seeder.");
            }

            if (!_context.Products.Any())
            {
                AddProduct("iPhone x",user);
                AddProduct("Magic Mause",user);
                AddProduct("iWatch Series 4",user );
                AddProduct("Laptop Toshiba", user);
                AddProduct("Sunglasses oracle", user);

                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string product, User user)
        {
            _context.Products.Add(new Product 
            {
                Name = product,
                Price = random.Next(1000),
                IsAvailabe = true,
                Stock  = random.Next(100),
                User = user,
            });
        }
    }
}
