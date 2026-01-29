using ElectronSharp.API.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace ElectronSharp.API
{
    /// <summary>
    /// A BrowserView can be used to embed additional web content into a BrowserWindow. 
    /// It is like a child window, except that it is positioned relative to its owning window. 
    /// It is meant to be an alternative to the webview tag.
    /// <see href="https://www.electronjs.org/docs/api/browser-view"/>
    /// </summary>
    public class BrowserView
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; internal set; }

        /// <summary>
        /// Render and control web pages.
        /// </summary>
        public WebContents WebContents { get; internal set; }

        /// <summary>
        /// Resizes and moves the view to the supplied bounds relative to the window.
        /// 
        /// (experimental)
        /// <see href="https://www.electronjs.org/docs/api/browser-view#viewgetbounds-experimental"/>
        /// </summary>
        public Task<Rectangle> GetBoundsAsync() => BridgeConnector.OnResult<Rectangle>("browserView-getBounds", "browserView-getBounds-reply" + Id, Id);

        /// <summary>
        /// Set the bounds of the current view inside the window
        /// <see href="https://www.electronjs.org/docs/api/browser-view#viewsetboundsbounds-experimental"/>
        /// </summary>
        /// <param name="value"></param>
        public void SetBounds(Rectangle value)
        {
            BridgeConnector.Emit("browserView-setBounds", Id, value);
        }

        /// <summary>
        /// BrowserView
        /// </summary>
        internal BrowserView(int id)
        {
            Id = id;

            // Workaround: increase the Id so as not to conflict with BrowserWindow id
            // the backend detect about the value an BrowserView
            WebContents = new WebContents(id + 1000);
        }

        /// <summary>
        /// (experimental)
        /// <see href="https://www.electronjs.org/docs/api/browser-view#viewsetautoresizeoptions-experimental"/>
        /// </summary>
        /// <param name="options"></param>
        public void SetAutoResize(AutoResizeOptions options)
        {
            BridgeConnector.Emit("browserView-setAutoResize", Id, options);
        }

        /// <summary>
        /// Color in #aarrggbb or #argb form. The alpha channel is optional.
        /// 
        /// (experimental)
        /// <see href="https://www.electronjs.org/docs/api/browser-view#viewsetbackgroundcolorcolor-experimental"/>
        /// </summary>
        /// <param name="color">Color in #aarrggbb or #argb form. The alpha channel is optional.</param>
        public void SetBackgroundColor(string color)
        {
            BridgeConnector.Emit("browserView-setBackgroundColor", Id, color);
        }
    }
}