﻿using System.Runtime.Serialization;

namespace HotPot.Exceptions
{
    [Serializable]
    internal class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() { }

        public InvalidPasswordException(string message) : base(message) { }

        public InvalidPasswordException(string message, Exception innerException) : base(message, innerException) { }
    }
}