using IvTem.Logging.Elapsed;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());


ILogger logger = loggerFactory.CreateLogger<Program>();


// Put a message describing the operation
using (logger.BeginLogElapsed("Testing BeginLogElapsed from class {Class}", nameof(Program)))
{
    // Do some actual work here
    await Task.Delay(1000);
}


