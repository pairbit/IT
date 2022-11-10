using System;

namespace IT.Id.Samples.Framework461
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var id = System.Id.New();

            var id2 = new System.Id(id.Timestamp, id.B, id.C);
            var id3 = new System.Id(id.Timestamp, id.Machine, id.Pid, id.Increment);

            if (!id.Equals(id2) || !id.Equals(id3)) throw new InvalidOperationException();

            Console.WriteLine($"{"ToString",16} | {"Created",26} | {"Machine",9} | {"Pid",6} | {"Increment"}");
            Console.WriteLine($"{id} | {id.Created.ToLocalTime()} | {id.Machine,9} | {id.Pid,6} | {id.Increment}");

            Console.WriteLine();

            foreach (Idf idf in Enum.GetValues(typeof(Idf)))
            {
                Console.WriteLine($"{idf,9} -> {id.ToString(idf)}");
            }

            Console.WriteLine($"{"XXH32",9} -> {id.Hash32()}");
            Console.WriteLine($"{"XXH64",9} -> {id.Hash64()}");
        }
    }
}