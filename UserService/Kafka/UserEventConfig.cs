namespace UserService.Kafka;

public class UserEventConfig
{
    public int UserId { get; set; }
    public string Endpoint { get; set; }
    public int Rpm { get; set; }
    public CancellationTokenSource CancellationTokenSource { get; set; } = new();
}
