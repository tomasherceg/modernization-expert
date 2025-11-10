using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Chapter02_AllocationFreeParser;

[MemoryDiagnoser]
public class Example02
{

    public static void Run()
    {
        BenchmarkRunner.Run([typeof(Example01), typeof(Example02)],
            ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.JoinSummary));
    }


    [Benchmark]
    public void ParseWithSpans()
    {
        var content = File.ReadAllBytes("input.tsv");
        var span = new ReadOnlySpan<byte>(content);

        var lines = new List<double[]>();
        while (span.Length > 0)
        {
            // find the end of the line
            var lineSpan = AcceptUntil(ref span, (byte)'\n');

            var currentLine = new List<double>();
            while (lineSpan.Length > 0)
            {
                // find the end of the number
                var numberSpan = AcceptUntil(ref lineSpan, (byte)'\t');
                currentLine.Add(double.Parse(numberSpan));
            }
            lines.Add(currentLine.ToArray());
        }

        var result = lines.ToArray();
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
