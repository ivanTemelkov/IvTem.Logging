using System.Diagnostics;
using IvTem.Logging.Elapsed;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());


ILogger logger = loggerFactory.CreateLogger<Program>();

// Successful Operation
await PrepareRepository();

// Unsuccessful Operation discards logging
await GetResource("ODL001");

// Unsuccessful Operation logs Warning
await ManageResource("ODL001");

// Unsuccessful Operation logs Error and Exception
await UpdateResource("ODL001");

await ExampleWithoutHelper();

async Task PrepareRepository()
{
    using var _ = logger.BeginLogElapsed("Prepare Repository");

    await Task.Delay(1000);
}

async Task GetResource(string id)
{
    using var operationWrapper = logger.BeginLogElapsed("Get Resource with Id {Id}", id);

    try
    {
        await Task.Delay(1000);
        throw new InvalidOperationException("Just needed to break something!");
    }
    catch
    {
        operationWrapper.SetFailed(isKeepLog: false);
    }
    
}

async Task ManageResource(string id)
{
    using var operationWrapper = logger.BeginLogElapsed("Manage Resource with Id {Id}", id);

    try
    {
        await Task.Delay(1000);
        throw new InvalidOperationException("Just needed to break something!");
    }
    catch
    {
        operationWrapper.SetFailed(LogLevel.Warning);
    }
}

async Task UpdateResource(string id)
{
    using var operationWrapper = logger.BeginLogElapsed("Update Resource with Id {Id}", id);

    try
    {
        await Task.Delay(1000);
        throw new InvalidOperationException("Just needed to break something!");
    }
    catch (Exception e)
    {
        operationWrapper.SetFailed(e.Message, LogLevel.Error);
    }
}

async Task ExampleWithoutHelper()
{
    var startTimestamp = Stopwatch.GetTimestamp();

    try
    {
        await Task.Delay(1000);
    }
    finally
    {
        var delta = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
        logger.LogInformation("ExampleWithoutHelper took {ElapsedTime}ms", delta);
    }
}


