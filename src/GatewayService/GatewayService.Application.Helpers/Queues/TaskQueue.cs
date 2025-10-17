using System.Threading.Channels;

namespace GatewayService.Application.Helpers.Queues;

public class TaskQueue<T> where T : class
{
    private readonly Channel<T> _channel;

    public TaskQueue()
    {
        _channel = Channel.CreateUnbounded<T>();
    }

    public async ValueTask EnqueueAsync(T task)
    {
        await _channel.Writer.WriteAsync(task);
    }

    public IAsyncEnumerable<T> ReadAllAsync(CancellationToken token)
        => _channel.Reader.ReadAllAsync(token);
}