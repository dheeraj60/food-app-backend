namespace HotPot.Exceptions
{
    public class NoCustomerReviewFoundException : ApplicationException
    {
        public NoCustomerReviewFoundException() { }
        public override string Message => "No reviews found";
    }
}