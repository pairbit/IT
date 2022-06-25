using System;

namespace IT.Pdf;

public interface IPdfConverter
{
    Byte[] Convert(String content);
}