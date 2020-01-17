using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Data;
using Shop.Web.Data.Repository;
using System.Threading.Tasks;

namespace Shop.Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index() => View(await _orderRepository.GetOrdersAsync(User.Identity.Name));

        public async Task<IActionResult> Create() => View(await _orderRepository.GetDetailTempsAsync(User.Identity.Name));



    }
}