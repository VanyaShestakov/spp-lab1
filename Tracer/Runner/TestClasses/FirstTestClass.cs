using spp_lab1.TracerLibrary.Tracer;

namespace spp_lab1.Runner.TestClasses;

public class FirstTestClass
{
    private readonly ITracer _tracer;
    private readonly SecondTestClass _secondTestClass;
    public FirstTestClass(ITracer tracer)
    {
        _tracer = tracer;
        _secondTestClass = new SecondTestClass(_tracer);
    }

    public void Method1()
    {
        _tracer.StartTrace();

        var events = new List<WaitHandle>();

        for (var i = 0; i < 2; i++)
        {   
            var resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(
                _ =>
                {
                    _secondTestClass.Method2();
                    resetEvent.Set();
                });
            events.Add(resetEvent);
        }

        _secondTestClass.Method2(); 
        _secondTestClass.Method2();
        
        WaitHandle.WaitAll(events.ToArray());
        _tracer.StopTrace();
    }
}