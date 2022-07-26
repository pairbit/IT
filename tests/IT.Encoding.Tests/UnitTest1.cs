namespace IT.Encoding.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void HtmlTest()
    {
        var htmlEncoder = System.Text.Encodings.Web.HtmlEncoder.Default;

        // "&#xFFFF;" is worst case for single char ("&#x10FFFF;" [10 chars] worst case for arbitrary scalar value)
        var maxEn = htmlEncoder.MaxOutputCharactersPerInputCharacter;

        var encoded = htmlEncoder.Encode("ÿ");

        Assert.That(htmlEncoder.Encode("ÿ"), Is.EqualTo("&#x44F;"));

        Assert.That(htmlEncoder.Encode("<p>My text</p>"), Is.EqualTo("&lt;p&gt;My text&lt;/p&gt;"));


    }
}