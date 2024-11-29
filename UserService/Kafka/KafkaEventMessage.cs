using System.Text.Json.Serialization;

namespace UserService.Kafka;

public class KafkaEventMessage
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("endpoint")]
    public string Endpoint { get; set; }
}