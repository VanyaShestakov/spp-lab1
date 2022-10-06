using System.Collections.Concurrent;
using System.Diagnostics;
using spp_lab1.TracerLibrary.Model;

namespace spp_lab1.TracerLibrary.Tracer.Implementation;

public class ThreadTracer 
{
    public int Id { get; init; }
    public ConcurrentStack<MethodTracer> MethodsTracers { get; init; } = new();
    public ConcurrentStack<MethodResult> MethodsResults { get; } = new();
    public MethodTracer? CurrentMethodTracer { get; set; }

    public virtual void StartTrace()
    {
        if (CurrentMethodTracer is not null)
        {
            MethodsTracers.Push(CurrentMethodTracer);
        }
        CurrentMethodTracer = new MethodTracer();
        CurrentMethodTracer.StartTrace();
    }

    public virtual void StopTrace()
    {
        if (CurrentMethodTracer is null) return;
        
        CurrentMethodTracer.StopTrace();
        var methodInfo = new MethodResult()
        {
            Name = new StackFrame(2, false)
                .GetMethod()?
                .Name,
            
            Class = new StackFrame(2, false)
                .GetMethod()?
                .ReflectedType?
                .Name,
            
            ExecutionTime = CurrentMethodTracer
                .Stopwatch
                .ElapsedMilliseconds,
            
            MethodsResults = CurrentMethodTracer
                .MethodsResults
                .ToList()
        };

        if (MethodsTracers.IsEmpty)
        {
            MethodsResults.Push(methodInfo);
            CurrentMethodTracer = null;
        }
        else
        {
            if (!MethodsTracers.TryPop(out var methodTrace)) return;
            
            CurrentMethodTracer = methodTrace;
            CurrentMethodTracer?
                .MethodsResults
                .Push(methodInfo);
        }
    }

    public ThreadResult GetTraceResult()
    {
        return new ThreadResult()
        {
            Id = Id,
            MethodsResults = MethodsResults.ToList()
        };
    }
}