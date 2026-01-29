using ElectronSharp.API.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace ElectronSharp.API
{
    /// <summary>
    /// Manage browser sessions, cookies, cache, proxy settings, etc.
    /// <see href="https://www.electronjs.org/docs/api/session"/>
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; private set; }

        /// <summary>
        /// Query and modify a session's cookies.
        /// <see href="https://www.electronjs.org/docs/api/cookies"/>
        /// </summary>
        public Cookies Cookies { get; }

        /// <summary>
        /// Manage Chrome extensions.
        /// </summary>
        public SessionExtensions Extensions { get; }

        internal Session(int id)
        {
            Id      = id;
            Cookies = new Cookies(id);
            Extensions = new SessionExtensions(id);
        }

        /// <summary>
        /// Dynamically sets whether to always send credentials for HTTP NTLM or Negotiate authentication.
        /// <see href="https://www.electronjs.org/docs/api/session#sesallowntlmcredentialsfordomainsdomains"/>
        /// </summary>
        /// <param name="domains">A comma-separated list of servers for which integrated authentication is enabled.</param>
        public void AllowNTLMCredentialsForDomains(string domains)
        {
            BridgeConnector.Emit("webContents-session-allowNTLMCredentialsForDomains", Id, domains);
        }

        /// <summary>
        /// Clears the session’s HTTP authentication cache.
        /// <see href="https://www.electronjs.org/docs/api/session#sesclearauthcacheoptions"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task ClearAuthCacheAsync(RemovePassword options)
        {
            var    taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On("webContents-session-clearAuthCache-completed" + guid, () =>
            {
                BridgeConnector.Off("webContents-session-clearAuthCache-completed" + guid);
                taskCompletionSource.SetResult(null);
            });

            BridgeConnector.Emit("webContents-session-clearAuthCache", Id, options, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Clears the session’s HTTP authentication cache.
        /// <see href="https://www.electronjs.org/docs/api/session#sesclearauthcacheoptions"/>
        /// </summary>
        public Task ClearAuthCacheAsync()
        {
            var    taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On("webContents-session-clearAuthCache-completed" + guid, () =>
            {
                BridgeConnector.Off("webContents-session-clearAuthCache-completed" + guid);
                taskCompletionSource.SetResult(null);
            });

            BridgeConnector.Emit("webContents-session-clearAuthCache", Id, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Clears the session’s HTTP cache.
        /// <see href="https://www.electronjs.org/docs/api/session#sesclearcache"/>
        /// </summary>
        /// <returns></returns>
        public Task ClearCacheAsync()
        {
            var    taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On("webContents-session-clearCache-completed" + guid, () =>
            {
                BridgeConnector.Off("webContents-session-clearCache-completed" + guid);
                taskCompletionSource.SetResult(null);
            });

            BridgeConnector.Emit("webContents-session-clearCache", Id, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Clears the host resolver cache.
        /// <see href="https://www.electronjs.org/docs/api/session#sesclearhostresolvercache"/>
        /// </summary>
        /// <returns></returns>
        public Task ClearHostResolverCacheAsync()
        {
            var    taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On("webContents-session-clearHostResolverCache-completed" + guid, () =>
            {
                BridgeConnector.Off("webContents-session-clearHostResolverCache-completed" + guid);
                taskCompletionSource.SetResult(null);
            });

            BridgeConnector.Emit("webContents-session-clearHostResolverCache", Id, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Clears the data of web storages.
        /// <see href="https://www.electronjs.org/docs/api/session#sesclearstoragedataoptions"/>
        /// </summary>
        /// <returns></returns>
        public Task ClearStorageDataAsync()
        {
            var    taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On("webContents-session-clearStorageData-completed" + guid, () =>
            {
                BridgeConnector.Off("webContents-session-clearStorageData-completed" + guid);
                taskCompletionSource.SetResult(null);
            });

            BridgeConnector.Emit("webContents-session-clearStorageData", Id, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Clears the data of web storages.
        /// <see href="https://www.electronjs.org/docs/api/session#sesclearstoragedataoptions"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Task ClearStorageDataAsync(ClearStorageDataOptions options)
        {
            var    taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On("webContents-session-clearStorageData-options-completed" + guid, () =>
            {
                BridgeConnector.Off("webContents-session-clearStorageData-options-completed" + guid);
                taskCompletionSource.SetResult(null);
            });

            BridgeConnector.Emit("webContents-session-clearStorageData-options", Id, options, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Allows resuming cancelled or interrupted downloads from previous Session. The
        /// API will generate a DownloadItem that can be accessed with the will-download
        /// event. The DownloadItem will not have any WebContents associated with it and the
        /// initial state will be interrupted. The download will start only when the resume
        /// API is called on the DownloadItem.
        /// <see href="https://www.electronjs.org/docs/api/session#sescreateinterrupteddownloadoptions"/>
        /// </summary>
        /// <param name="options"></param>
        public void CreateInterruptedDownload(CreateInterruptedDownloadOptions options)
        {
            BridgeConnector.Emit("webContents-session-createInterruptedDownload", Id, options);
        }

        /// <summary>
        /// Disables any network emulation already active for the session. Resets to the
        /// original network configuration.
        /// <see href="https://www.electronjs.org/docs/api/session#sesdisablenetworkemulation"/>
        /// </summary>
        public void DisableNetworkEmulation()
        {
            BridgeConnector.Emit("webContents-session-disableNetworkEmulation", Id);
        }

        /// <summary>
        /// Emulates network with the given configuration for the session.
        /// <see href="https://www.electronjs.org/docs/api/session#sesenablenetworkemulationoptions"/>
        /// </summary>
        /// <param name="options"></param>
        public void EnableNetworkEmulation(EnableNetworkEmulationOptions options)
        {
            BridgeConnector.Emit("webContents-session-enableNetworkEmulation", Id, options);
        }

        /// <summary>
        /// Writes any unwritten DOMStorage data to disk.
        /// <see href="https://www.electronjs.org/docs/api/session#sesflushstoragedata"/>
        /// </summary>
        public void FlushStorageData()
        {
            BridgeConnector.Emit("webContents-session-flushStorageData", Id);
        }

        /// <summary>
        /// 
        /// <see href="https://www.electronjs.org/docs/api/session#sesgetblobdataidentifier"/>
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Task<int[]> GetBlobDataAsync(string identifier)
        {
            var    taskCompletionSource = new TaskCompletionSource<int[]>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On<int[]>("webContents-session-getBlobData-completed" + guid, (buffer) =>
            {
                BridgeConnector.Off("webContents-session-getBlobData-completed" + guid);
                taskCompletionSource.SetResult(buffer);
            });

            BridgeConnector.Emit("webContents-session-getBlobData", Id, identifier, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Get session's current cache size.
        /// <see href="https://www.electronjs.org/docs/api/session#sesgetcachesize"/>
        /// </summary>
        /// <returns>Callback is invoked with the session's current cache size.</returns>
        public Task<int> GetCacheSizeAsync()
        {
            var    taskCompletionSource = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On<int>("webContents-session-getCacheSize-completed" + guid, (size) =>
            {
                BridgeConnector.Off("webContents-session-getCacheSize-completed" + guid);
                taskCompletionSource.SetResult(size);
            });

            BridgeConnector.Emit("webContents-session-getCacheSize", Id, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Deprecated. Use GetPreloadScriptsAsync instead.
        /// <see href="https://www.electronjs.org/docs/api/session#sesgetpreloads"/>
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use GetPreloadScriptsAsync instead")]
        public Task<string[]> GetPreloadsAsync()
        {
            var    taskCompletionSource = new TaskCompletionSource<string[]>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On<string[]>("webContents-session-getPreloads-completed" + guid, (preloads) =>
            {
                BridgeConnector.Off("webContents-session-getPreloads-completed" + guid);
                taskCompletionSource.SetResult(preloads);
            });

            BridgeConnector.Emit("webContents-session-getPreloads", Id, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Returns an array of registered preload scripts.
        /// <see href="https://www.electronjs.org/docs/api/session#sesgetpreloadscripts"/>
        /// </summary>
        public Task<PreloadScript[]> GetPreloadScriptsAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<PreloadScript[]>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid = Guid.NewGuid().ToString();

            BridgeConnector.On<PreloadScript[]>("webContents-session-getPreloadScripts-completed" + guid, (scripts) =>
            {
                BridgeConnector.Off("webContents-session-getPreloadScripts-completed" + guid);
                taskCompletionSource.SetResult(scripts);
            });

            BridgeConnector.Emit("webContents-session-getPreloadScripts", Id, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// 
        /// <see href="https://www.electronjs.org/docs/api/session#sesgetuseragent"/>
        /// </summary>
        /// <returns></returns>
        public Task<string> GetUserAgent()
        {
            var    taskCompletionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On<string>("webContents-session-getUserAgent-completed" + guid, (userAgent) =>
            {
                BridgeConnector.Off("webContents-session-getUserAgent-completed" + guid);
                taskCompletionSource.SetResult(userAgent.ToString());
            });

            BridgeConnector.Emit("webContents-session-getUserAgent", Id, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Resolves the proxy information for url. The callback will be called with
        /// callback(proxy) when the request is performed.
        /// <see href="https://www.electronjs.org/docs/api/session#sesresolveproxyurl"/>
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<string> ResolveProxyAsync(string url)
        {
            var    taskCompletionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On<string>("webContents-session-resolveProxy-completed" + guid, (proxy) =>
            {
                BridgeConnector.Off("webContents-session-resolveProxy-completed" + guid);
                taskCompletionSource.SetResult(proxy.ToString());
            });

            BridgeConnector.Emit("webContents-session-resolveProxy", Id, url, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Sets download saving directory. By default, the download directory will be the
        /// Downloads under the respective app folder.
        /// <see href="https://www.electronjs.org/docs/api/session#sessetdownloadpathpath"/>
        /// </summary>
        /// <param name="path"></param>
        public void SetDownloadPath(string path)
        {
            BridgeConnector.Emit("webContents-session-setDownloadPath", Id, path);
        }

        /// <summary>
        /// Deprecated. Use RegisterPreloadScript instead.
        /// Adds scripts that will be executed on ALL web contents that are associated with
        /// this session just before normal preload scripts run.
        /// <see href="https://www.electronjs.org/docs/api/session#sessetpreloadspreloads"/>
        /// </summary>
        /// <param name="preloads"></param>
        [Obsolete("Use RegisterPreloadScript instead")]
        public void SetPreloads(string[] preloads)
        {
            BridgeConnector.Emit("webContents-session-setPreloads", Id, preloads);
        }

        /// <summary>
        /// Registers a preload script.
        /// <see href="https://www.electronjs.org/docs/api/session#sesregisterpreloadscriptoptions"/>
        /// </summary>
        public void RegisterPreloadScript(RegisterPreloadScriptOptions options)
        {
            BridgeConnector.Emit("webContents-session-registerPreloadScript", Id, options);
        }

        /// <summary>
        /// Unregisters a preload script.
        /// <see href="https://www.electronjs.org/docs/api/session#sesunregisterpreloadscriptid"/>
        /// </summary>
        public void UnregisterPreloadScript(string id)
        {
            BridgeConnector.Emit("webContents-session-unregisterPreloadScript", Id, id);
        }

        /// <summary>
        /// Sets the proxy settings. When pacScript and proxyRules are provided together,
        /// the proxyRules option is ignored and pacScript configuration is applied.
        /// <see href="https://www.electronjs.org/docs/api/session#sessetproxyconfig"/>
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Task SetProxyAsync(ProxyConfig config)
        {
            var    taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            string guid                 = Guid.NewGuid().ToString();

            BridgeConnector.On("webContents-session-setProxy-completed" + guid, () =>
            {
                BridgeConnector.Off("webContents-session-setProxy-completed" + guid);
                taskCompletionSource.SetResult(null);
            });

            BridgeConnector.Emit("webContents-session-setProxy", Id, config, guid);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Overrides the userAgent for this session. This doesn't affect existing WebContents, and
        /// each WebContents can use webContents.setUserAgent to override the session-wide
        /// user agent.
        /// <see href="https://www.electronjs.org/docs/api/session#sessetuseragentuseragent-acceptlanguages"/>
        /// </summary>
        /// <param name="userAgent"></param>
        public void SetUserAgent(string userAgent)
        {
            BridgeConnector.Emit("webContents-session-setUserAgent", Id, userAgent);
        }

        /// <summary>
        /// Overrides the userAgent and acceptLanguages for this session. The
        /// acceptLanguages must a comma separated ordered list of language codes, for
        /// example "en-US,fr,de,ko,zh-CN,ja". This doesn't affect existing WebContents, and
        /// each WebContents can use webContents.setUserAgent to override the session-wide
        /// user agent.
        /// <see href="https://www.electronjs.org/docs/api/session#sessetuseragentuseragent-acceptlanguages"/>
        /// </summary>
        /// <param name="userAgent"></param>
        /// <param name="acceptLanguages">The
        /// acceptLanguages must a comma separated ordered list of language codes, for
        /// example "en-US,fr,de,ko,zh-CN,ja".</param>
        public void SetUserAgent(string userAgent, string acceptLanguages)
        {
            BridgeConnector.Emit("webContents-session-setUserAgent", Id, userAgent, acceptLanguages);
        }

        /// <summary>
        /// Deprecated. Use Extensions.GetAllAsync instead.
        /// The keys are the extension names and each value is an object containing name and version properties.
        /// Note: This API cannot be called before the ready event of the app module is emitted.
        /// <see href="https://www.electronjs.org/docs/api/session#sesgetallextensions"/>
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use Extensions.GetAllAsync instead")]
        public Task<ChromeExtensionInfo[]> GetAllExtensionsAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<ChromeExtensionInfo[]>(TaskCreationOptions.RunContinuationsAsynchronously);

            BridgeConnector.On<ChromeExtensionInfo[]>("webContents-session-getAllExtensions-completed" + Id, (extensionslist) =>
            {
                BridgeConnector.Off("webContents-session-getAllExtensions-completed" + Id);
                taskCompletionSource.SetResult(extensionslist);
            });

            BridgeConnector.Emit("webContents-session-getAllExtensions", Id);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Deprecated. Use Extensions.Remove instead.
        /// Remove Chrome extension with the specified name.
        /// Note: This API cannot be called before the ready event of the app module is emitted.
        /// <see href="https://www.electronjs.org/docs/api/session#sesremoveextensionname"/>
        /// </summary>
        /// <param name="name">Name of the Chrome extension to remove</param>
        [Obsolete("Use Extensions.Remove instead")]
        public void RemoveExtension(string name)
        {
            BridgeConnector.Emit("webContents-session-removeExtension", Id, name);
        }

        /// <summary>
        /// Deprecated. Use Extensions.LoadAsync instead.
        /// resolves when the extension is loaded.
        ///
        /// This method will raise an exception if the extension could not be loaded.If
        /// there are warnings when installing the extension (e.g. if the extension requests
        /// an API that Electron does not support) then they will be logged to the console.
        ///
        /// Note that Electron does not support the full range of Chrome extensions APIs.
        /// See Supported Extensions APIs for more details on what is supported.
        ///
        /// Note that in previous versions of Electron, extensions that were loaded would be
        /// remembered for future runs of the application.This is no longer the case:
        /// `loadExtension` must be called on every boot of your app if you want the
        /// extension to be loaded.
        ///
        /// This API does not support loading packed (.crx) extensions.
        ///
        ///** Note:** This API cannot be called before the `ready` event of the `app` module
        /// is emitted.
        ///
        ///** Note:** Loading extensions into in-memory(non-persistent) sessions is not supported and will throw an error.
        /// <see href="https://www.electronjs.org/docs/api/session#sesloadextensionpath-options"/>
        /// </summary>
        /// <param name="path">Path to the Chrome extension</param>
        /// <param name="allowFileAccess">Whether to allow the extension to read local files over `file://` protocol and
        /// inject content scripts into `file://` pages. This is required e.g. for loading
        /// devtools extensions on `file://` URLs. Defaults to false.</param>
        /// <returns></returns>
        [Obsolete("Use Extensions.LoadAsync instead")]
        public Task<Extension> LoadExtensionAsync(string path, bool allowFileAccess = false)
        {
            var taskCompletionSource = new TaskCompletionSource<Extension>(TaskCreationOptions.RunContinuationsAsynchronously);

            BridgeConnector.On<Extension>("webContents-session-loadExtension-completed" + Id, (extension) =>
            {
                BridgeConnector.Off("webContents-session-loadExtension-completed" + Id);

                taskCompletionSource.SetResult(extension);
            });

            BridgeConnector.Emit("webContents-session-loadExtension", Id, path, allowFileAccess);

            return taskCompletionSource.Task;
        }
    }
}