using CoraetionTask.Models;
using CoraetionTask.Services;
using CoraetionTask.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CoraetionTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerRepository _customerRepository;
        public AccountController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            ICustomerRepository customerRepository
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _customerRepository = customerRepository;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationViewModel newAccount)
        {
            if (ModelState.IsValid)
            {
                // first register will be admin and any other rigester will be customer
                if (_userManager.Users.Count() > 0)
                {
                    // create new customer 
                    Customer newCustomer = new Customer()
                    {
                        FullName = newAccount.FullName,
                        Address = newAccount.Address,
                        Mobile = newAccount.Mobile,
                    };

                    // save new customer in database
                    await _customerRepository.AddAsync(newCustomer);

                    // assign new customer as a user in the system
                    IdentityUser newUser = new IdentityUser()
                    {
                        UserName = newAccount.FullName,
                        Email = newAccount.Email,
                        PhoneNumber = newAccount.Mobile,
                    };

                    // save new user in database
                    IdentityResult result = await _userManager.CreateAsync(newUser, newAccount.Password);

                    if (result.Succeeded)
                    {
                        if (await _roleManager.RoleExistsAsync("User"))
                        {
                            await _userManager.AddToRoleAsync(newUser, "User");
                        }
                        else
                        {
                            IdentityResult roleUser = await _roleManager.CreateAsync(new IdentityRole("User"));

                            await _userManager.AddToRoleAsync(newUser, "User");
                        }
                        return RedirectToAction("RegisterCompleted");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    // create admin to the system but not save him as a customer
                    IdentityUser newUser = new IdentityUser()
                    {
                        UserName = newAccount.FullName,
                        Email = newAccount.Email,
                        PhoneNumber = newAccount.Mobile,
                    };

                    // save new user (admin) in database
                    IdentityResult result = await _userManager.CreateAsync(newUser, newAccount.Password);
                    if (result.Succeeded)
                    {
                        if (await _roleManager.RoleExistsAsync("Admin"))
                        {
                            await _userManager.AddToRoleAsync(newUser, "Admin");
                        }
                        else
                        {
                            IdentityResult roleAdmin = await _roleManager.CreateAsync(new IdentityRole("Admin"));

                            await _userManager.AddToRoleAsync(newUser, "Admin");
                        }
                        return RedirectToAction("RegisterCompleted");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(newAccount);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                IdentityUser existUser = await _userManager.FindByEmailAsync(loginUser.Email);
                if (existUser != null)
                {
                    // get customer to get id from him
                    Customer customer = await _customerRepository.GetCustomerByFullName(existUser.UserName);

                    if (customer!=null)
                    {
                        // save customer id in session storage to use it later to get his profile
                        HttpContext.Session.SetInt32("customerId", customer.ID);
                    }
                    var result = await _signInManager.PasswordSignInAsync(existUser, loginUser.Password, loginUser.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid username or password.";
                }

            }
            return View(loginUser);
        }

        public IActionResult RegisterCompleted() => View();
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
