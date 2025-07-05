using Microsoft.Extensions.Logging;

namespace IvTem.Logging.Elapsed;

public interface ILogElapsed : IDisposable
{
    void SetFailed(string errorMessage, LogLevel? logLevel = null);
    void SetFailed(Exception exception, LogLevel? logLevel = null);
    void SetFailed(LogLevel logLevel);
    void SetFailed(bool isKeepLog = false, LogLevel? logLevel = null);
}