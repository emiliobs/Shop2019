using Microsoft.EntityFrameworkCore;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using Shop.Web.Models;
using Shop.Web.ViewModels;
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

      

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserBiEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            return _context.OrderDetailTemps.Include(p => p.Product).Where(u => u.User == user).OrderBy(P => P.Product.Name); 
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
                return _context.Orders
                    .Include(o => o.User)
                    .Include(od => od.Items)
                    .ThenInclude(p => p.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders                        
                    .Include(od => od.Items)
                    .ThenInclude(p => p.Product)
                    .Where(o => o.User == user)
                    .OrderByDescending(o => o.OrderDate);
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await _context.OrderDetailTemps.FindAsync(id);

            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                _context.OrderDetailTemps.Update(orderDetailTemp);
               await  _context.SaveChangesAsync();
            }

        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserBiEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var product = await _context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await _context.OrderDetailTemps.Where(odt => odt.User == user && odt.Product == product).FirstOrDefaultAsync();
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp 
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                _context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                _context.OrderDetailTemps.Update(orderDetailTemp);
            }


            await _context.SaveChangesAsync();
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var oderDetailTemp = await _context.OrderDetailTemps.FindAsync(id);
            if (oderDetailTemp == null)
            {
                return;
            }

            _context.OrderDetailTemps.Remove(oderDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserBiEmailAsync(userName);
            if (user == null)
            {
                return false;
            }

            var orderTemps = await _context.OrderDetailTemps.Include(o => o.Product).Where(o => o.User == user).ToListAsync();
            if (orderTemps == null || orderTemps.Count == 0)
            {
                return false;
            }

            var details = orderTemps.Select(o => new OrderDetail 
            {
              Price = o.Price,
              Product = o.Product,
              Quantity = o.Quantity,
            }).ToList();


            var order = new Order 
            {
                OrderDate = DateTime.UtcNow,
               DeliveryDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            _context.Orders.Add(order);
            _context.OrderDetailTemps.RemoveRange(orderTemps);
           await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task DeliverOrder(DeliverViewModel model)
        {
            var order = await _context.Orders.FindAsync(model.Id);
            if (order == null)
            {
                return;
            }

            order.DeliveryDate = model.DeliveryDate;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrdersAsync(int id) => await _context.Orders.FindAsync(id);
      
    }
}
