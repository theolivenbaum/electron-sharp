using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronSharp.SampleApp.Controllers
{
    public class CookiesController : Controller
    {
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.OnWithId("set-cookie", async (info) =>
                {
                    if (Electron.WindowManager.TryGetBrowserWindows(info.browserId, out var window))
                    {
                        var cookieDetails = new CookieDetails
                        {
                            Url = "http://localhost",
                            Name = "electron-sharp-test",
                            Value = "Hello World",
                            ExpirationDate = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds()
                        };

                        await window.WebContents.Session.Cookies.SetAsync(cookieDetails);
                        Electron.IpcMain.Send(window, "cookie-set", "Cookie 'electron-sharp-test' set with value 'Hello World'");
                    }
                });

                Electron.IpcMain.OnWithId("get-cookie", async (info) =>
                {
                    if (Electron.WindowManager.TryGetBrowserWindows(info.browserId, out var window))
                    {
                        var cookies = await window.WebContents.Session.Cookies.GetAsync(new CookieFilter { Name = "electron-sharp-test" });
                        var cookieValue = cookies.FirstOrDefault()?.Value ?? "Cookie not found";
                        Electron.IpcMain.Send(window, "cookie-get", cookieValue);
                    }
                });
            }

            return View();
        }
    }
}
