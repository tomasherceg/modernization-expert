using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Chapter02_Vectorization;

public class VectorDemo
{

    public static void Run()
    {
        BenchmarkRunner.Run([typeof(VectorDemo)],
            ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.JoinSummary));

        //new VectorDemo().VectorizedCount();
    }


    private byte[] array = null!;


    [GlobalSetup]
    public void GlobalSetup()
    {
        array = File.ReadAllBytes("input.tsv");
    }


    [Benchmark]
    public int NaiveCount()
    {
        var count = 0;
        var searchedByte = (byte)'\n';

        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] == searchedByte)
            {
                count++;
            }
        }
        return count;
    }

    [Benchmark]
    public int VectorizedCount()
    {
        var count = 0;
        var searchedByte = (byte)'\n';

        var i = 0;
        if (Vector.IsHardwareAccelerated)
        {
            // build vector with searched character    (10, 10, 10, 10)
            var searchVector = new Vector<byte>(searchedByte);

            while (i + Vector<byte>.Count < array.Length)
            {
                // compare input with search vector
                // input:  ( 10,  20,  10,  40)
                // search: ( 10,  10,  10,  10)
                // result: (255,   0, 255,  0)
                var inputChunk = new Vector<byte>(array, i);
                if (Vector.EqualsAny(inputChunk, searchVector))
                {
                    for (var j = 0; j < Vector<byte>.Count; j++)
                    {
                        if (array[i + j] == searchedByte)
                        {
                            count++;
                        }
                    }
                }

                i += Vector<byte>.Count;
            }
        }

        // process the remaining elements that do not fit the array size
        for (; i < array.Length; i++)
        {
            if (array[i] == searchedByte)
            {
                count++;
            }
        }

        return count;
    }

}