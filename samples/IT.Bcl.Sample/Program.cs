using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

Console.WriteLine("Hello C# 10");

List<A> list = new()
{
    new() { Field = "a1" },
    new() { Field = "a2" }
};

//list[0].Field = "Error";

var tes = "login(displayName)";

var subs = tes[(tes.IndexOf("(") + 1)..tes.IndexOf(")")];

Console.WriteLine(subs);

var r1 = new Record("Ivan", 30);

//Error
//r1.FirstName = "sdf";

Console.WriteLine(r1);

var oBytes = Encoding.UTF8.GetBytes("Hello World");

var hexString = _Convert.ToHexString(oBytes);

Console.WriteLine(hexString);

var bytes = _Convert.FromHexString(hexString);

Console.WriteLine(oBytes.SequenceEqual(bytes));

var content = "content1";
var globalFolder = "Files";
var pathFile = GetPath("file.txt");
var dir = Path.GetDirectoryName(pathFile);

if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

await _File.WriteAllTextAsync(pathFile, content).ConfigureAwait(false);

var readContent = await _File.ReadAllTextAsync(pathFile).ConfigureAwait(false);

Console.WriteLine(readContent == content);

Console.ReadKey();

String GetPath(String fileName) => Path.Combine(Environment.CurrentDirectory, globalFolder, fileName);

class A
{
    public String Field { get; init; }
}

record Record(String FirstName, Int32 Age);