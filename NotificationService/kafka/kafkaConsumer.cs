using Confluent.Kafka;
using ECommerce.Shared;
using Newtonsoft.Json;

namespace NotificationService.kafka
{
    public class KafkaConsumer(IServiceScopeFactory scopeFactory, ILogger<KafkaConsumer> logger) : BackgroundService
    {
        //private readonly ILogger<KafkaConsumer> _logger;
        //public KafkaConsumer()
        //{
        //    _logger = logger;
        //}

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            return Task.Run(() =>
            {
                _ = ConsumeAsync("order-topic", stoppingToken);
            }, stoppingToken);
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting Kafka Consumer Service.");

            var config = new ConsumerConfig
            {
                GroupId = "order-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(topic);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    var order = JsonConvert.DeserializeObject<CheckoutMessage>(consumeResult.Message.Value);

                    logger.LogInformation("Received message");
                    logger.LogDebug("Processing message...");
                    Console.WriteLine($"Message received: {order}");

                }
                consumer.Close();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred in Kafka Consumer Service.");
            }
        }
    }
}
