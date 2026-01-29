using ElectronSharp.API.Entities;
using System.Threading.Tasks;

namespace ElectronSharp.API
{
    /// <summary>
    /// Manage Chrome extensions.
    /// </summary>
    public class SessionExtensions
    {
        private readonly int _id;

        internal SessionExtensions(int id)
        {
            _id = id;
        }

        /// <summary>
        /// resolves when the extension is loaded.
        /// </summary>
        /// <param name="path">Path to the Chrome extension</param>
        /// <param name="allowFileAccess">Whether to allow the extension to read local files over `file://` protocol and
        /// inject content scripts into `file://` pages. This is required e.g. for loading
        /// devtools extensions on `file://` URLs. Defaults to false.</param>
        /// <returns></returns>
        public Task<Extension> LoadAsync(string path, bool allowFileAccess = false)
        {
            var taskCompletionSource = new TaskCompletionSource<Extension>(TaskCreationOptions.RunContinuationsAsynchronously);

            BridgeConnector.On<Extension>("webContents-session-loadExtension-completed" + _id, (extension) =>
            {
                BridgeConnector.Off("webContents-session-loadExtension-completed" + _id);
                taskCompletionSource.SetResult(extension);
            });

            BridgeConnector.Emit("webContents-session-loadExtension", _id, path, allowFileAccess);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Remove Chrome extension with the specified name.
        /// </summary>
        /// <param name="name">Name of the Chrome extension to remove</param>
        public void Remove(string name)
        {
            BridgeConnector.Emit("webContents-session-removeExtension", _id, name);
        }

        /// <summary>
        /// The keys are the extension names and each value is an object containing name and version properties.
        /// </summary>
        /// <returns></returns>
        public Task<ChromeExtensionInfo[]> GetAllAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<ChromeExtensionInfo[]>(TaskCreationOptions.RunContinuationsAsynchronously);

            BridgeConnector.On<ChromeExtensionInfo[]>("webContents-session-getAllExtensions-completed" + _id, (extensionslist) =>
            {
                BridgeConnector.Off("webContents-session-getAllExtensions-completed" + _id);
                taskCompletionSource.SetResult(extensionslist);
            });

            BridgeConnector.Emit("webContents-session-getAllExtensions", _id);

            return taskCompletionSource.Task;
        }
    }
}
