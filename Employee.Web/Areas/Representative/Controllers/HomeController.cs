using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Web.Areas.Representative.Controllers
{
    [Area("Representative")]
    [Authorize(Roles = "Representative")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
