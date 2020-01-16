using Microsoft.EntityFrameworkCore;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Repository
{
    public class OrderRepository: GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(AppDataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
        {
            var user = await _userHelper.GetUserBiEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Orders.Include(od => od.Items).ThenInclude(p => p.Product).OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders
                    .Include(od => od.Items)
                    .ThenInclude(p => p.Product)
                    .Where(o => o.User == user)
                    .OrderByDescending(o => o.OrderDate);
        }
    }
}
