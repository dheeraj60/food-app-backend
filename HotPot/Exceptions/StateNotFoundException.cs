﻿namespace HotPot.Exceptions
{
    public class StateNotFoundException : ApplicationException
    {
        public StateNotFoundException()
        {

        }

        public StateNotFoundException(string message) : base(message)
        {

        }
    }
}