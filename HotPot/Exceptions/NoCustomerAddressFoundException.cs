namespace HotPot.Exceptions
{
    public class NoCustomerAddressFoundException : ApplicationException
    {
        public NoCustomerAddressFoundException()
        {

        }

        public NoCustomerAddressFoundException(string message) : base(message)
        {

        }
    }
}