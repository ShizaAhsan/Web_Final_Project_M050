
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Web_Final_Project_sem_5.Models;
using Web_Final_Project_sem_5.Models.ViewModels;
namespace Web_Final_Project_sem_5.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // LOGIN - GET
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // LOGIN - POST
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Please enter valid login information."
                });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid email or password."
                });
            }

            bool hasRole = await _userManager.IsInRoleAsync(user, model.Role);

            if (!hasRole)
            {
                return Json(new
                {
                    success = false,
                    message = "Selected role does not match your account."
                });
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                string redirectUrl;

                if (model.Role == "Admin")
                {
                    redirectUrl = Url.Action("Dashboard", "Admin");
                }
                else
                {
                    redirectUrl = Url.Action("Dashboard", "Farmer");
                }

                return Json(new
                {
                    success = true,
                    redirectUrl = redirectUrl
                });
            }
            return Json(new
            {
                success = false,
                message = "Invalid email or password."
            });
        }

        // SIGNUP - GET
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string roleName = model.Role;

            // Create role if it does not exist
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }

            // Create user
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(
                user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            // Assign selected role
            var roleAssignment = await _userManager.AddToRoleAsync(
                user, roleName);

            if (!roleAssignment.Succeeded)
            {
                foreach (var error in roleAssignment.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                await _userManager.DeleteAsync(user);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            TempData["SuccessMessage"] =
                $"Registration Successful! Welcome to CropCare as {roleName}.";

            if (roleName == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return RedirectToAction("Dashboard", "Farmer");
        }
        //LOGOUT
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        // Existing Register route
        public IActionResult Register()
        {
            return RedirectToAction("SignUp");
        }
    }
}
