using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElectronSharp.API.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class ProgressBarOptions
    {
        /// <summary>
        /// Mode for the progress bar. Can be 'none' | 'normal' | 'indeterminate' | 'error' | 'paused'.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ProgressBarMode Mode { get; set; }
    }
}