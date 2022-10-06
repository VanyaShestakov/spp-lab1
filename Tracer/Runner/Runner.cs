using spp_lab1.Runner.TestClasses;
using spp_lab1.TracerLibrary.Serializer.Implementation;
using spp_lab1.TracerLibrary.Tracer;
using spp_lab1.TracerLibrary.Tracer.Implementation;

namespace spp_lab1.Runner;

class Runner
{
    public static void Main(string[] args)
    {
        var tracer = new TracerImpl();
        var demo = new FirstTestClass(tracer);
        var jsonSerializer = new JsonSerializer();
        var xmlSerializer = new XmlSerializer();
        
        demo.Method1();
        var result = tracer.GetTraceResult();
        
        var json = jsonSerializer.Serialize(result);
        var xml = xmlSerializer.Serialize(result);
        
        Console.WriteLine(xml);
        Console.WriteLine(json);
    }
}
