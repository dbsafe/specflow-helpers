using System;

namespace Demo.CalcEng.Domain
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
