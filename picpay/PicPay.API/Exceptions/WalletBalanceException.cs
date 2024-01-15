namespace PicPay.API.Exceptions;

public class WalletBalanceException : Exception
{
    public WalletBalanceException()
    {
    }

    public WalletBalanceException(string message) : base(message)
    {
    }

    public WalletBalanceException(string message, Exception inner) : base(message)
    {
    }
}