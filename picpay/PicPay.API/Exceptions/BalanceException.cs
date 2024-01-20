namespace PicPay.API.Exceptions;

public class BalanceException : Exception
{
    public BalanceException()
    {
    }

    public BalanceException(string message) : base(message)
    {
    }

    public BalanceException(string message, Exception inner) : base(message)
    {
    }
}