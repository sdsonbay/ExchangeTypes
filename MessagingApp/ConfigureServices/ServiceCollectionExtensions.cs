using MassTransit;
using MessagingApp.Consumers;
using MessagingApp.Messages;
using RabbitMQ.Client;

namespace MessagingApp.ConfigureServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddMassTransit<IBus>(x =>
        {
            x.AddConsumer<DirectMessageConsumer>();
            x.AddConsumer<FanoutMessageConsumer>();
            x.AddConsumer<TopicMessageConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost");
                cfg.Publish<IDirectMessage>(c =>
                {
                    c.ExchangeType = ExchangeType.Direct;
                });
                cfg.Publish<ITopicMessage>(c =>
                {
                    c.ExchangeType = ExchangeType.Topic;
                });
                cfg.Publish<IFanoutMessage>(c =>
                {
                    c.ExchangeType = ExchangeType.Fanout;
                });
                
                cfg.ReceiveEndpoint("direct-queue-a", ep =>
                {
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<DirectMessageConsumer>();
                    ep.Bind<IDirectMessage>(z =>
                    {
                        z.RoutingKey = "A";
                        z.ExchangeType = ExchangeType.Direct;
                    });
                });
                cfg.ReceiveEndpoint("direct-queue-b", ep =>
                {
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<DirectMessageConsumer>();
                    ep.Bind<IDirectMessage>(z =>
                    {
                        z.RoutingKey = "B";
                        z.ExchangeType = ExchangeType.Direct;
                    });
                });
                cfg.ReceiveEndpoint("topic-queue-a", ep =>
                {
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<TopicMessageConsumer>(context);
                    ep.Bind<ITopicMessage>(x =>
                    {
                        x.RoutingKey = "order.log.mog";
                        x.ExchangeType = ExchangeType.Topic;
                    });
                });
                cfg.ReceiveEndpoint("topic-queue-b", ep =>
                {
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<TopicMessageConsumer>(context);
                    ep.Bind<ITopicMessage>(x =>
                    {
                        x.RoutingKey = "order.test.*";
                        x.ExchangeType = ExchangeType.Topic;
                    });
                });
                cfg.ReceiveEndpoint("topic-queue-c", ep =>
                {
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<TopicMessageConsumer>(context);
                    ep.Bind<ITopicMessage>(x =>
                    {
                        x.RoutingKey = "order.#";
                        x.ExchangeType = ExchangeType.Topic;
                    });
                });
                cfg.ReceiveEndpoint("topic-queue-d", ep =>
                {
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<TopicMessageConsumer>(context);
                    ep.Bind<ITopicMessage>(x =>
                    {
                        x.RoutingKey = "order.mog.log";
                        x.ExchangeType = ExchangeType.Topic;
                    });
                });
                cfg.ReceiveEndpoint("fanout-queue-a", ep =>
                {
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<FanoutMessageConsumer>(context);
                    ep.Bind<IFanoutMessage>(x =>
                    {
                        x.RoutingKey = "AA";
                        x.ExchangeType = ExchangeType.Fanout;
                    });
                });
                cfg.ReceiveEndpoint("fanout-queue-b", ep =>
                {
                    ep.ConfigureConsumeTopology = false;
                    ep.Consumer<FanoutMessageConsumer>(context);
                    ep.Bind<IFanoutMessage>(x =>
                    {
                        x.RoutingKey = "AB";
                        x.ExchangeType = ExchangeType.Fanout;
                    });
                });
            });
        });
        services.AddMassTransitHostedService();
        return services;
    }
}