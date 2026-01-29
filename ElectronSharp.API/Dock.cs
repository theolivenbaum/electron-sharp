using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using ElectronSharp.API.Entities;
using ElectronSharp.API.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ElectronSharp.API
{
    /// <summary>
    /// Control your app in the macOS dock.
    /// <see href="https://www.electronjs.org/docs/api/dock"/>
    /// </summary>
    [SupportedOSPlatform("macos")]
    public sealed class Dock
    {
        private static          Dock   _dock;
        private static readonly object _syncRoot = new();

        internal Dock()
        {
        }

        internal static Dock Instance
        {
            get
            {
                if (_dock == null)
                {
                    lock (_syncRoot)
                    {
                        if (_dock == null)
                        {
                            _dock = new Dock();
                        }
                    }
                }

                return _dock;
            }
        }

        /// <summary>
        /// When <see cref="DockBounceType.Critical"/> is passed, the dock icon will bounce until either the application becomes
        /// active or the request is canceled. When <see cref="DockBounceType.Informational"/> is passed, the dock icon will bounce
        /// for one second. However, the request remains active until either the application becomes active or the request is canceled.
        /// <para/>
        /// Note: This method can only be used while the app is not focused; when the app is focused it will return -1.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockbouncetype-macos"/>
        /// </summary>
        /// <param name="type">Can be critical or informational. The default is informational.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Return an ID representing the request.</returns>
        public Task<int> BounceAsync(DockBounceType type, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return BridgeConnector.OnResult<int>("dock-bounce", "dock-bounce-completed", cancellationToken, type.GetDescription());
        }

        /// <summary>
        /// Cancel the bounce of id.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockcancelbounceid-macos"/>
        /// </summary>
        /// <param name="id">Id of the request.</param>
        public void CancelBounce(int id)
        {
            BridgeConnector.Emit("dock-cancelBounce", id);
        }

        /// <summary>
        /// Bounces the Downloads stack if the filePath is inside the Downloads folder.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockdownloadfinishedfilepath-macos"/>
        /// </summary>
        /// <param name="filePath"></param>
        public void DownloadFinished(string filePath)
        {
            BridgeConnector.Emit("dock-downloadFinished", filePath);
        }

        /// <summary>
        /// Sets the string to be displayed in the dock’s badging area.
        /// <see href="https://www.electronjs.org/docs/api/dock#docksetbadgetext-macos"/>
        /// </summary>
        /// <param name="text"></param>
        public void SetBadge(string text)
        {
            BridgeConnector.Emit("dock-setBadge", text);
        }

        /// <summary>
        /// Gets the string to be displayed in the dock’s badging area.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockgetbadge-macos"/>
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The badge string of the dock.</returns>
        public Task<string> GetBadgeAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return BridgeConnector.OnResult<string>("dock-getBadge", "dock-getBadge-completed", cancellationToken);
        }

        /// <summary>
        /// Hides the dock icon.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockhide-macos"/>
        /// </summary>
        public void Hide()
        {
            BridgeConnector.Emit("dock-hide");
        }

        /// <summary>
        /// Shows the dock icon.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockshow-macos"/>
        /// </summary>
        public void Show()
        {
            BridgeConnector.Emit("dock-show");
        }

        /// <summary>
        /// Whether the dock icon is visible. The app.dock.show() call is asynchronous
        /// so this method might not return true immediately after that call.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockisvisible-macos"/>
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Whether the dock icon is visible.</returns>
        public Task<bool> IsVisibleAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return BridgeConnector.OnResult<bool>("dock-isVisible", "dock-isVisible-completed", cancellationToken);
        }

        /// <summary>
        /// Gets the dock menu items.
        /// </summary>
        /// <value>
        /// The menu items.
        /// </value>
        public IReadOnlyCollection<MenuItem> MenuItems { get { return _items.AsReadOnly(); } }
        private readonly List<MenuItem> _items = new();

        /// <summary>
        /// Sets the application's dock menu.
        /// <see href="https://www.electronjs.org/docs/api/dock#docksetmenumenu-macos"/>
        /// </summary>
        public void SetMenu(MenuItem[] menuItems)
        {
            menuItems.AddMenuItemsId();
            BridgeConnector.Emit("dock-setMenu", JArray.FromObject(menuItems, _jsonSerializer));
            _items.AddRange(menuItems);

            BridgeConnector.Off("dockMenuItemClicked");

            BridgeConnector.On<string>("dockMenuItemClicked", (id) =>
            {
                MenuItem menuItem = _items.GetMenuItem(id);
                menuItem?.Click();
            });
        }

        /// <summary>
        /// TODO: Menu (macOS) still to be implemented
        /// Gets the application's dock menu.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockgetmenu-macos"/>
        /// </summary>
        public Task<Menu> GetMenu(CancellationToken cancellationToken = default) => BridgeConnector.OnResult<Menu>("dock-getMenu", "dock-getMenu-completed", cancellationToken);

        /// <summary>
        /// Sets the image associated with this dock icon.
        /// <see href="https://www.electronjs.org/docs/api/dock#dockseticonimage-macos"/>
        /// </summary>
        /// <param name="image"></param>
        public void SetIcon(string image)
        {
            BridgeConnector.Emit("dock-setIcon", image);
        }

        private static readonly JsonSerializer _jsonSerializer = new()
        {
            ContractResolver  = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}