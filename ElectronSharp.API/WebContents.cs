using ElectronSharp.API.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace ElectronSharp.API
{
    /// <summary>
    /// Render and control web pages.
    /// <see href="https://www.electronjs.org/docs/api/web-contents"/>
    /// </summary>
    public class WebContents
    {
        /// <summary>
        /// Gets the identifier.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsid"/>
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; private set; }

        /// <summary>
        /// Manage browser sessions, cookies, cache, proxy settings, etc.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentssession"/>
        /// </summary>
        public Session Session { get; internal set; }

        /// <summary>
        /// Emitted when the renderer process crashes or is killed.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#event-render-process-gone"/>
        /// </summary>
        public event Action<bool> OnRenderProcessGone
        {
            add
            {
                if (_renderProcessGone == null)
                {
                    BridgeConnector.On<bool>("webContents-render-process-gone" + Id, (killed) =>
                    {
                        _renderProcessGone(killed);
                    });

                    BridgeConnector.Emit("register-webContents-render-process-gone", Id);
                }
                _renderProcessGone += value;
            }
            remove
            {
                _renderProcessGone -= value;

                if (_renderProcessGone == null)
                    BridgeConnector.Off("webContents-render-process-gone" + Id);
            }
        }

        private event Action<bool> _renderProcessGone;

        /// <summary>
        /// Emitted when the navigation is done, i.e. the spinner of the tab has
        /// stopped spinning, and the onload event was dispatched.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#event-did-finish-load"/>
        /// </summary>
        public event Action OnDidFinishLoad
        {
            add
            {
                if (_didFinishLoad == null)
                {
                    BridgeConnector.On("webContents-didFinishLoad" + Id, () =>
                    {
                        _didFinishLoad();
                    });

                    BridgeConnector.Emit("register-webContents-didFinishLoad", Id);
                }
                _didFinishLoad += value;
            }
            remove
            {
                _didFinishLoad -= value;

                if (_didFinishLoad == null)
                    BridgeConnector.Off("webContents-didFinishLoad" + Id);
            }
        }

        private event Action _didFinishLoad;

        internal WebContents(int id)
        {
            Id      = id;
            Session = new Session(id);
        }

        /// <summary>
        /// Opens the devtools.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsopendevtoolsoptions"/>
        /// </summary>
        public void OpenDevTools()
        {
            BridgeConnector.Emit("webContentsOpenDevTools", Id);
        }

        /// <summary>
        /// Opens the devtools.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsopendevtoolsoptions"/>
        /// </summary>
        /// <param name="openDevToolsOptions"></param>
        public void OpenDevTools(OpenDevToolsOptions openDevToolsOptions)
        {
            BridgeConnector.Emit("webContentsOpenDevTools", Id, openDevToolsOptions);
        }

        /// <summary>
        /// Prints window's web page.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsprintoptions"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns>success</returns>
        public Task<bool> PrintAsync(PrintOptions options = null) => options is null
            ? BridgeConnector.OnResult<bool>("webContents-print", "webContents-print-completed" + Id, Id, "")
            : BridgeConnector.OnResult<bool>("webContents-print", "webContents-print-completed" + Id, Id, options);

        /// <summary>
        /// Prints window's web page as PDF with Chromium's preview printing custom
        /// settings.The landscape will be ignored if @page CSS at-rule is used in the web page. 
        /// By default, an empty options will be regarded as: Use page-break-before: always; 
        /// CSS style to force to print to a new page.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsprinttopdfoptions"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="options"></param>
        /// <returns>success</returns>
        public Task<bool> PrintToPDFAsync(string path, PrintToPDFOptions options = null) => options is null
            ? BridgeConnector.OnResult<bool>("webContents-printToPDF", "webContents-printToPDF-completed" + Id, Id, "", path)
            : BridgeConnector.OnResult<bool>("webContents-printToPDF", "webContents-printToPDF-completed" + Id, Id, options, path);

        /// <summary>
        /// Is used to get the Url of the loaded page.
        /// It's usefull if a web-server redirects you and you need to know where it redirects. For instance, It's useful in case of Implicit Authorization.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsgeturl"/>
        /// </summary>
        /// <returns>URL of the loaded page</returns>
        public Task<string> GetUrl()
        {
            return BridgeConnector.OnResult<string>("webContents-getUrl", "webContents-getUrl" + Id, Id);
        }

        /// <summary>
        /// Check if the webcontents is null (seems to happen when the Electron render process crashes) 
        /// </summary>
        /// <returns>True if the webcontent is null</returns>
        public Task<bool> IsNull()
        {
            return BridgeConnector.OnResult<bool>("webContents-isNull", "webContents-isNull" + Id, Id);
        }

        /// <summary>
        /// The async method will resolve when the page has finished loading, 
        /// and rejects if the page fails to load.
        /// 
        /// A noop rejection handler is already attached, which avoids unhandled rejection
        /// errors.
        ///
        /// Loads the `url` in the window. The `url` must contain the protocol prefix, e.g.
        /// the `http://` or `file://`. If the load should bypass http cache then use the
        /// `pragma` header to achieve it.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsloadurlurl-options"/>
        /// </summary>
        /// <param name="url"></param>
        public Task LoadURLAsync(string url)
        {
            return LoadURLAsync(url, new LoadURLOptions());
        }

        /// <summary>
        /// The async method will resolve when the page has finished loading, 
        /// and rejects if the page fails to load.
        /// 
        /// A noop rejection handler is already attached, which avoids unhandled rejection
        /// errors.
        ///
        /// Loads the `url` in the window. The `url` must contain the protocol prefix, e.g.
        /// the `http://` or `file://`. If the load should bypass http cache then use the
        /// `pragma` header to achieve it.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsloadurlurl-options"/>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="options"></param>
        public Task LoadURLAsync(string url, LoadURLOptions options)
        {
            var taskCompletionSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            BridgeConnector.On("webContents-loadURL-complete" + Id, () =>
            {
                BridgeConnector.Off("webContents-loadURL-complete" + Id);
                BridgeConnector.Off("webContents-loadURL-error" + Id);
                taskCompletionSource.SetResult();
            });

            BridgeConnector.On<string>("webContents-loadURL-error" + Id, (error) =>
            {
                BridgeConnector.Off("webContents-loadURL-error" + Id);
                BridgeConnector.Off("webContents-loadURL-complete" + Id);
                taskCompletionSource.SetException(new InvalidOperationException(error.ToString()));
            });

            BridgeConnector.Emit("webContents-loadURL", Id, url, options);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Inserts CSS into the web page.
        /// See: https://www.electronjs.org/docs/api/web-contents#contentsinsertcsscss-options
        /// Works for both BrowserWindows and BrowserViews.
        /// <see href="https://www.electronjs.org/docs/api/web-contents#contentsinsertcsscss-options"/>
        /// </summary>
        /// <param name="isBrowserWindow">Whether the webContents belong to a BrowserWindow or not (the other option is a BrowserView)</param>
        /// <param name="path">Absolute path to the CSS file location</param>
        public void InsertCSS(bool isBrowserWindow, string path)
        {
            BridgeConnector.Emit("webContents-insertCSS", Id, isBrowserWindow, path);
        }
    }
}