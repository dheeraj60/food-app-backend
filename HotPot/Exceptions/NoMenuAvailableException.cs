namespace HotPot.Exceptions
{
    public class NoMenuAvailableException : ApplicationException
    {
        public NoMenuAvailableException()
        {

        }

        public override string Message => "No menu available at the moment to display";
    }
}