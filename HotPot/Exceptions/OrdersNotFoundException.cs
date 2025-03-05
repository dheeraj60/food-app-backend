namespace HotPot.Exceptions
{
    public class OrdersNotFoundException : ApplicationException
    {
        public OrdersNotFoundException()
        {

        }

        public override string Message => "No orders available to show at the moment";
    }
}