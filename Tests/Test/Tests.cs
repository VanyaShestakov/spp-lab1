using System.Collections.Concurrent;
using System.Diagnostics;
using FluentAssertions;
using Moq;
using spp_lab1.TracerLibrary.Model;
using spp_lab1.TracerLibrary.Serializer.Implementation;
using spp_lab1.TracerLibrary.Tracer;
using spp_lab1.TracerLibrary.Tracer.Implementation;
using Xunit;

namespace Tests.Test
{
    public class Tests
    {
        public static IEnumerable<object[]> TestTraceResults
        {
            get
            {
                yield return new object[]
                {
                    new TraceResult()
                    {
                        ThreadResults = new List<ThreadResult>()
                        {
                            new() { Id = 0 } 
                            
                        }
                        
                    } 
                    
                };
                
                yield return new object[]
                {
                    new TraceResult()
                    {
                        ThreadResults = new List<ThreadResult>()
                        {
                            new() { Id = 999 } 
                            
                        }
                        
                    } 
                    
                };
                
                yield return new object[]
                {
                    new TraceResult()
                    {
                        ThreadResults = new List<ThreadResult>()
                        {
                            new() { Id = int.MaxValue } 
                            
                        }
                        
                    } 
                    
                };
            }
        }

        [Theory]
        [MemberData(nameof(TestTraceResults))]
        public void ShouldSerializeCorrectlyWhenXmlSerializerSerialize(TraceResult testTraceInfo)
        {
            //given
            var mock = new Mock<ITracer>();
            mock.Setup(tracer => tracer.GetTraceResult())
                .Returns(testTraceInfo);
            var xmlSerializer = new XmlSerializer();

            //when
            var actual = xmlSerializer.Serialize(mock.Object.GetTraceResult());
            
            //then
            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
            
            actual.Should()
                .ContainAll(testTraceInfo.ThreadResults.Select(info => $"Id=\"{info.Id}\""));
            actual.Should()
                .ContainAll(testTraceInfo.ThreadResults.Select(info => $"TotalExecutionTime=\"{info.TotalExecutionTime}\""));
            actual.Should()
                .StartWith("<");
            actual.Should()
                .EndWith(">");
        }
        
        [Theory]
        [MemberData(nameof(TestTraceResults))]
        public void ShouldSerializeCorrectlyWhenJsonSerializerSerialize(TraceResult testTraceInfo)
        {
            //given
            var mock = new Mock<ITracer>();
            mock.Setup(tracer => tracer.GetTraceResult())
                .Returns(testTraceInfo);
            var jsonSerializer = new JsonSerializer();

            //when
            var actual = jsonSerializer.Serialize(mock.Object.GetTraceResult());
            
            //then
            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
            
            actual.Should()
                .ContainAll(testTraceInfo.ThreadResults.Select(info => $"\"Id\": {info.Id}"));
            actual.Should()
                .ContainAll(testTraceInfo.ThreadResults.Select(info => $"\"TotalExecutionTime\": {info.TotalExecutionTime}"));
            actual.Should()
                .StartWith("{");
            actual.Should()
                .EndWith("}");
        }

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
}