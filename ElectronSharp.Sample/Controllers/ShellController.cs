using ElectronSharp.API;
using ElectronSharp.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ElectronSharp.SampleApp.Controllers
{
    public class ShellController : Controller
    {
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.On("open-file-manager", async (args) =>
                {
                    string path = await Electron.App.GetPathAsync(PathName.Home);
                    await Electron.Shell.ShowItemInFolderAsync(path);

                });

                Electron.IpcMain.On("open-ex-links", async (args) =>
                {
                    await Electron.Shell.OpenExternalAsync("https://github.com/ElectronSharp");
                });
            }

            return View();
        }
    }
}