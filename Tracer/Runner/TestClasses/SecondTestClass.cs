using spp_lab1.TracerLibrary.Tracer;

namespace spp_lab1.Runner.TestClasses;

public class SecondTestClass
{
    private readonly ITracer _tracer;

    public SecondTestClass(ITracer tracer)
    {
        _tracer = tracer;
    }
        
    public void Method2()
    {
        _tracer.StartTrace();

        for (var i = 0; i < 2; i++)
        {
            Method3();
        }
            
        _tracer.StopTrace();
    }

    private void Method3()
    {
        _tracer.StartTrace();

        for (var i = 0; i < 3; i++)
        {
            Method4();
        }
            
        _tracer.StopTrace();
    }

    private void Method4()
    {
        _tracer.StartTrace();

        for (var i = 0; i < 99999999; i++)
        {
            var b = 1 + i;
        }
            
        _tracer.StopTrace();
    }
}