using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronSharp.SampleApp.Controllers
{
    public class PdfController : Controller
    {
        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.On("print-pdf", async (args) =>
                {
                    BrowserWindow mainWindow = Electron.WindowManager.BrowserWindows.First();

                    var saveOptions = new SaveDialogOptions
                    {
                        Title       = "Save an PDF-File",
                        DefaultPath = await Electron.App.GetPathAsync(PathName.Documents),
                        Filters = new FileFilter[]
                        {
                            new FileFilter { Name = "PDF", Extensions = new string[] { "pdf" } }
                        }
                    };
                    var path = await Electron.Dialog.ShowSaveDialogAsync(mainWindow, saveOptions);

                    if (await mainWindow.WebContents.PrintToPDFAsync(path))
                    {
                        await Electron.Shell.OpenExternalAsync("file://" + path);
                    }
                    else
                    {
                        Electron.Dialog.ShowErrorBox("Error", "Failed to create pdf file.");
                    }
                });
            }

            return View();
        }
    }
}