using System.Reflection;
using System.Xml.Serialization;

namespace spp_lab1.TracerLibrary.Model;

[Serializable]
public class ThreadResult
{
    private long _time;

    [XmlAttribute]
    public int Id { get; init; }
    

    [XmlAttribute]
    public long TotalExecutionTime
    {
        get
        {
            _time = 0;
            foreach (var methodResult in MethodsResults)
            {
                _time += methodResult.ExecutionTime;
            }

            return _time;
        }
        init => _time = value;
    }

    [XmlElement(ElementName = "MethodResult")]
    public List<MethodResult> MethodsResults { get; init; } = new();
}