using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Chapter02_AllocationFreeParser;

[MemoryDiagnoser]
public class Example03
{
    public static void Run()
    {
        BenchmarkRunner.Run([typeof(Example01), typeof(Example02), typeof(Example03)], 
            ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.JoinSummary));
    }


    [Benchmark]
    public void ParseWithSpansWithoutLists()
    {
        var content = File.ReadAllBytes("input.tsv");
        var span = new ReadOnlySpan<byte>(content);

        var linesCount = span.Count((byte)'\n');
        var lineIndex = 0;
        var lines = new double[linesCount + 1][];
        while (span.Length > 0)
        {
            // find the end of the line
            var lineSpan = AcceptUntil(ref span, (byte)'\n');

            var numbersCount = lineSpan.Count((byte)'\t');
            var numberIndex = 0;
            var currentLine = new double[numbersCount + 1];
            while (lineSpan.Length > 0)
            {
                // find the end of the number
                var numberSpan = AcceptUntil(ref lineSpan, (byte)'\t');
                currentLine[numberIndex++] = double.Parse(numberSpan);
            }
            lines[lineIndex++] = currentLine;
        }

        var result = lines;
        Console.WriteLine(result.Length);
    }

    private ReadOnlySpan<byte> AcceptUntil(ref ReadOnlySpan<byte> inputSpan, byte value)
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
