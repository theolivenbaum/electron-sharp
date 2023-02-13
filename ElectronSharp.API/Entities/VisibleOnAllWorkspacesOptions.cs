namespace ElectronSharp.API.Entities
{
    public class VisibleOnAllWorkspacesOptions
    {
        /// <summary>
        /// visibleOnFullScreen boolean (optional) macOS - Sets whether the window should be visible above fullscreen windows.
        /// </summary>
        public bool VisibleOnFullScreen { get; set; } = false;

        /// <summary>
        /// skipTransformProcessType boolean (optional) macOS - Calling setVisibleOnAllWorkspaces will by default transform the process type 
        /// between UIElementApplication and ForegroundApplication to ensure the correct behavior. However, this will hide the window and dock
        /// for a short time every time it is called. If your window is already of type UIElementApplication, you can bypass this transformation
        /// by passing true to skipTransformProcessType.
        /// </summary>
        public bool SkipTransformProcessType { get; set; } = false;
    }
}