using CoraetionTask.Models;
using CoraetionTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoraetionTask.Controllers
{
    
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public CustomerController(ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<Customer> customersList = await _customerRepository.GetAllAsync();
            return View(customersList);
        }

        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> Details(int id)
        {
            Customer customersDetails = await _customerRepository.GetByIdAsync(id,c=>c.Products);

            if (customersDetails == null) return View("NotFound");
            return View(customersDetails);
        }

        [HttpGet,Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Customer customersDetails = await _customerRepository.GetByIDAsync(id);
            if (customersDetails == null) return View("NotFound");

            return View(customersDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            Customer customersDetails = await _customerRepository.GetByIDAsync(id);
            if (customersDetails == null) return View("NotFound");
            if (ModelState.IsValid)
            {
                await _customerRepository.UpdateAsync(id, customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Customer customersDetails = await _customerRepository.GetByIDAsync(id);
            if (customersDetails == null) return View("NotFound");

            return View(customersDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            Customer customersDetails = await _customerRepository.GetByIDAsync(id);
            if (customersDetails == null) return View("NotFound");
            await _customerRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Create(int id) => View();

        [HttpPost]
        public async Task<IActionResult> Create(int id, Customer customer)
        {

            if (ModelState.IsValid)
            {
                await _customerRepository.AddAsync(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Profile(int id)
        {
            Customer customersDetails = await _customerRepository.GetByIdAsync(id,c=>c.Products);
            //ViewBag.Products = await _productRepository.GetAllAsync();

            if (customersDetails == null) return View("NotFound");

            return View(customersDetails);
        }

        public async Task<IActionResult> BuyProduct() {
            ViewBag.Products = await _productRepository.GetAllAsync();
            return View(); 
        }

        [HttpPost,Authorize(Roles = "User")]
        public async Task<IActionResult> BuyProduct(int customerId, int[] productIds)
        {
            Customer customer = await _customerRepository.GetByIDAsync(customerId);

            if (customer == null)
            {
                return View("NotFound");
            }

            foreach (int productId in productIds)
            {
                Product product = await _productRepository.GetByIDAsync(productId);

                if (product != null)
                {
                    await _customerRepository.AddProductToCustomer(customerId, product);
                }
            }

            return RedirectToAction("Profile", new { id = customerId });
        }

    }
}

