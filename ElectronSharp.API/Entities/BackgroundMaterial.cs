namespace ElectronSharp.API.Entities
{
    /// <summary>
    /// See the [Windows documentation](https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwm_systembackdrop_type) for more details.
    /// This method is only supported on Windows 11 22H2 and up.
    /// </summary>
    public enum BackgroundMaterial
    {
        /// <summary>
        /// Let the Desktop Window Manager (DWM) automatically decide the system-drawn backdrop material for this window. This is the default.
        /// </summary>
        auto,
        /// <summary>
        /// Don't draw any system backdrop.
        /// </summary>
        none,
        /// <summary>
        /// Draw the backdrop material effect corresponding to a long-lived window.
        /// </summary>
        mica,
        /// <summary>
        /// Draw the backdrop material effect corresponding to a transient window.
        /// </summary>
        acrylic,
        /// <summary>
        /// Draw the backdrop material effect corresponding to a window with a tabbed title bar.
        /// </summary>
        tabbed,
    }
}
