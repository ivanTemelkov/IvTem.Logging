using Microsoft.Extensions.Logging;

namespace IvTem.Logging.Elapsed;

public static class LogElapsedDisposableExtensions
{
    public static ILogElapsed BeginLogElapsed(
        this ILogger logger, string messageTemplate, params object[] args)
    {
        return BeginLogElapsed(logger, LogLevel.Information, messageTemplate, args);
    }
    
    public static ILogElapsed BeginLogElapsed(
        this ILogger logger, LogLevel logLevel, string messageTemplate, params object[] args)
    {
        return new LogElapsedDisposable(logger, logLevel, messageTemplate, args);
    }
}