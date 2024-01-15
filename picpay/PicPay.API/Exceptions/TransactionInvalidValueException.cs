namespace PicPay.API.Exceptions;

public class TransactionInvalidValueException : Exception
{
    public TransactionInvalidValueException()
    {
    }

    public TransactionInvalidValueException(string message) : base(message)
    {
    }

    public TransactionInvalidValueException(string message, Exception inner) : base(message)
    {
    }
}