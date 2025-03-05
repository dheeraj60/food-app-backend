namespace HotPot.Exceptions
{
    public class RestaurantNotFoundException : ApplicationException
    {
        public RestaurantNotFoundException()
        {

        }

        public override string Message => "No Restaurant found";
    }
}