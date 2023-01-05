using ElectronSharp.API;
using ElectronSharp.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ElectronSharp.SampleApp.Controllers
{
    public class HostHookController : Controller
    {
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.On("start-hoosthook", async (args) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    var options = new OpenDialogOptions
                    {
                        Properties = new OpenDialogProperty[] 
                        {
                            OpenDialogProperty.openDirectory
                        }
                    };
                    var folderPath = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);

                    var resultFromTypeScript = await Electron.HostHook.CallAsync<string>("create-excel-file", folderPath);
                    Electron.IpcMain.Send(mainWindow, "excel-file-created", resultFromTypeScript);
                });
            }

            return View();
        }
    }
}
