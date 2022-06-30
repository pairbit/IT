using IT.Text;

namespace IT.Tests.Text;

public class TagFinderTest
{
    private static ITagFinder _tagFinder = new TagFinder();


    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void FirstCloseTest()
    {
        //Assert.That(ML.IndexOf("</p>"), Is.EqualTo(_tagFinder.FirstClose(ML, "p")));
        //Assert.That(MLNS.IndexOf("</ns:p>"), Is.EqualTo(_tagFinder.FirstClose(MLNS, "p")));
    }

    [Test]
    public void LastOpenTest()
    {
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
    }
}