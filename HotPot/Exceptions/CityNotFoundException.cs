namespace HotPot.Exceptions
{
    public class CityNotFoundException : ApplicationException
    {
        public CityNotFoundException()
        {

        }

        public override string Message => "No city found";
    }
}