namespace ElectronSharp.API.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class ClearStorageDataOptions
    {
        /// <summary>
        /// Should follow window.location.origin’s representation scheme://host:port.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// The types of storages to clear, can contain: appcache, cookies, filesystem,
        /// indexdb, localstorage, shadercache, websql, serviceworkers, cachestorage.
        /// </summary>
        public string[] Storages { get; set; }
    }
}