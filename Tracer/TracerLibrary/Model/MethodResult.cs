using System.Xml.Serialization;

namespace spp_lab1.TracerLibrary.Model;

[Serializable]
public class MethodResult
{

    [XmlAttribute]
    public string? Name { get; init; }
    
    [XmlAttribute]
    public string? Class { get; init; }
    
    [XmlAttribute]
    public long ExecutionTime { get; init; }
    
    [XmlElement(ElementName = "MethodResult")]
    public List<MethodResult> MethodsResults { get; init; } = new ();
}