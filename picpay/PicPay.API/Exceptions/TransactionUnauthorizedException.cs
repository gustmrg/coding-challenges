namespace PicPay.API.Exceptions;

public class TransactionUnauthorizedException : Exception
{
    public TransactionUnauthorizedException()
    {
    }

    public TransactionUnauthorizedException(string message) : base(message)
    {
    }

    public TransactionUnauthorizedException(string message, Exception inner) : base(message)
    {
    }
}