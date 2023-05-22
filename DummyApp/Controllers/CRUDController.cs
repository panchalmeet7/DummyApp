using Microsoft.AspNetCore.Mvc;

namespace DummyApp.Controllers
{
    public class CRUDController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
