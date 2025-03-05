namespace HotPot.Exceptions
{
    public class NoUsersAvailableException : Exception
    {
        public NoUsersAvailableException()
        {

        }

        public override string Message => "No users available at the moment to display";
    }
}