using spp_lab1.TracerLibrary.Model;

namespace spp_lab1.TracerLibrary.Tracer;

public interface ITracer
{
    void StartTrace();
    
    void StopTrace();
    
    TraceResult GetTraceResult();
}