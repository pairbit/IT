using System.Buffers;

namespace IT.Encoding.Tests;

public class WebEncoderTest
{
    private ITextEncoder _textEncoder;

    [SetUp]
    public void Setup()
    {
        _textEncoder = new IT.Encoding.Web.WebEncoder(System.Text.Encodings.Web.HtmlEncoder.Default);
    }

    private static Byte[] GetBytes(String text)=> System.Text.Encoding.UTF8.GetBytes(text);

    [Test]
    public void ByteTest()
    {
        Assert.That(_textEncoder.Encode(GetBytes("y")), Is.EqualTo(GetBytes("y")));

        Assert.That(_textEncoder.Encode(GetBytes("я")), Is.EqualTo(GetBytes("&#x44F;")));

        Assert.That(_textEncoder.Encode(GetBytes("&nbsp;")), Is.EqualTo(GetBytes("&amp;nbsp;")));

        Assert.That(_textEncoder.Encode(GetBytes("<p>My text</p>")), Is.EqualTo(GetBytes("&lt;p&gt;My text&lt;/p&gt;")));

        Assert.That(_textEncoder.Encode(GetBytes("�")), Is.EqualTo(GetBytes("&#xFFFD;")));

        Assert.That(_textEncoder.Encode(GetBytes("􀀀")), Is.EqualTo(GetBytes("&#x100000;")));

        Assert.That(_textEncoder.Encode(GetBytes("􏿿")), Is.EqualTo(GetBytes("&#x10FFFF;")));

        var maxDataLength = _textEncoder.MaxDataLength;

        var maxEncodedLength = _textEncoder.GetMaxEncodedLength(maxDataLength);

        var maxUnicode = (maxDataLength / 8) - 90;

        var maxData = new String('�', maxUnicode);

        var bytes = GetBytes(maxData);

        var byteslen = bytes.Length;

        Assert.That(byteslen / 3, Is.EqualTo(maxUnicode));

        var encoded = _textEncoder.Encode(bytes);

        Assert.That(encoded.Length, Is.LessThanOrEqualTo(maxEncodedLength));

        maxData = new String('x', maxDataLength);

        bytes = GetBytes(maxData);

        byteslen = bytes.Length;

        encoded = _textEncoder.Encode(bytes);

        Assert.That(encoded.Length, Is.EqualTo(byteslen));

        Assert.That(encoded.Length, Is.EqualTo(maxEncodedLength));
    }

    [Test]
    public void TextTest()
    {
        Assert.That(_textEncoder.EncodeToText("я"), Is.EqualTo("&#x44F;"));

        Assert.That(_textEncoder.EncodeToText("&nbsp;"), Is.EqualTo("&amp;nbsp;"));

        Assert.That(_textEncoder.EncodeToText("<p>My text</p>"), Is.EqualTo("&lt;p&gt;My text&lt;/p&gt;"));

        Assert.That(_textEncoder.EncodeToText("�"), Is.EqualTo("&#xFFFD;"));

        Assert.That(_textEncoder.EncodeToText("􀀀"), Is.EqualTo("&#x100000;"));

        Assert.That(_textEncoder.EncodeToText("􏿿"), Is.EqualTo("&#x10FFFF;"));

        var maxDataLength = _textEncoder.MaxDataLength;

        var maxEncodedLength = _textEncoder.GetMaxEncodedLength(maxDataLength);

        var maxData = new String('x', maxDataLength);

        var encoded = _textEncoder.EncodeToText(maxData);

        Assert.That(encoded.Length, Is.EqualTo(maxEncodedLength));
        
        var maxUnicode = (maxDataLength / 8) - 90;
        
        maxData = new String('�', maxUnicode);

        encoded = _textEncoder.EncodeToText(maxData);

        Assert.That(encoded.Length, Is.LessThanOrEqualTo(maxEncodedLength));
    }

    [Test]
    public void HtmlTest()
    {
        var htmlEncoder = System.Text.Encodings.Web.HtmlEncoder.Default;

        Assert.That(htmlEncoder.Encode("я"), Is.EqualTo("&#x44F;"));

        Assert.That(htmlEncoder.Encode("&nbsp;"), Is.EqualTo("&amp;nbsp;"));

        Assert.That(htmlEncoder.Encode("<p>My text</p>"), Is.EqualTo("&lt;p&gt;My text&lt;/p&gt;"));

        Assert.That(htmlEncoder.Encode("�"), Is.EqualTo("&#xFFFD;"));

        Assert.That(htmlEncoder.Encode("􀀀"), Is.EqualTo("&#x100000;"));

        Assert.That(htmlEncoder.Encode("􏿿"), Is.EqualTo("&#x10FFFF;"));

        // "&#xFFFF;" is worst case for single char ("&#x10FFFF;" [10 chars] worst case for arbitrary scalar value)
        var maxEn = htmlEncoder.MaxOutputCharactersPerInputCharacter;

        var max = (1024 * 1024 * 1024) - 33;

        var maxUnicode = (max / maxEn) - 90;
        
        var str = new String('�', maxUnicode);

        var strlen = str.Length;

        var encoded = htmlEncoder.Encode(str);

        var encodedLength = encoded.Length;

        if (encodedLength <= 1_073_741_064)
        {

        }
    }
}