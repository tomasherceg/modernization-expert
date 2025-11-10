using System.Buffers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Chapter02_AllocationFreeParser;

[MemoryDiagnoser]
public class Example04
{

    public static void Run()
    {
        BenchmarkRunner.Run([typeof(Example01), typeof(Example02), typeof(Example03), typeof(Example04)],
            ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.JoinSummary));
    }

    [Benchmark]
    public void ParseWithSpansWithBuffer()
    {
        using var stream = File.OpenRead("input.tsv");
        using var buffer = MemoryPool<byte>.Shared.Rent(65536);

        var lines = new List<double[]>();
        var isFinalSegment = false;
        var startIndex = 0;
        while (!isFinalSegment)
        {
            var bytesRead = stream.Read(buffer.Memory.Span.Slice(startIndex));
            isFinalSegment = bytesRead == 0;

            ReadOnlySpan<byte> span = buffer.Memory.Slice(0, bytesRead + startIndex).Span;
            while (TryParseLine(ref span, isFinalSegment) is { } currentLine)
            {
                lines.Add(currentLine);
            }

            span.CopyTo(buffer.Memory.Span);
            startIndex = span.Length;
        }

        var resultArray = lines.ToArray();
        Console.WriteLine(resultArray.Length);

        double[]? TryParseLine(ref ReadOnlySpan<byte> span, bool isFinalSegment)
        {
            if (span.IsEmpty)
            {
                return null;
            }
            var originalSpan = span;

            var lineSpan = AcceptUntil(ref span, (byte)'\n');
            if (span.IsEmpty && !isFinalSegment)
            {
                // we may not have the entire line in the current segment
                span = originalSpan;
                return null;
            }
            if (lineSpan.IsEmpty)
            {
                return Array.Empty<double>();
            }

            var numbersCount = lineSpan.Count((byte)'\t');
            var numberIndex = 0;
            var currentLine = new double[numbersCount + 1];
            while (lineSpan.Length > 0)
            {
                // find the end of the number
                var numberSpan = AcceptUntil(ref lineSpan, (byte)'\t');
                currentLine[numberIndex++] = double.Parse(numberSpan);
            }
            return currentLine;
        }


        ReadOnlySpan<byte> AcceptUntil(ref ReadOnlySpan<byte> inputSpan, byte value)
        {
            var index = inputSpan.IndexOf(value);
            if (index < 0)
            {
                var result = inputSpan;
                inputSpan = inputSpan.Slice(inputSpan.Length);
                return result;
            }
            else
            {
                var result = inputSpan.Slice(0, index);
                inputSpan = inputSpan.Slice(index + 1);
                return result;
            }
        }
    }
}
