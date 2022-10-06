using Moq;
using spp_lab1.TracerLibrary.Tracer.Implementation;
using Xunit;

namespace Tests.Test.Tracer;

public class TracerTest
{
    [Fact]
    public void ShouldStartsThreadTracerWhenTracerStarts()
    {
        //given
        var mock = new Mock<ThreadTracer>();
        var tracer = new TracerImpl()
        {
            ThreadsTracers = { [Environment.CurrentManagedThreadId] = mock.Object }
        };
            
        //when
        tracer.StartTrace();

        //then
        mock.Verify(threadTracer => threadTracer.StartTrace(), Times.Once);
    }
        
    [Fact]
    public void ShouldStopsThreadTracerWhenTracerStops()
    {
        //given
        var mock = new Mock<ThreadTracer>();
        var tracer = new TracerImpl()
        {
            ThreadsTracers = { [Environment.CurrentManagedThreadId] = mock.Object }
        };
            
        //when
        tracer.StopTrace();

        //then
        mock.Verify(threadTracer => threadTracer.StopTrace(), Times.Once);
    }
}