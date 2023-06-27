// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "put your connection string here";

const string queueName = "your-queuena you can always me-here"; 

ServiceBusClient serviceBusClient;
ServiceBusProcessor processor = default!;

async Task MessageHandler(ProcessMessageEventArgs processMessageEventArgs)
{
    string body = processMessageEventArgs.Message.Body.ToString();
    Console.WriteLine(body);
    await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
}

Task ErrorHandler(ProcessErrorEventArgs processMessageEventArgs)
{
    Console.WriteLine(processMessageEventArgs.Exception.ToString());
    return Task.CompletedTask;
}

serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
processor = serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions()
{

});

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Press any key to end the process");
    Console.ReadKey();

    Console.WriteLine("\n Stopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("\n Stopped receiving messages.");
}
catch (Exception ex)
{

    Console.WriteLine($"Exception: {ex.Message}");
}
finally
{
    await processor.DisposeAsync(); 
    await serviceBusClient.DisposeAsync();  
}