namespace IT.Text.Tests;

public class TagFinderTest
{
    private static ITagFinder _tagFinder = new TagFinder();


    [SetUp]
    public void Setup()
    {
    }

    //[Test]
    //public void FirstCloseTest()
    //{
    //    //Assert.That(ML.IndexOf("</p>"), Is.EqualTo(_tagFinder.FirstClose(ML, "p")));
    //    //Assert.That(MLNS.IndexOf("</ns:p>"), Is.EqualTo(_tagFinder.FirstClose(MLNS, "p")));
    //}

    [Test]
    public void FirstOpenTest()
    {
        FirstOpenExact("<p>1</p><p>2</p>", "<p>", "p", "", StringComparison.Ordinal);
        FirstOpenExact("<ns:p>1</ns:p><ns:p>2</ns:p>", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        FirstOpenExact("<p>5</p>", "<p>", "p", "", StringComparison.Ordinal);
        FirstOpenExact("<ns:p>5</ns:p>", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        FirstOpenExact("<p>", "<p>", "p", "", StringComparison.Ordinal);
        FirstOpenExact("<ns:p>", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        FirstOpenExact("p>", "<p>", "p", "", StringComparison.Ordinal);
        FirstOpenExact("ns:p>", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        FirstOpenExact("p>---", "<p>", "p", "", StringComparison.Ordinal);
        FirstOpenExact("ns:p>---", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        FirstOpenExact("<p", "<p>", "p", "", StringComparison.Ordinal);
        FirstOpenExact("<ns:p", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        FirstOpenExact("<p----", "<p>", "p", "", StringComparison.Ordinal);
        FirstOpenExact("<ns:p----", "<ns:p>", "p", "ns", StringComparison.Ordinal);
    }

    [Test]
    public void InnerTest()
    {
        Inner("<p>1</p>", "<p", "p", "", StringComparison.Ordinal);
        Inner("<ns:p>2</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Inner("<p>", "<p>", "p", "", StringComparison.Ordinal);
        Inner("<ns:p>", "<ns:p>", "p", "", StringComparison.Ordinal);

        Inner("<body><p>3</p></body>", "<p", "p", "", StringComparison.Ordinal);
        Inner("<body><ns:p>4</ns:p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Inner("<body><p>_</p><ns:p>5</ns:p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);
        Inner("<body><ns:p>_</ns:p><p>6</p></body>", "<p", "p", "", StringComparison.Ordinal);

        InnerExact("<body><p>7</p><ns:p>_</ns:p></body>", "<p", "p", "", StringComparison.Ordinal);
        InnerExact("<body><ns:p>8</ns:p><p>_</p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Inner("<p a=1>9</p>", "<p", "p", "", StringComparison.Ordinal);
        Inner("<ns:p p=2>10</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Inner("<p><p>11</p></p>", "<p", "p", "", StringComparison.Ordinal);

        Console.WriteLine("ERROR!");

        //Для одинаковых рядом стоящих тегов использовать нельзя
        Inner("<p a=1>_</p><p a=1>Error 1</p>", "<p", "p", "", StringComparison.Ordinal);
        Inner("<ns:p p=2>_</ns:p><ns:p p=2>Error 2</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);
        Inner("<p>Error 3</p><p><p>Error 3</p></p>", "<p", "p", "", StringComparison.Ordinal);

        //Ниже приведена тяжелая ситуация, где в атрибуте тега в ковычках используется закрывающийся тег,
        //которую не возможно решить простым способом? Игнорировать все что внутри ковычек очень сложно.
        Inner("<p a=\"<p>Error 4</p>\"></p>", "<p", "p", "", StringComparison.Ordinal);
        Inner("<p a='<p>Error 5</p>'></p>", "<p", "p", "", StringComparison.Ordinal);
    }

    [Test]
    public void LastInnerTest()
    {
        LastInner("<p>1</p>", "<p", "p", "", StringComparison.Ordinal);
        LastInner("<ns:p>2</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        LastInner("<p>", "<p>", "p", "", StringComparison.Ordinal);
        LastInner("<ns:p>", "<ns:p>", "p", "", StringComparison.Ordinal);

        LastInner("<body><p>3</p></body>", "<p", "p", "", StringComparison.Ordinal);
        LastInner("<body><ns:p>4</ns:p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        LastInner("<body><p>_</p><ns:p>5</ns:p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);
        LastInner("<body><ns:p>_</ns:p><p>6</p></body>", "<p", "p", "", StringComparison.Ordinal);

        LastInnerExact("<body><p>7</p><ns:p>_</ns:p></body>", "<p", "p", "", StringComparison.Ordinal);
        LastInnerExact("<body><ns:p>8</ns:p><p>_</p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        LastInner("<p a=1>9</p>", "<p", "p", "", StringComparison.Ordinal);
        LastInner("<ns:p p=2>10</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);
        
        LastInner("<p a=1>_</p><p a=1>11</p>", "<p", "p", "", StringComparison.Ordinal);
        LastInner("<ns:p p=2>_</ns:p><ns:p p=2>12</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Console.WriteLine("ERROR!");

        //Для одинаково вложенных тегов использовать нельзя!
        LastInner("<p><p>Error 1</p></p>", "<p", "p", "", StringComparison.Ordinal);
        LastInner("<p>14</p><p><p>Error 2</p></p>", "<p", "p", "", StringComparison.Ordinal);

        //Ниже приведена тяжелая ситуация, где в атрибуте тега в ковычках используется закрывающийся тег,
        //которую не возможно решить простым способом? Игнорировать все что внутри ковычек очень сложно.
        LastInner("<p a=\"<p>Error 3</p>\"></p>", "<p", "p", "", StringComparison.Ordinal);
        LastInner("<p a='<p>Error 4</p>'></p>", "<p", "p", "", StringComparison.Ordinal);
    }

    [Test]
    public void OuterTest()
    {
        Outer("<p>1</p>", "<p", "p", "", StringComparison.Ordinal);
        Outer("<ns:p>2</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Outer("<p>", "<p", "p", "", StringComparison.Ordinal);
        Outer("<ns:p>", "<ns:p", "p", "", StringComparison.Ordinal);

        Outer("<body><p>3</p></body>", "<p", "p", "", StringComparison.Ordinal);
        Outer("<body><ns:p>4</ns:p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Outer("<body><p>_</p><ns:p>5</ns:p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);
        Outer("<body><ns:p>_</ns:p><p>6</p></body>", "<p", "p", "", StringComparison.Ordinal);

        OuterExact("<body><p>7</p><ns:p>_</ns:p></body>", "<p", "p", "", StringComparison.Ordinal);
        OuterExact("<body><ns:p>8</ns:p><p>_</p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Outer("<p a=1>9</p>", "<p", "p", "", StringComparison.Ordinal);
        Outer("<ns:p p=2>10</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Outer("<p><p>11</p></p>", "<p", "p", "", StringComparison.Ordinal);

        Console.WriteLine("ERROR!");

        //Для одинаковых рядом стоящих тегов использовать нельзя
        Outer("<p a=1>_</p><p a=1>Error 1</p>", "<p", "p", "", StringComparison.Ordinal);
        Outer("<ns:p p=2>_</ns:p><ns:p p=2>Error 2</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);
        Outer("<p>Error 3</p><p><p>Error 3</p></p>", "<p", "p", "", StringComparison.Ordinal);

        //Ниже приведена тяжелая ситуация, где в атрибуте тега в ковычках используется закрывающийся тег,
        //которую не возможно решить простым способом? Игнорировать все что внутри ковычек очень сложно.
        Outer("<p a=\"<p>Error 3</p>\"></p>", "<p", "p", "", StringComparison.Ordinal);
        Outer("<p a='<p>Error 4</p>'></p>", "<p", "p", "", StringComparison.Ordinal);
    }

    [Test]
    public void LastOuterTest()
    {
        LastOuter("<p>1</p>", "<p", "p", "", StringComparison.Ordinal);
        LastOuter("<ns:p>2</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        LastOuter("<p>", "<p", "p", "", StringComparison.Ordinal);
        LastOuter("<ns:p>", "<ns:p", "p", "", StringComparison.Ordinal);

        LastOuter("<body><p>3</p></body>", "<p", "p", "", StringComparison.Ordinal);
        LastOuter("<body><ns:p>4</ns:p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        LastOuter("<body><p>_</p><ns:p>5</ns:p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);
        LastOuter("<body><ns:p>_</ns:p><p>6</p></body>", "<p", "p", "", StringComparison.Ordinal);

        LastOuterExact("<body><p>7</p><ns:p>_</ns:p></body>", "<p", "p", "", StringComparison.Ordinal);
        LastOuterExact("<body><ns:p>8</ns:p><p>_</p></body>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        LastOuter("<p a=1>9</p>", "<p", "p", "", StringComparison.Ordinal);
        LastOuter("<ns:p p=2>10</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        LastOuter("<p a=1>_</p><p a=1>11</p>", "<p", "p", "", StringComparison.Ordinal);
        LastOuter("<ns:p p=2>_</ns:p><ns:p p=2>12</ns:p>", "<ns:p", "p", "ns", StringComparison.Ordinal);

        Console.WriteLine("ERROR!");

        //Для одинаково вложенных тегов использовать нельзя!
        LastOuter("<p><p>Error 1</p></p>", "<p", "p", "", StringComparison.Ordinal);
        LastOuter("<p>14</p><p><p>Error 2</p></p>", "<p", "p", "", StringComparison.Ordinal);

        //Ниже приведена тяжелая ситуация, где в атрибуте тега в ковычках используется закрывающийся тег,
        //которую не возможно решить простым способом? Игнорировать все что внутри ковычек очень сложно.
        LastOuter("<p a=\"<p>Error 3</p>\"></p>", "<p", "p", "", StringComparison.Ordinal);
        LastOuter("<p a='<p>Error 4</p>'></p>", "<p", "p", "", StringComparison.Ordinal);
    }

    [Test]
    public void LastOpenTest()
    {
        LastOpenExact("<p>1</p><p>2</p>", "<p>", "p", "", StringComparison.Ordinal);
        LastOpenExact("<ns:p>1</ns:p><ns:p>2</ns:p>", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        LastOpenExact("<p>5</p>", "<p>", "p", "", StringComparison.Ordinal);
        LastOpenExact("<ns:p>5</ns:p>", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        LastOpenExact("<p>", "<p>", "p", "", StringComparison.Ordinal);
        LastOpenExact("<ns:p>", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        LastOpenExact("p>", "<p>", "p", "", StringComparison.Ordinal);
        LastOpenExact("ns:p>", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        LastOpenExact("p>---", "<p>", "p", "", StringComparison.Ordinal);
        LastOpenExact("ns:p>---", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        LastOpenExact("<p", "<p>", "p", "", StringComparison.Ordinal);
        LastOpenExact("<ns:p", "<ns:p>", "p", "ns", StringComparison.Ordinal);

        LastOpenExact("<p----", "<p>", "p", "", StringComparison.Ordinal);
        LastOpenExact("<ns:p----", "<ns:p>", "p", "ns", StringComparison.Ordinal);
    }

    [Test]
    public void LastCloseTest()
    {
        //"p<p p>1</p><p a=p>2</p><p b=/p>3</p><p>4</p><p>5</p>p"
        //"p<p p>1</p><p a=p>2</p><p b=/p>3</p><p>4</p><ns:p>5</ns:p>p"

        LastClose("<p>5</p>", "</p>", "p", "", StringComparison.Ordinal);
        LastClose("<ns:p>5</ns:p>", "</ns:p>", "p", "ns", StringComparison.Ordinal);

        LastClose("</p>", "</p>", "p", "", StringComparison.Ordinal);
        LastClose("</ns:p>", "</ns:p>", "p", "ns", StringComparison.Ordinal);

        LastClose("/p>", "</p>", "p", "", StringComparison.Ordinal);
        LastClose("/ns:p>", "</ns:p>", "p", "", StringComparison.Ordinal);
        LastCloseExact("/ns:p>", "</ns:p>", "p", "ns", StringComparison.Ordinal);

        LastClose("/p>----", "</p>", "p", "", StringComparison.Ordinal);
        LastClose("/ns:p>----", "</ns:p>", "p", "", StringComparison.Ordinal);
        LastCloseExact("/ns:p>----", "</ns:p>", "p", "ns", StringComparison.Ordinal);

        LastClose("</p", "</p>", "p", "", StringComparison.Ordinal);
        LastClose("</ns:p", "</ns:p>", "p", "", StringComparison.Ordinal);
        LastCloseExact("</ns:p", "</ns:p>", "p", "ns", StringComparison.Ordinal);
    }

    //[Test]
    //public void Test1()
    //{
    //    var ml = "<p>1</p><p>2</p><p>3</p><p>4</p>";
    //    //var ml = " <MyTag>Data</MyTag> ";
    //    //var ml = "MyTag>/MyTag>_/MyTag> <MyTag>Data</MyTag> MyTag>/MyTag>_/MyTag>";
    //    //var ml = "MyTag>/MyTag>_/MyTag> <MyTag>Data</MyTag> MyTag>/MyTag>_/</MyTag>MyTag>";

    //    Assert.True(ml.LastIndexOf("</MyTag>") == _tagFinder.FindClose(ml, "MyTag"));
    //    Assert.True(ml.IndexOf("</MyTag>") == _tagFinder.FindClose(ml, "MyTag", out var ns));

    //    Assert.True(ns is null);
    //    Assert.True(ml.IndexOf("<MyTag>") == _tagFinder.FindOpen(ml, "MyTag", ns));

    //    //-1
    //    Assert.True(ml.IndexOf("</myTag>") == _tagFinder.FindClose(ml, "myTag"));
    //    Assert.True(ml.IndexOf("</myTag>") == _tagFinder.FindClose(ml, "myTag", out ns));
    //    Assert.True(ns is null);
    //    Assert.True(ml.IndexOf("<myTag>") == _tagFinder.FindOpen(ml, "myTag", ns));

    //    var ic = StringComparison.OrdinalIgnoreCase;

    //    ml = "<ns:MyTag>Data</ns:MyTag>";

    //    Assert.True(ml.IndexOf("</ns:myTag>", ic) == _tagFinder.FindClose(ml, "myTag", ic));
    //    Assert.True(ml.IndexOf("</ns:myTag>", ic) == _tagFinder.FindClose(ml, "myTag", out ns, ic));
    //    Assert.True(ns == "ns");
    //    Assert.True(ml.IndexOf("<ns:myTag>", ic) == _tagFinder.FindOpen(ml, "myTag", ns, ic));

    //    ml = "<:MyTag>Data</:MyTag>";

    //    Assert.True(ml.IndexOf("</:myTag>", ic) == _tagFinder.FindClose(ml, "myTag", ic));
    //    Assert.True(ml.IndexOf("</:myTag>", ic) == _tagFinder.FindClose(ml, "myTag", out ns, ic));
    //    Assert.True(ns == "");
    //    Assert.True(ml.IndexOf("<:myTag>", ic) == _tagFinder.FindOpen(ml, "myTag", ns, ic));

    //    ml = "<MyTag attr=1>";
    //    Assert.True(ml.IndexOf("<MyTag ") == _tagFinder.FindOpen(ml, "MyTag", null));

    //    ml = "<ns:MyTag attr=1>";
    //    Assert.True(ml.IndexOf("<ns:MyTag ") == _tagFinder.FindOpen(ml, "MyTag", "ns"));

    //    //ml = "te<MyTag>Data</MyTag>st";
    //    //Arg.Eq("test", Tag.Remove(ml, "MyTag"));

    //    //ml = "<Root><MyTag>Data</MyTag></Root>";
    //    //Arg.Eq(ml, Tag.Remove(ml, "mytag"));
    //    //Arg.Eq("<Root></Root>", Tag.Remove(ml, "mytag", ic));

    //    //ml = "<ns:MyTag xmlns:ns=''><ns2:MyTag xmlns:ns2=''><MyTag>Data</MyTag></ns2:MyTag></ns:MyTag>";
    //    //Arg.Eq("", Tag.Remove(ml, "mytag", ic));
    //    //Arg.Eq("", Tag.Remove(ml, "mytag", "ns", ic));
    //    //Arg.Eq("<ns:MyTag xmlns:ns=''></ns:MyTag>", Tag.Remove(ml, "mytag", "ns2", ic));
    //    //Arg.Eq("<ns:MyTag xmlns:ns=''><ns2:MyTag xmlns:ns2=''></ns2:MyTag></ns:MyTag>", Tag.Remove(ml, "mytag", null, ic));

    //    ml = "<MyTag>";
    //    Assert.True(0 == _tagFinder.FindOpen(ml, "MyTag", null));

    //    ml = "</MyTag>";
    //    Assert.True(0 == _tagFinder.FindClose(ml, "MyTag", null));
    //    Assert.True(0 == _tagFinder.FindClose(ml, "MyTag"));
    //    Assert.True(0 == _tagFinder.FindClose(ml, "MyTag", out ns));

    //    //ml = "<MyTag></MyTag>";
    //    //Arg.Eq("", Tag.Remove(ml, "MyTag"));
    //    //Arg.Eq("", Tag.Remove(ml, "MyTag", null));

    //    ml = "<MyTag";
    //    Assert.True(-1 == _tagFinder.FindOpen(ml, "MyTag", null));

    //    ml = "</MyTag";
    //    Assert.True(-1 == _tagFinder.FindClose(ml, "MyTag", null));
    //    Assert.True(-1 == _tagFinder.FindClose(ml, "MyTag"));
    //    Assert.True(-1 == _tagFinder.FindClose(ml, "MyTag", out ns));

    //    //ml = "<MyTag</MyTag>";
    //    //Arg.Eq(ml, Tag.Remove(ml, "MyTag"));
    //    //Arg.Eq(ml, Tag.Remove(ml, "MyTag", null));

    //    //ml = "<MyTag</MyTag>sdfsdf";
    //    //Arg.Eq(ml, Tag.Remove(ml, "MyTag"));
    //    //Arg.Eq(ml, Tag.Remove(ml, "MyTag", null));
    //}

    private static void LastClose(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var lastIndex = chars.LastIndexOf(tagFull, comparison);
        
        var ns = _tagFinder.LastClose(chars, tagName, out var index, comparison);
        Assert.That(lastIndex, Is.EqualTo(index));
        Assert.True(ns.SequenceEqual(tagNS));

        Assert.That(lastIndex, Is.EqualTo(_tagFinder.LastClose(chars, tagName, comparison)));

        Assert.That(lastIndex, Is.EqualTo(_tagFinder.LastClose(chars, tagName, tagNS, comparison)));
    }

    private static void LastCloseExact(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var lastIndex = chars.LastIndexOf(tagFull, comparison);

        Assert.That(lastIndex, Is.EqualTo(_tagFinder.LastClose(chars, tagName, tagNS, comparison)));
    }

    private static void LastOpenExact(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var lastIndex = chars.LastIndexOf(tagFull, comparison);

        Assert.That(lastIndex, Is.EqualTo(_tagFinder.LastOpen(chars, tagName, tagNS, comparison)));

        Console.WriteLine(lastIndex);
    }

    private static void Outer(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var outer = Outer(chars, tagFull, comparison);

        var ns = _tagFinder.Outer(chars, tagName, out var range, comparison);
        var outer2 = chars[range];

        Assert.True(ns.SequenceEqual(tagNS));
        Assert.True(outer.SequenceEqual(outer2));

        var outer3 = chars[_tagFinder.Outer(chars, tagName, comparison)];
        Assert.True(outer.SequenceEqual(outer3));

        Console.WriteLine(outer.ToString());
    }

    private static void OuterExact(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var outer = Outer(chars, tagFull, comparison);

        var range = _tagFinder.Outer(chars, tagName, tagNS, comparison);
        var outer2 = chars[range];

        Assert.True(outer.SequenceEqual(outer2));

        Console.WriteLine(outer.ToString());
    }

    private static void LastOuter(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var outer = LastOuter(chars, tagFull, comparison);

        var ns = _tagFinder.LastOuter(chars, tagName, out var range, comparison);
        var outer2 = chars[range];

        Assert.True(ns.SequenceEqual(tagNS));
        Assert.True(outer.SequenceEqual(outer2));

        var outer3 = chars[_tagFinder.LastOuter(chars, tagName, comparison)];
        Assert.True(outer.SequenceEqual(outer3));

        Console.WriteLine(outer.ToString());
    }

    private static void LastOuterExact(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var outer = LastOuter(chars, tagFull, comparison);

        var range = _tagFinder.LastOuter(chars, tagName, tagNS, comparison);
        var outer2 = chars[range];

        Assert.True(outer.SequenceEqual(outer2));

        Console.WriteLine(outer.ToString());
    }

    private static void Inner(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var inner = Inner(chars, tagFull, comparison);

        var ns = _tagFinder.Inner(chars, tagName, out var range, comparison);
        var inner2 = chars[range];

        Assert.True(ns.SequenceEqual(tagNS));
        Assert.True(inner.SequenceEqual(inner2));

        var inner3 = chars[_tagFinder.Inner(chars, tagName, comparison)];
        Assert.True(inner.SequenceEqual(inner3));

        Console.WriteLine(inner.ToString());
    }

    private static void InnerExact(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var inner = Inner(chars, tagFull, comparison);

        var range = _tagFinder.Inner(chars, tagName, tagNS, comparison);
        var inner2 = chars[range];

        Assert.True(inner.SequenceEqual(inner2));

        Console.WriteLine(inner.ToString());
    }

    private static void LastInner(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var inner = LastInner(chars, tagFull, comparison);

        var ns = _tagFinder.LastInner(chars, tagName, out var range, comparison);
        var inner2 = chars[range];

        Assert.True(ns.SequenceEqual(tagNS));
        Assert.True(inner.SequenceEqual(inner2));

        var inner3 = chars[_tagFinder.LastInner(chars, tagName, comparison)];
        Assert.True(inner.SequenceEqual(inner3));

        Console.WriteLine(inner.ToString());
    }

    private static void LastInnerExact(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var inner = LastInner(chars, tagFull, comparison);

        var range = _tagFinder.LastInner(chars, tagName, tagNS, comparison);
        var inner2 = chars[range];

        Assert.True(inner.SequenceEqual(inner2));

        Console.WriteLine(inner.ToString());
    }

    private static void FirstOpenExact(ReadOnlySpan<char> chars, string tagFull, string tagName, string tagNS, StringComparison comparison)
    {
        var index = chars.IndexOf(tagFull, comparison);

        Assert.That(index, Is.EqualTo(_tagFinder.FirstOpen(chars, tagName, tagNS, comparison)));

        Console.WriteLine(index);
    }

    private static ReadOnlySpan<char> LastInner(ReadOnlySpan<char> chars, string tagFull, StringComparison comparison)
    {
        var startIndex = chars.LastIndexOf(tagFull, comparison);
        var endIndex = chars.LastIndexOf(tagFull.Insert(1, "/") + ">", comparison);
        if (startIndex > -1) startIndex = chars.ToString().IndexOf(">", startIndex + 1) + 1;
        return startIndex == -1 || endIndex == -1 ? string.Empty : chars[startIndex..endIndex];
    }

    private static ReadOnlySpan<char> LastOuter(ReadOnlySpan<char> chars, string tagFull, StringComparison comparison)
    {
        var startIndex = chars.LastIndexOf(tagFull, comparison);
        var endIndex = chars.LastIndexOf(tagFull.Insert(1, "/") + ">", comparison);
        if (endIndex > -1) endIndex += tagFull.Length + 2;
        return startIndex == -1 || endIndex == -1 ? string.Empty : chars[startIndex..endIndex];
    }

    private static ReadOnlySpan<char> Outer(ReadOnlySpan<char> chars, string tagFull, StringComparison comparison)
    {
        var startIndex = chars.IndexOf(tagFull, comparison);
        var endIndex = chars.LastIndexOf(tagFull.Insert(1, "/") + ">", comparison);
        if (endIndex > -1) endIndex += tagFull.Length + 2;
        return startIndex == -1 || endIndex == -1 ? string.Empty : chars[startIndex..endIndex];
    }

    private static ReadOnlySpan<char> Inner(ReadOnlySpan<char> chars, string tagFull, StringComparison comparison)
    {
        var startIndex = chars.IndexOf(tagFull, comparison);
        var endIndex = chars.LastIndexOf(tagFull.Insert(1, "/") + ">", comparison);
        if (startIndex > -1) startIndex = chars.ToString().IndexOf(">", startIndex + 1) + 1;
        return startIndex == -1 || endIndex == -1 ? string.Empty : chars[startIndex..endIndex];
    }
}