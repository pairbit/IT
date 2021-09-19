using System;

namespace IT.Exceptions
{
    public class ErrorException : Exception
    {
        public Error Error { get; set; }
    }
}