using System.Xml.Serialization;

namespace spp_lab1.TracerLibrary.Model;

[Serializable]
public class TraceResult
{
    [XmlElement(ElementName = "ThreadResult")]
    public List<ThreadResult> ThreadResults { get; init; } = new();
}