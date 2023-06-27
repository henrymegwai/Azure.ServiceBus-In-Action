// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "put your connection string here";

const string topicName = "your-topic you can always me-here";
const int maxNumberOfMessages = 3; // you can always cha=nge this your preferred max number

ServiceBusClient serviceBusClient;
ServiceBusSender serviceBusSender;

try
{
    serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
    serviceBusSender = serviceBusClient.CreateSender(topicName);

    using ServiceBusMessageBatch batch = await serviceBusSender.CreateMessageBatchAsync();
    for (int i = 1; i < maxNumberOfMessages; i++)
    {
        if (!batch.TryAddMessage(new ServiceBusMessage($"This is a messgae - {i}")))
        {
            Console.WriteLine($"Message - {i} was not added to the batch");
        }
        try
        {
            await serviceBusSender.SendMessagesAsync(batch);
            Console.WriteLine("Message Sent");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
        finally
        {
            await serviceBusSender.DisposeAsync();
            await serviceBusClient.DisposeAsync();

        }
    }
}
catch (Exception ex)
{
    throw;
}
