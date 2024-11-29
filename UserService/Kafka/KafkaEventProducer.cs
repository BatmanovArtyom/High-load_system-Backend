using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace UserService.Kafka;

public class KafkaEventProducer : IDisposable
{
    private readonly string _topic;
    private readonly IProducer<Null, string> _producer;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public KafkaEventProducer(string bootstrapServers, string topic)
    {
        _topic = topic;
        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null, 
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 
            WriteIndented = false 
        };
    }

    public async Task SendEventAsync(int userId, string endpoint)
    {
        var eventMessage = new KafkaEventMessage
        {
            UserId = userId,
            Endpoint = endpoint
        };

        var message = JsonSerializer.Serialize(eventMessage, _jsonSerializerOptions);

        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
        // Console.WriteLine($"Отправлено сообщение: {message}");
    }
    
    public void Dispose()
    {
        _producer?.Dispose();
    }
}