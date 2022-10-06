using System.Collections.Concurrent;
using System.Diagnostics;
using spp_lab1.TracerLibrary.Model;

namespace spp_lab1.TracerLibrary.Tracer.Implementation;

public class MethodTracer
{
    public virtual Stopwatch Stopwatch { get; } = new();
    public virtual ConcurrentStack<MethodResult> MethodsResults { get; set;} = new();

    public void StartTrace()
    {
        Stopwatch.Start();
    }

    public void StopTrace()
    {
        Stopwatch.Stop();
    }

}