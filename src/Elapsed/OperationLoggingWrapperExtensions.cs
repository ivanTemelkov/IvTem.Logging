using Microsoft.Extensions.Logging;

namespace IvTem.Logging.Elapsed;

public static class OperationLoggingWrapperExtensions
{
    public static ILoggingWrapper BeginOperation(this ILogger logger,
        string messageTemplate,
        params object[] args)
        => logger.BeginOperation(useExtensiveLog: false, messageTemplate, args);

    public static ILoggingWrapper BeginOperation(this ILogger logger,
        bool useExtensiveLog,
        string messageTemplate,
        params object[] args)
        => BeginOperation(logger, LogLevel.Information, useExtensiveLog, messageTemplate, args);

    public static ILoggingWrapper BeginOperation(this ILogger logger,
        LogLevel logLevel,
        string messageTemplate,
        params object[] args)
        => BeginOperation(logger, logLevel, useExtensiveLog: false, messageTemplate, args);

    public static ILoggingWrapper BeginOperation(this ILogger logger,
        LogLevel logLevel,
        bool useExtensiveLog,
        string messageTemplate,
        params object[] args)
        => new OperationLoggingWrapper(logger, logLevel, messageTemplate, args)
        {
            UseExtensiveLog = useExtensiveLog
        };
}