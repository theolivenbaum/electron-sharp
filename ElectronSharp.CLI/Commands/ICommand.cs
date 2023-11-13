using System.Threading.Tasks;

namespace ElectronSharp.CLI.Commands
{
    /// <summary>
    /// Interface for commands to implement.
    /// </summary>
    public interface ICommand
    {
        Task<bool> ExecuteAsync();
    }
}