using FluentAssertions;
using Moq;
using spp_lab1.TracerLibrary.Model;
using spp_lab1.TracerLibrary.Serializer.Implementation;
using spp_lab1.TracerLibrary.Tracer;
using Xunit;

namespace Tests.Test.Serializer;

public class JsonSerializerTest
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
}