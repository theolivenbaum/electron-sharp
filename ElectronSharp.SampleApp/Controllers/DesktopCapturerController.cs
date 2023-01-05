using Microsoft.AspNetCore.Mvc;

namespace ElectronSharp.SampleApp.Controllers
{
    public class DesktopCapturerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}