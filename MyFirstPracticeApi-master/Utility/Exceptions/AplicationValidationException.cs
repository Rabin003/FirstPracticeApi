using System;
namespace Utility.Exceptions
{
    public class AplicationValidationException : Exception
    {
        public AplicationValidationException(string message) : base(message)
        {
        }
    }
}