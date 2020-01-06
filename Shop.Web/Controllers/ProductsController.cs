using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Web.Data;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using Shop.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = string.Empty;

                    if (view.ImageFile != null && view.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(Directory.GetCurrentDirectory(),
                                            "wwwroot\\images\\Products",
                                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{file}";
                    }


                    var product = ToProduct(view, path);


                    product.User = await _userHelper.GetUserBiEmailAsync(User.Identity.Name);
                    await _productRepository.CreateAsync(product);

                }
                catch (Exception ex)
                {

                    return BadRequest($"The product couldn't be created: {ex.Message}");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        private Product ToProduct(ProductViewModel view, string path)
        {
            return new Product
            {
                Id = view.Id,
                ImageUrl = path,
                IsAvailabe = view.IsAvailabe,
                LastPurchase = view.LastPurchase,
                LastSale = view.LastSale,
                Name = view.Name,
                Price = view.Price,
                Stock = view.Stock,
                User = view.User,

            };
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            var view = ToProductViewModel(product);

            return View(view);
        }

        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User,
                ImageUrl = product.ImageUrl,
            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel view)
        {
            if (id != view.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var path = view.ImageUrl;

                    if (view.ImageFile != null && view.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(Directory.GetCurrentDirectory(),
                                            "wwwroot\\images\\Products",
                                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await view.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{file}";
                    }

                    var product = ToProduct(view, path);

                    product.User = await _userHelper.GetUserBiEmailAsync(User.Identity.Name);
                    await _productRepository.UpdateAsync(product);
                    //await _productRepository.SaveAllAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(view.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        // GET: Products/Delete/5
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                await _productRepository.DeleteAsync(product);

                //await _productRepository.SaveAllAsync();
            }
            catch (Exception ex)
            {

                return BadRequest($"Product couldn't be Delete: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
