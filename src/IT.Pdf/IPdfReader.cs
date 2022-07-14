using System;
using System.IO;

namespace IT.Pdf;

public interface IPdfReader
{
    Int32 GetCountPages(Stream pdf);

    void ReadPage(Stream pdf, Int32 number, Stream page);

    Byte[] ReadPage(Stream pdf, Int32 number);
}