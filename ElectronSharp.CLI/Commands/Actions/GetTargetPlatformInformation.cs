using System;
using System.Runtime.InteropServices;

namespace ElectronSharp.CLI.Commands.Actions
{
    public static class GetTargetPlatformInformation
    {
        public struct GetTargetPlatformInformationResult
        {
            public string NetCorePublishRid      { get; set; }
            public string ElectronPackerPlatform { get; set; }

        }

        public static GetTargetPlatformInformationResult Do(string desiredPlatform, string specifiedPlatfromFromCustom)
        {
            string netCorePublishRid      = string.Empty;
            string electronPackerPlatform = string.Empty;

            switch (desiredPlatform)
            {
                case "win":
                    netCorePublishRid      = "win-x64";
                    electronPackerPlatform = "win";
                    break;
                case "osx":
                    netCorePublishRid      = "osx-x64";
                    electronPackerPlatform = "mac";
                    break;
                case "osx-arm64":
                    netCorePublishRid      = "osx-arm64";
                    electronPackerPlatform = "mac";
                    break;
                case "linux":
                    netCorePublishRid      = "linux-x64";
                    electronPackerPlatform = "linux";
                    break;
                case "linux-arm":
                    netCorePublishRid      = "linux-arm";
                    electronPackerPlatform = "linux";
                    break;
                case "linux-arm64":
                    netCorePublishRid = "linux-arm64";
                    electronPackerPlatform = "linux";
                    break;
                case "custom":
                    var splittedSpecified = specifiedPlatfromFromCustom.Split(';');
                    netCorePublishRid      = splittedSpecified[0];
                    electronPackerPlatform = splittedSpecified[1];
                    break;
                default:

                    if (desiredPlatform.StartsWith("osx.")) //Versioned osx target platform
                    {
                        netCorePublishRid      = desiredPlatform;
                        electronPackerPlatform = "mac";
                        break;
                    }

                    string currentOsPlatform = null!;
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        currentOsPlatform = "linux";
                        electronPackerPlatform = "linux";
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        currentOsPlatform = "osx";
                        electronPackerPlatform = "mac";
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        currentOsPlatform = "win";
                        electronPackerPlatform = "win";
                    }
                    if (string.IsNullOrEmpty(currentOsPlatform))
                    {
                        throw new Exception($"Unsupported OS platform: {RuntimeInformation.OSDescription}");
                    }
                    var osArchitecture = RuntimeInformation.OSArchitecture;
                    string archName = null!;
                    if (osArchitecture == Architecture.Arm64)
                    {
                        archName = "arm64";
                    }
                    else if (osArchitecture == Architecture.X64)
                    {
                        archName = "x64";
                    }
                    else if (osArchitecture == Architecture.Arm)
                    {
                        archName = "arm";
                    }
                    else if (osArchitecture == Architecture.X86)
                    {
                        archName = "x86";
                    }
                    else
                    {
                        throw new Exception($"Unsupported system architecture: {RuntimeInformation.OSArchitecture}");
                    }
                    netCorePublishRid = $"{currentOsPlatform}-{archName}";
                    break;
            }
            Console.WriteLine($"Target electron platform: {electronPackerPlatform} ({netCorePublishRid})");
            return new GetTargetPlatformInformationResult()
            {
                ElectronPackerPlatform = electronPackerPlatform,
                NetCorePublishRid      = netCorePublishRid
            };
        }
    }
}