namespace IT.Pdf.Tests;

public abstract class PdfReaderTest
{
    private readonly IPdfReader _pdfReader;

    public PdfReaderTest(IPdfReader pdfReader)
    {
        _pdfReader = pdfReader;
    }

    [Test]
    public void ReadTest()
    {
        var dir = $@"C:\var\pdf";

        var files = Directory.GetFiles(dir, "*.pdf", SearchOption.TopDirectoryOnly);

        foreach (var file in files)
        {
            using var pdf = File.OpenRead(file);

            var countPages = _pdfReader.GetCountPages(pdf);

            Assert.True(countPages > 0);

            for (int page = 0; page < countPages; page++)
            {
                var pageBytes = _pdfReader.ReadPage(pdf, page);

                Assert.True(pageBytes is not null);

                Assert.True(pageBytes!.Length > 0);

                var path = Path.Combine(dir, Path.GetFileNameWithoutExtension(file));
                path += $".{page}.pdf";
                //File.WriteAllBytes(path, pageBytes);
            }
        }
    }
}