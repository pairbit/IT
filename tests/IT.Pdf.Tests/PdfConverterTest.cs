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
        var html = $"Простые данные для отображения в PDF ({Guid.NewGuid()})";

        //var html = File.ReadAllText(@"D:\var\pdf\View2.txt");

        var bytes = _pdfConverter.Convert(html);

        Assert.True(bytes is not null);

        Assert.True(bytes!.Length > 0);

        //File.WriteAllBytes($@"D:\var\pdf\{DateTime.Now.Ticks}.pdf", bytes);
    }

    public void Dispose()
    {
        var disposable = _pdfConverter as IDisposable;

        if (disposable is null) return;

        disposable.Dispose();

        Console.WriteLine("Dispose");
    }
}