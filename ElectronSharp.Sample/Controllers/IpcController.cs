using Microsoft.AspNetCore.Mvc;
using ElectronSharp.API;
using System.Linq;

namespace ElectronSharp.SampleApp.Controllers
{
    public class IpcController : Controller
    {
        public IActionResult Index()
        {
            if(HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.On("async-msg", (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "asynchronous-reply", "pong");
                });

                Electron.IpcMain.OnSync("sync-msg", (args) =>
                {
                    return "pong";
                });
            }

            return View();
        }
    }
}