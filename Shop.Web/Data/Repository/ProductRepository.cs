using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDataContext _context;

        public ProductRepository(AppDataContext context) : base(context)
        {
           _context = context;
        }

        public IQueryable GetAllWithUser()
        {
            return _context.Products.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboProducts()
        {
            var list = _context.Products.Select(p => new SelectListItem
            {
               Text = p.Name,
               Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem 
            {
               Text = "(Select a Product.....)",
               Value = "0",
            });

            return list;
        }
    }
}
