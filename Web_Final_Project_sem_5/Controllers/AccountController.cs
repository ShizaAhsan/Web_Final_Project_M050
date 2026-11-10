
using Microsoft.AspNetCore.Mvc;

namespace Web_Final_Project_sem_5.Controllers
{

    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
    }
}
