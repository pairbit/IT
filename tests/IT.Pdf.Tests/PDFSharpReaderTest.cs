namespace IT.Pdf.Tests;

public class PDFSharpReaderTest : PdfReaderTest
{
    public PDFSharpReaderTest() : base(new IT.Pdf.PDFSharp.PdfReader()) { }
}