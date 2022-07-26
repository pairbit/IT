namespace IT.Encoding.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var htmlEncoder = System.Text.Encodings.Web.HtmlEncoder.Default;

        Assert.That(htmlEncoder.Encode("<p>My text</p>"), Is.EqualTo("&lt;p&gt;My text&lt;/p&gt;"));


    }
}