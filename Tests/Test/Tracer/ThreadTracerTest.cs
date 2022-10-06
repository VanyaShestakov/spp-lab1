using System.Collections.Concurrent;
using System.Diagnostics;
using Moq;
using spp_lab1.TracerLibrary.Model;
using spp_lab1.TracerLibrary.Tracer.Implementation;
using Xunit;

namespace Tests.Test.Tracer;

public class ThreadTracerTest
{
    [Fact]
        public void ShouldBeInitializedCurrentMethodTracerWhenThreadTracerStarts()
        {
            //given
            var mock = new Mock<MethodTracer>();
            var threadTracer = new ThreadTracer()
            {
                CurrentMethodTracer = mock.Object
            };
            
            //when
            threadTracer.StartTrace();
            
            //then
            Assert.Contains(mock.Object, threadTracer.MethodsTracers);
        }
        
        [Fact]
        
        public void ShouldStopTraceCorrectlyWhenThreadTracerStopTraceAndMethodsTracesIsNotEmptyAndCurrentMethodTracerNotNull()
        {
            //given
            var mock = new Mock<MethodTracer>();
            mock.Setup(tracer => tracer.MethodsResults)
                .Returns(() => new ConcurrentStack<MethodResult>());
            mock.Setup(tracer => tracer.Stopwatch)
                .Returns(() => new Stopwatch());
            var threadTracer = new ThreadTracer()
            {
                CurrentMethodTracer = mock.Object,
                MethodsTracers = new ConcurrentStack<MethodTracer>(new[] { mock.Object })
            };
            
            //when
            threadTracer.StopTrace();
            
            //then
            Assert.NotNull(threadTracer.CurrentMethodTracer);
            Assert.Equal(mock.Object, threadTracer.CurrentMethodTracer);
        }
        
        [Fact]
        public void ThreadTracerStopTrace_MethodsTracesIsEmptyAndCurrentMethodTracerNotNull_StopTraceCorrectly()
        {
            //given
            var mock = new Mock<MethodTracer>();
            mock.Setup(tracer => tracer.MethodsResults)
                .Returns(() => new ConcurrentStack<MethodResult>());
            mock.Setup(tracer => tracer.Stopwatch)
                .Returns(() => new Stopwatch());
            var threadTracer = new ThreadTracer()
            {
                CurrentMethodTracer = mock.Object,
            };
            
            //when
            threadTracer.StopTrace();
            
            //then
            Assert.Null(threadTracer.CurrentMethodTracer);
            Assert.NotEmpty(threadTracer.MethodsResults);
        }
}