namespace ElectronSharp.API.Entities
{
    /// <summary>
    /// Represents a registered preload script.
    /// </summary>
    public class PreloadScript
    {
        /// <summary>
        /// The id of the preload script.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The type of the preload script. Can be 'frame' or 'service-worker'.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The path to the preload script.
        /// </summary>
        public string FilePath { get; set; }
    }
}
