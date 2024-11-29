using System.Collections.Concurrent;
using UserService.Kafka;

public class EventScheduler
{
    private readonly KafkaEventProducer _producer;
    private readonly ConcurrentDictionary<int, UserEventConfig> _userConfigs = new();

    public EventScheduler(KafkaEventProducer producer)
    {
        _producer = producer;
    }

    public void AddOrUpdateUserEvent(UserEventConfig config)
    {
        if (_userConfigs.ContainsKey(config.UserId))
        {
            _userConfigs[config.UserId].CancellationTokenSource.Cancel();
            _userConfigs[config.UserId] = config;
        }
        else
        {
            _userConfigs[config.UserId] = config;
        }

        StartEventLoop(config);
    }

    public void RemoveUserEvent(int userId)
    {
        if (_userConfigs.TryRemove(userId, out var config))
        {
            config.CancellationTokenSource.Cancel();
        }
    }

    private async void StartEventLoop(UserEventConfig config)
    {
        var interval = 60000 / config.Rpm; 

        try
        {
            while (!config.CancellationTokenSource.Token.IsCancellationRequested)
            {
                await _producer.SendEventAsync(config.UserId, config.Endpoint);
                await Task.Delay(interval, config.CancellationTokenSource.Token);
            }
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine($"Остановка отправки событий для пользователя {config.UserId}");
        }
    }
}