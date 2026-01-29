using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronSharp.SampleApp.Controllers
{
    public class NativeThemeController : Controller
    {
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.OnWithId("get-theme-source", async (info) =>
                {
                    var themeSource = await Electron.NativeTheme.GetThemeSourceAsync();

                    if (Electron.WindowManager.TryGetBrowserWindows(info.browserId, out var window))
                    {
                        Electron.IpcMain.Send(window, "got-theme-source", themeSource.ToString());
                    }
                });

                Electron.IpcMain.OnWithId("toggle-theme-source", async (info) =>
                {
                   var currentTheme = await Electron.NativeTheme.GetThemeSourceAsync();
                   var newTheme = currentTheme == ThemeSourceMode.Dark ? ThemeSourceMode.Light : ThemeSourceMode.Dark;

                   Electron.NativeTheme.SetThemeSource(newTheme);

                    if (Electron.WindowManager.TryGetBrowserWindows(info.browserId, out var window))
                    {
                        Electron.IpcMain.Send(window, "theme-source-updated", newTheme.ToString());
                    }
                });

                 Electron.IpcMain.OnWithId("reset-theme-source", async (info) =>
                {
                   Electron.NativeTheme.SetThemeSource(ThemeSourceMode.System);

                    if (Electron.WindowManager.TryGetBrowserWindows(info.browserId, out var window))
                    {
                        Electron.IpcMain.Send(window, "theme-source-updated", ThemeSourceMode.System.ToString());
                    }
                });

                Electron.IpcMain.OnWithId("is-dark-mode", async (info) =>
                {
                    var isDark = await Electron.NativeTheme.ShouldUseDarkColorsAsync();
                     if (Electron.WindowManager.TryGetBrowserWindows(info.browserId, out var window))
                    {
                        Electron.IpcMain.Send(window, "got-is-dark-mode", isDark);
                    }
                });
            }

            return View();
        }
    }
}
