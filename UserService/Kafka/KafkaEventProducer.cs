using System.Text.Json;
using Confluent.Kafka;

namespace UserService.Kafka;

public class KafkaEventProducer
{
    private readonly string _bootstrapServers;
    private readonly string _topic;

    public KafkaEventProducer(string bootstrapServers, string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
    }

    public async Task SendEventAsync(int userId, string endpoint)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var eventMessage = new
        {
            user_id = userId,
            endpoint = endpoint
        };

        string message = JsonSerializer.Serialize(eventMessage);

        await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
        Console.WriteLine($"Отправлено сообщение: {message}");
    }
}