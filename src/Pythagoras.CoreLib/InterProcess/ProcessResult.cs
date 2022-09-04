namespace Saorsa.Pythagoras.InterProcess;

public class ProcessResult
{
    public int ProcessId { get; set; }
    
    public int? ExitCode { get; set; }

    public bool HasExited => ExitCode.HasValue;

    public bool IsFault => HasExited && ExitCode != 0;

    public bool IsSuccess => HasExited && ExitCode == 0;
    
    public DateTimeOffset? StartedAt { get; set; }
    
    public DateTimeOffset? FinishedAt { get; set; }
    
    public Exception? RuntimeException { get; set; }

    public string ErrorOutput { get; set; } = string.Empty;

    public string StandardOutput { get; set; } = string.Empty;
}
