namespace PicPay.API.Exceptions;

public class TransactionException : Exception
{
    public TransactionException()
    {
    }

    public TransactionException(string message) : base(message)
    {
    }
}