﻿namespace HotPot.Exceptions
{
    public class SpecialityNotFoundException : ApplicationException
    {
        public SpecialityNotFoundException()
        {

        }

        public SpecialityNotFoundException(string message) : base(message)
        {

        }
    }
}