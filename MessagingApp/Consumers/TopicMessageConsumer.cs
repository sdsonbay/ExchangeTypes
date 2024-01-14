using MassTransit;
using MessagingApp.Messages;

namespace MessagingApp.Consumers;

public class TopicMessageConsumer : IConsumer<ITopicMessage>
{
    public Task Consume(ConsumeContext<ITopicMessage> context)
    {
        Console.WriteLine($"Topic Message Received. Message: {context.Message.Message}, Source Address: {context.SourceAddress}, Routing Key: {context.RoutingKey()}");
        return Task.CompletedTask;
    }
}