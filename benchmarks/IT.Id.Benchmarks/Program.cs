
var random = new Random(123);
var high = random.Next();
var low = random.Next();

Console.WriteLine($"{high} - {low}");

int sizeId = System.Runtime.InteropServices.Marshal.SizeOf<Id>(Id.New());

Console.WriteLine($"SizeOf Id - {sizeId} bytes");

var idb = new IT.Id.Benchmarks.IdBenchmark();

var ulid = idb.Ulid_Decode();

var bytes = ulid.ToByteArray();

var base32 = SimpleBase.Base32.Crockford.Encode(bytes);

if (!base32.Equals(idb._ulidString))
    Console.WriteLine($"Ulid '{idb._ulidString}' != Crockford base32 '{base32}'");

//var base32_2 = Wiry.Base32.Base32Encoding.Base32.GetString(bytes);

var id1 = idb.Id_Decode_HexLower();
var id2 = idb.Id_Decode_HexUpper();
var id3 = idb.Id_Decode_Base85();
var id4 = idb.Id_Decode_Path2();
var id5 = idb.Id_Decode_Path3();
var id6 = idb.Id_Decode_Base32();
var id7 = idb.Id_Decode_Base58();

//if (!idb._idBase32.Equals("CDF3X28R6E0ACQ4NEVR0")) throw new InvalidOperationException();

if (!id1.Equals(id2) || !id1.Equals(id3) || !id1.Equals(id4) || 
    !id1.Equals(id5) || !id1.Equals(id6) || !id1.Equals(id7)) throw new InvalidOperationException();

Console.WriteLine("Ok");

var id = Id.New();
var idBytes = id.ToByteArray();

//var lengths = new List<Int32>();

//for (int i = 0; i < 10000; i++)
//{
//    var id58 = Id.New();
//    var bitcoin = SimpleBase.Base58.Bitcoin.Encode(id.ToByteArray());

//    if (!lengths.Contains(bitcoin.Length)) lengths.Add(bitcoin.Length);
//}

var f1 = id.ToString(Idf.Base64Url);
var f2 = id.ToString("u64");
var f3 = $"{id:u64}";
var f4 = id.ToString();

if (!f1.Equals(f2) || !f1.Equals(f3) || !f1.Equals(f4) || !id.Equals(Id.Parse(f4)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Base64);
f2 = id.ToString("b64");
f3 = $"{id:b64}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Base85);
f2 = id.ToString("85");
f3 = $"{id:85}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Path2);
f2 = id.ToString("p2");
f3 = $"{id:p2}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Path3);
f2 = id.ToString("p3");
f3 = $"{id:p3}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Hex);
f2 = id.ToString("h");
f3 = $"{id:h}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)) || !id.Equals(Id.Parse(f3, Idf.Hex)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.HexUpper);
f2 = id.ToString("H");
f3 = $"{id:H}";

if (!f1.Equals(f2) || !f1.Equals(f3) || !id.Equals(Id.Parse(f3)) || !id.Equals(Id.Parse(f3, Idf.HexUpper)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Base32);
f2 = id.ToString("32");
f3 = $"{id:32}";
f4 = SimpleBase.Base32.Crockford.Encode(id.ToByteArray());

if (!f1.Equals(f2) || !f1.Equals(f3) || !f1.Equals(f4) || 
    !id.Equals(Id.Parse(f2)) || !id.Equals(Id.Parse(f2, Idf.Base32)))
    throw new InvalidOperationException();

f1 = id.ToString(Idf.Base58);
f2 = id.ToString("58");
//f3 = $"{id:58}";
f4 = SimpleBase.Base58.Bitcoin.Encode(id.ToByteArray());

if (!f1.Equals(f2) || !f1.Equals(f4) ||
    !id.Equals(Id.Parse(f2)) || !id.Equals(Id.Parse(f2, Idf.Base58)))
    throw new InvalidOperationException();

Console.WriteLine("Ok");

//BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(IT.Id.Benchmarks.IdBenchmark));