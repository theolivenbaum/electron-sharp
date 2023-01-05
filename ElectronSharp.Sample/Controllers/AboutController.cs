using Microsoft.AspNetCore.Mvc;

namespace ElectronSharp.SampleApp.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}