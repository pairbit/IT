namespace IT.Pdf.Tests;

public abstract class PdfConverterTest : IDisposable
{
    private readonly IPdfConverter _pdfConverter;

    public PdfConverterTest(IPdfConverter pdfConverter)
    {
        _pdfConverter = pdfConverter;
    }

    [Test]
    public void ConvertTest()
    {
        //var html = $"������� ������ ��� ����������� � PDF ({Guid.NewGuid()})";

        var html = File.ReadAllText(@"C:\var\pdf\EMP-View.html");

        var bytes = _pdfConverter.Convert(html);

        Assert.True(bytes is not null);

        Assert.True(bytes!.Length > 0);

        File.WriteAllBytes($@"C:\var\pdf\{DateTime.Now.Ticks}.pdf", bytes);
    }

    public void Dispose()
    {
        var disposable = _pdfConverter as IDisposable;

        if (disposable is null) return;

        disposable.Dispose();

        Console.WriteLine("Dispose");
    }
}