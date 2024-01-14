using MassTransit;
using MessagingApp.Messages;

namespace MessagingApp.Consumers;

public class FanoutMessageConsumer : IConsumer<IFanoutMessage>
{
    public Task Consume(ConsumeContext<IFanoutMessage> context)
    {
        Console.WriteLine($"Fanout Message Received. Message: {context.Message.Message}");
        return Task.CompletedTask;
    }
}