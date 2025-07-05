using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace IvTem.Logging.Elapsed;

// This code is heavily influenced by the Dometrain course titled "From Zero to Hero: Logging in .NET" by Nick Chapsas
// https://dometrain.com/course/from-zero-to-hero-logging-in-dotnet/

public class LogElapsedDisposable : IDisposable
{
    private ILogger Logger { get; }
    private LogLevel LogLevel { get; }
    private string MessageTemplate { get; }
    private object?[] Args { get; }
    private long StartingTimestamp { get; }
    private DateTime StartDateTime { get; }

    public LogElapsedDisposable(ILogger logger, LogLevel logLevel, string messageTemplate, object?[] args)
    {
        Logger = logger;
        LogLevel = logLevel;
        MessageTemplate = messageTemplate;
        Args = new object[args.Length + 3];
        Array.Copy(args, Args, args.Length);
        StartDateTime = DateTime.UtcNow;
        StartingTimestamp = Stopwatch.GetTimestamp();
    }

    public void Dispose()
    {
        var delta = Stopwatch.GetElapsedTime(StartingTimestamp).TotalMilliseconds;
        var endDateTime = DateTime.UtcNow;
        Args[^3] = delta;
        Args[^2] = StartDateTime;
        Args[^1] = endDateTime;
        Logger.Log(LogLevel, $"{MessageTemplate} took {{OperationDurationMs}}ms. From {{StartDateTime:s}} to {{EndDateTime:s}}.", Args);
    }
}