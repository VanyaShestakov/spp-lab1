using System.Collections.Concurrent;
using spp_lab1.TracerLibrary.Model;

namespace spp_lab1.TracerLibrary.Tracer.Implementation;

public sealed class TracerImpl : ITracer
{
    public ConcurrentDictionary<int, ThreadTracer> ThreadsTracers { get; } = new();

    public void StartTrace()
    {
        var threadId = Environment.CurrentManagedThreadId;
        if (!ThreadsTracers.TryGetValue(threadId, out var threadTrace))
        {
            threadTrace = ThreadsTracers.GetOrAdd(threadId, new ThreadTracer() { Id = threadId });
        }
        threadTrace.StartTrace();
    }

    public void StopTrace()
    {
        var threadId = Environment.CurrentManagedThreadId;
        if (ThreadsTracers.TryGetValue(threadId, out var threadTrace))
        {
            threadTrace.StopTrace();
        }
    }

    public TraceResult GetTraceResult()
    {
        return new TraceResult()
        {
            ThreadResults = ThreadsTracers
                .Select(pair => pair.Value.GetTraceResult())
                .ToList()
        };
    }
}