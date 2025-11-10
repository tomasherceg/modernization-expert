using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Chapter02_AllocationFreeParser;

[MemoryDiagnoser]
public class Example01
{

    public static void Run()
    {
        BenchmarkRunner.Run([typeof(Example01)],
            ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.JoinSummary));
    }


    [Benchmark]
    public void ParseWithStrings()
    {
        var lines = File.ReadAllText("input.tsv");
        var numbers = lines
            .Split('\n')
            .Select(l => l.Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray())
            .ToArray();
        Console.WriteLine(numbers.Length);
    }

}
