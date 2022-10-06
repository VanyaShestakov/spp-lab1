using System.Xml;
using spp_lab1.TracerLibrary.Model;

namespace spp_lab1.TracerLibrary.Serializer.Implementation;

public class XmlSerializer : ISerializer
{
    public string Serialize(TraceResult traceResult)
    {
        var settings = new XmlWriterSettings
        {
            IndentChars = "\t",
            Indent = true
        };

        var sw = new StringWriter();
        var xw = XmlWriter.Create(sw, settings);
        
        new System.Xml.Serialization.XmlSerializer(traceResult.GetType()).Serialize(xw, traceResult);
        return sw.ToString();
    }
}