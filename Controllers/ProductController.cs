using CoraetionTask.Models;
using CoraetionTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoraetionTask.Controllers
{

    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        private readonly ICustomerRepository _customerRepository;

        public ProductController(IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var productList = await _productRepository.GetAllAsync(u=>u.Customer);
            return View(productList);
        }

        public async Task<IActionResult> Details(int id)
        {
            Product productDetails = await _productRepository.GetByIDAsync(id);

            if (productDetails == null) return View("NotFound");
            return View(productDetails);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            Product productDetails = await _productRepository.GetByIDAsync(id);
            if (productDetails == null) return View("NotFound");

            return View(productDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            Product productDetails = await _productRepository.GetByIDAsync(id);
            if (productDetails == null) return View("NotFound");
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(id, product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            Product productDetails = await _productRepository.GetByIDAsync(id);

            if (productDetails == null) return View("NotFound");

            return View(productDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            Product productDetails = await _productRepository.GetByIDAsync(id);
            if (productDetails == null) return View("NotFound");
            await _productRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Create() =>  View();

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

            if (ModelState.IsValid)
            {
                await _productRepository.AddAsync(product);

                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
