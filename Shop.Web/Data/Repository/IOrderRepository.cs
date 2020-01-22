using Shop.Web.Data.Entities;
using Shop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string UserName);

        Task AddItemToOrderAsync(AddItemViewModel model, string name);

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);

        Task DeleteDetailTempAsync(int id);
    }
}
