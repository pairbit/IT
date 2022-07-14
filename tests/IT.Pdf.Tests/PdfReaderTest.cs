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

            var path = Path.Combine(dir, Path.GetFileNameWithoutExtension(file));

            for (int page = 0; page < countPages; page++)
            {
                using var pageWrite = File.OpenWrite(path + $".{page}.stream.pdf");

                _pdfReader.ReadPage(pdf, page, pageWrite);

                pageWrite.Close();

                //ToBytes
                var pageBytes = _pdfReader.ReadPage(pdf, page);

                Assert.True(pageBytes is not null);

                Assert.True(pageBytes!.Length > 0);

                File.WriteAllBytes(path + $".{page}.bytes.pdf", pageBytes);
            }
        }
    }
}