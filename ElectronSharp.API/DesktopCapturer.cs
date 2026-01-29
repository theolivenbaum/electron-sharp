using System.Threading.Tasks;
using ElectronSharp.API.Entities;
using Newtonsoft.Json;

namespace ElectronSharp.API
{
    /// <summary>
    /// Access information about media sources that can be used to capture audio and video from the desktop using the navigator.mediaDevices.getUserMedia API.
    /// <see href="https://www.electronjs.org/docs/api/desktop-capturer"/>
    /// </summary>
    public sealed class DesktopCapturer
    {
        private static readonly object          _syncRoot = new();
        private static          DesktopCapturer _desktopCapturer;

        internal DesktopCapturer() { }

        internal static DesktopCapturer Instance
        {
            get
            {
                if (_desktopCapturer == null)
                {
                    lock (_syncRoot)
                    {
                        if (_desktopCapturer == null)
                        {
                            _desktopCapturer = new DesktopCapturer();
                        }
                    }
                }

                return _desktopCapturer;
            }
        }

        /// <summary>
        /// Starts gathering information about all available desktop media sources, and calls callback(error, sources) when finished.
        /// <see href="https://www.electronjs.org/docs/api/desktop-capturer#desktopcapturergetsourcesoptions"/>
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<DesktopCapturerSource[]> GetSourcesAsync(SourcesOption option)
        {
            return await BridgeConnector.OnResult<DesktopCapturerSource[]>("desktop-capturer-get-sources", "desktop-capturer-get-sources-result", option);
        }
    }
}