using System.Buffers;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Chapter02_AllocationFreeParser;

[MemoryDiagnoser]
public class Example05
{

    public static void Run()
    {
        BenchmarkRunner.Run([typeof(Example05)],
            ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.JoinSummary));
    }

    [Benchmark]
    public void ParseJson()
    {
        using var fs = File.OpenRead("input.json");
        using var buffer = MemoryPool<byte>.Shared.Rent(65536);

        var bytesRead = fs.Read(buffer.Memory.Span);

        var reader = new Utf8JsonReader(buffer.Memory.Span.Slice(0, bytesRead), isFinalBlock: false, default);
        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray)
        {
            throw new InvalidOperationException("Input must be an array!");
        }

        var lines = new List<double[]>();
        var currentLine = new List<double>();
        var isInsideLine = false;
        while (bytesRead > 0)
        {
            if (!reader.Read())
            {
                // the reader has reached the end of the buffer
                var remainingSpan = buffer.Memory.Span.Slice((int)reader.BytesConsumed);
                remainingSpan.CopyTo(buffer.Memory.Span);
                bytesRead = fs.Read(buffer.Memory.Span.Slice(remainingSpan.Length));
                reader = new Utf8JsonReader(buffer.Memory.Span.Slice(0, bytesRead + remainingSpan.Length), isFinalBlock: bytesRead == 0, reader.CurrentState);
                reader.Read();
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                isInsideLine = true;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                currentLine!.Add(reader.GetDouble());
            }
            else if (reader.TokenType == JsonTokenType.EndArray)
            {
                if (isInsideLine)
                {
                    lines.Add(currentLine.ToArray());
                    currentLine.Clear();
                    isInsideLine = false;
                }
                else
                {
                    break;
                }
            }
        }

        var resultArray = lines.ToArray();
        Console.WriteLine(resultArray.Length);
    }
}