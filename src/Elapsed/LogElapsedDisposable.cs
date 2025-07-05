using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace IvTem.Logging.Elapsed;

// This code is heavily influenced by the Dometrain course titled "From Zero to Hero: Logging in .NET" by Nick Chapsas
// https://dometrain.com/course/from-zero-to-hero-logging-in-dotnet/

public sealed class LogElapsedDisposable : ILogElapsed
{
    private ILogger Logger { get; }
    private LogLevel LogLevel { get; set; }
    private string MessageTemplate { get; }
    private object?[] Args { get; }
    private long StartingTimestamp { get; }
    private DateTime StartDateTime { get; }

    private bool IsOperationFailed { get; set; }

    private bool IsKeepLog { get; set; }

    private Exception? Exception { get; set; }

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

    public void SetFailed(string errorMessage, LogLevel? logLevel = null) 
        => SetFailed(new MessageOnlyException(errorMessage), isKeepLog: true, logLevel);

    public void SetFailed(Exception exception, LogLevel? logLevel = null) 
        => SetFailed(exception, isKeepLog:true, logLevel);

    public void SetFailed(LogLevel logLevel) 
        => SetFailed(isKeepLog: true, logLevel);

    public void SetFailed(bool isKeepLog = false, LogLevel? logLevel = null) 
        => SetFailed(exception: null, isKeepLog, logLevel);

    private void SetFailed(Exception? exception, bool isKeepLog = false, LogLevel? logLevel = null)
    {
        Exception = exception;
        IsKeepLog = isKeepLog;
        IsOperationFailed = true;

        if (logLevel.HasValue == false)
            return;

        LogLevel = logLevel.Value;
    }

    public void Dispose()
    {
        if (IsOperationFailed && IsKeepLog == false)
            return;
        
        var delta = Stopwatch.GetElapsedTime(StartingTimestamp).TotalMilliseconds;
        var endDateTime = DateTime.UtcNow;
        
        if (IsOperationFailed == false)
        {
            LogSuccess(delta, endDateTime);
            return;
        }

        if (Exception is MessageOnlyException messageOnlyException)
        {
            LogFailedWithMessage(messageOnlyException, delta, endDateTime);
        }
        else
        {
            LogFailedWithException(delta, endDateTime);
        }
    }

    private void LogFailedWithException(double delta, DateTime endDateTime)
    {
        Args[^3] = delta;
        Args[^2] = StartDateTime;
        Args[^1] = endDateTime;
            
        Logger.Log(LogLevel, Exception,
            $"{MessageTemplate} failed in {{ElapsedMilliseconds}}ms. From {{StartDateTime:s}} to {{EndDateTime:s}}.",
            Args);
    }

    private void LogFailedWithMessage(MessageOnlyException messageOnlyException, double delta, DateTime endDateTime)
    {
        var newArgs = new object[Args.Length + 1];
            
        Array.Copy(Args, 0, newArgs, 1, Args.Length - 3);

        newArgs[^4] = messageOnlyException.Message;
        newArgs[^3] = delta;
        newArgs[^2] = StartDateTime;
        newArgs[^1] = endDateTime;
                
        Logger.Log(LogLevel,
            $"{MessageTemplate} failed message '{{ErrorMessage}}' in {{ElapsedMilliseconds}}ms. From {{StartDateTime:s}} to {{EndDateTime:s}}.", newArgs);
    }

    private void LogSuccess(double delta, DateTime endDateTime)
    {
        Args[^3] = delta;
        Args[^2] = StartDateTime;
        Args[^1] = endDateTime;
            
        Logger.Log(LogLevel,
            $"{MessageTemplate} took {{ElapsedMilliseconds}}ms. From {{StartDateTime:s}} to {{EndDateTime:s}}.",
            Args);
    }
}