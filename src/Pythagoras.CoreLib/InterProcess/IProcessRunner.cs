using System.Diagnostics;

namespace Saorsa.Pythagoras.InterProcess;

public interface IProcessRunner
{
    Process BuildCommandProcess(string commandText);

    Task<ProcessResult> RunCommandProcessAsync(
        string commandText,
        CancellationToken cancellationToken = default);
}
