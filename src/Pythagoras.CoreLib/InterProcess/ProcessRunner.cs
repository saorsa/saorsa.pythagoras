using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Saorsa.Pythagoras.InterProcess;


public class ProcessRunner : IProcessRunner
{
    public ILogger<ProcessRunner> Logger { get; }

    public ProcessRunner(ILogger<ProcessRunner> logger)
    {
        Logger = logger;
    }
    
    public Process BuildBashCommand(string commandText)
    {
        var escaped = commandText.Replace("\"", "\\\"");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{escaped}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            },
            EnableRaisingEvents = true,
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            Logger.LogError("PID = {ProcessId}, Error running command [{CommandText}]: {Error}", 
                process.Id,
                commandText,
                args.Data ?? string.Empty);
        };

        process.OutputDataReceived += (sender, args) =>
        {
            Logger.LogDebug("PID = {ProcessId}, Output from command [{CommandText}]: {Message}", 
                process.Id,
                commandText,
                args.Data ?? string.Empty);
        };

        process.Exited += (sender, args) =>
        {
            Logger.LogWarning("PID = {ProcessId}, Exited process for command [{CommandText}]", 
                process.Id,
                commandText);
        };

        return process;
    }

    public async Task<ProcessResult> RunBashCommandAsync(
        string commandText,
        CancellationToken cancellationToken = default)
    {
        Process? process = default;
        int? exitCode = default;
        try
        {
            process = BuildBashCommand(commandText);
            var started = process.Start();
            if (!started)
            {
                throw new InvalidOperationException(
                    $"Failed to start a new process for command '{commandText}'.");
            }

            var start = DateTimeOffset.Now;
            var error = await process.StandardError.ReadToEndAsync();
            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync(cancellationToken);
            exitCode = process.ExitCode;

            return new ProcessResult
            {
                ProcessId = process?.Id ?? -1,
                ExitCode = exitCode,
                ErrorOutput = error,
                StandardOutput = output,
                StartedAt = start,
                FinishedAt = DateTimeOffset.Now,
            };
        }
        catch (Exception e)
        {
            return new ProcessResult
            {
                ProcessId = process?.Id ?? -1,
                ExitCode = exitCode,
                RuntimeException = e,
            };
        }
        finally
        {
            process?.Close();
            process?.Dispose();
        }
    }
}
