using System.Text.Json;
using System.Xml;
using spp_lab1.TracerLibrary.Model;

namespace spp_lab1.TracerLibrary.Serializer.Implementation;

public class JsonSerializer : ISerializer
{
    public string Serialize(TraceResult traceResult)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return System.Text.Json.JsonSerializer.Serialize(traceResult, options);
    }
}
