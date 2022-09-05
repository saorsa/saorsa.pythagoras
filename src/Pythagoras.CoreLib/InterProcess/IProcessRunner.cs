using System.Diagnostics;

namespace Saorsa.Pythagoras.InterProcess;

public interface IProcessRunner
{
    Process BuildBashCommand(string commandText);

    Task<ProcessResult> RunBashCommandAsync(
        string commandText,
        CancellationToken cancellationToken = default);
}
