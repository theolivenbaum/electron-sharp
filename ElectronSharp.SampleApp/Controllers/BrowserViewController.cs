using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ElectronSharp.API;
using ElectronSharp.API.Entities;
using System.Threading.Tasks;

namespace ElectronSharp.SampleApp.Controllers
{
    public class BrowserViewController : Controller
    {
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.OnWithId("create-browser-view", async (info) =>
                {
                    // Create a new window for this demo to avoid messing up the main window
                    var options = new BrowserWindowOptions
                    {
                        Width = 800,
                        Height = 600,
                        Title = "BrowserView Demo"
                    };

                    var win = await Electron.WindowManager.CreateWindowAsync(options);

                    var browserView = await Electron.WindowManager.CreateBrowserViewAsync();
                    win.SetBrowserView(browserView);
                    browserView.SetBounds(new Rectangle { X = 0, Y = 0, Width = 800, Height = 300 });
                    await browserView.WebContents.LoadURLAsync("https://electronjs.org");

                     if (Electron.WindowManager.TryGetBrowserWindows(info.browserId, out var mainWindow))
                    {
                        Electron.IpcMain.Send(mainWindow, "browser-view-created");
                    }
                });
            }

            return View();
        }
    }
}
