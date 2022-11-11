using System;

namespace Framework461
{
    class MyClass
    {
        public Id Id { get; set; }

        public Int32 Value { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var id = Id.New();

            var id2 = new Id(id.Timestamp, id.B, id.C);
            var id3 = new Id(id.Timestamp, id.Machine, id.Pid, id.Increment);

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

            var ids = System.Text.Json.JsonSerializer.Serialize(id);

            var idd = System.Text.Json.JsonSerializer.Deserialize<Id>(ids);

            if (!idd.Equals(id)) throw new InvalidOperationException();

            var obj = new MyClass { Id = id, Value = 123 };

            var objs = System.Text.Json.JsonSerializer.Serialize(obj);

            obj = System.Text.Json.JsonSerializer.Deserialize<MyClass>(objs);

            if (obj == null || !obj.Id.Equals(id)) throw new InvalidOperationException();
        }
    }
}