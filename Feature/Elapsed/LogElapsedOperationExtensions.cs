using Microsoft.Extensions.Logging;

namespace IvTem.Logging.Elapsed;

public static class LogElapsedOperationExtensions
{
    public static IDisposable LogElapsed(
        this ILogger logger, string messageTemplate, params object[] args)
    {
        return LogElapsed(logger, LogLevel.Information, messageTemplate, args);
    }
    
    public static IDisposable LogElapsed(
        this ILogger logger, LogLevel logLevel, string messageTemplate, params object[] args)
    {
        return new LogElapsedOperation(logger, logLevel, messageTemplate, args);
    }
}