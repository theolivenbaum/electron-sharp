namespace ElectronSharp.API.Entities
{
    public class GPUInfo
    {
        public AuxAttributes auxAttributes { get; set; }
        public GpuDevice[]   gpuDevice     { get; set; }

        public class AuxAttributes
        {
            public bool                          amdSwitchable                          { get; set; }
            public bool                          canSupportThreadedTextureMailbox       { get; set; }
            public string                        displayType                            { get; set; }
            public string                        dx12FeatureLevel                       { get; set; }
            public string                        glExtensions                           { get; set; }
            public string                        glImplementationParts                  { get; set; }
            public string                        glRenderer                             { get; set; }
            public int                           glResetNotificationStrategy            { get; set; }
            public string                        glVendor                               { get; set; }
            public string                        glVersion                              { get; set; }
            public string                        glWsExtensions                         { get; set; }
            public string                        glWsVendor                             { get; set; }
            public string                        glWsVersion                            { get; set; }
            public bool                          inProcessGpu                           { get; set; }
            public float                         initializationTime                     { get; set; }
            public bool                          isAsan                                 { get; set; }
            public bool                          isClangCoverage                        { get; set; }
            public bool                          jpegDecodeAcceleratorSupported         { get; set; }
            public string                        maxMsaaSamples                         { get; set; }
            public bool                          optimus                                { get; set; }
            public OverlayInfo                   overlayInfo                            { get; set; }
            public bool                          passthroughCmdDecoder                  { get; set; }
            public string                        pixelShaderVersion                     { get; set; }
            public bool                          sandboxed                              { get; set; }
            public bool                          subpixelFontRendering                  { get; set; }
            public bool                          supportsD3dSharedImages                { get; set; }
            public bool                          supportsDx12                           { get; set; }
            public bool                          supportsVulkan                         { get; set; }
            public int                           targetCpuBits                          { get; set; }
            public string                        vertexShaderVersion                    { get; set; }
            public VideoDecodeAcceleratorProfile videoDecodeAcceleratorSupportedProfile { get; set; }
            public VideoEncodeAcceleratorProfile videoEncodeAcceleratorSupportedProfile { get; set; }
            public int                           visibilityCallbackCallCount            { get; set; }
            public string                        vulkanVersion                          { get; set; }
        }

        public class OverlayInfo
        {
            public bool   directComposition  { get; set; }
            public string nv12OverlaySupport { get; set; }
            public bool   supportsOverlays   { get; set; }
            public string yuy2OverlaySupport { get; set; }
        }

        public class VideoDecodeAcceleratorProfile
        {
            public bool encrypted_only      { get; set; }
            public int  maxResolutionHeight { get; set; }
            public int  maxResolutionWidth  { get; set; }
            public int  minResolutionHeight { get; set; }
            public int  minResolutionWidth  { get; set; }
            public int  profile             { get; set; }
        }

        public class VideoEncodeAcceleratorProfile
        {
            public int maxFramerateDenominator { get; set; }
            public int maxFramerateNumerator   { get; set; }
            public int maxResolutionHeight     { get; set; }
            public int maxResolutionWidth      { get; set; }
            public int minResolutionHeight     { get; set; }
            public int minResolutionWidth      { get; set; }
            public int profile                 { get; set; }
        }

        public class GpuDevice
        {
            public bool   active                     { get; set; }
            public int    cudaComputeCapabilityMajor { get; set; }
            public int    deviceId                   { get; set; }
            public string driverVendor               { get; set; }
            public string driverVersion              { get; set; }
            public int    gpuPreference              { get; set; }
            public int    revision                   { get; set; }
            public int    subSysId                   { get; set; }
            public int    vendorId                   { get; set; }
        }
    }
}