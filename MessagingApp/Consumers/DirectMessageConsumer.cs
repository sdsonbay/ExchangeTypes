using MassTransit;
using MessagingApp.Messages;

namespace MessagingApp.Consumers;

public class DirectMessageConsumer : IConsumer<IDirectMessage>
{
    public Task Consume(ConsumeContext<IDirectMessage> context)
    {
        Console.WriteLine($"Direct Message Received. Message: {context.Message.Message}");
        return Task.CompletedTask;
    }
}