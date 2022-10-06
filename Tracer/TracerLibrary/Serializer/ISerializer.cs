using spp_lab1.TracerLibrary.Model;

namespace spp_lab1.TracerLibrary.Serializer;

public interface ISerializer
{
    string Serialize(TraceResult traceResult);
}