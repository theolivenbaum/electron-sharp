using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ElectronSharp.CLI.Commands
{
    public class VersionCommand : ICommand
    {
        public const  string               COMMAND_NAME        = "version";
        public const  string               COMMAND_DESCRIPTION = "Displays the ElectronSharp.CLI version";
        public const  string               COMMAND_ARGUMENTS   = "";
        public static IList<CommandOption> CommandOptions { get; set; } = new List<CommandOption>();

        public VersionCommand()
        {
        }

        public Task<bool> ExecuteAsync()
        {
            return Task.Run(() =>
            {
                var runtimeVersion = typeof(VersionCommand)
                   .GetTypeInfo()
                   .Assembly
                   .GetCustomAttribute<AssemblyFileVersionAttribute>();

                Console.WriteLine($"ElectronSharp.CLI Version: " + runtimeVersion.Version);

                return true;
            });
        }

    }
}