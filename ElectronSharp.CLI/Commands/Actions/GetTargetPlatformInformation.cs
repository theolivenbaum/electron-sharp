﻿using System;
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

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        netCorePublishRid      = $"win-x{(Environment.Is64BitOperatingSystem ? "64" : "86")}";
                        electronPackerPlatform = "win";
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        if (RuntimeInformation.OSArchitecture.Equals(Architecture.Arm64))
                        {
                            //Apple Silicon Mac:
                            netCorePublishRid      = "osx-arm64";
                            electronPackerPlatform = "mac";
                        }
                        else
                        {
                            //Intel Mac:
                            netCorePublishRid      = "osx-x64";
                            electronPackerPlatform = "mac";
                        }
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        if (RuntimeInformation.OSArchitecture.Equals(Architecture.Arm64))
                        {
                            //ARM64 device - e.g. Raspberry Pi
                            netCorePublishRid = "linux-arm64";
                            electronPackerPlatform = "linux";
                        }
                        else if (RuntimeInformation.OSArchitecture.Equals(Architecture.Arm))
                        {
                            //ARM device - e.g. Raspberry Pi 2 or 2
                            netCorePublishRid = "linux-arm";
                            electronPackerPlatform = "linux";
                        }
                        else
                        {
                            //Intel Mac:
                            netCorePublishRid = "linux-x64";
                            electronPackerPlatform = "linux";
                        }

                    }

                    break;
            }

            return new GetTargetPlatformInformationResult()
            {
                ElectronPackerPlatform = electronPackerPlatform,
                NetCorePublishRid      = netCorePublishRid
            };
        }
    }
}