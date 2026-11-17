using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_Final_Project_sem_5.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}