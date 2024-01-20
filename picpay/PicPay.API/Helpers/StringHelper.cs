using System.Text.RegularExpressions;

namespace PicPay.API.Helpers;

public static class StringHelper
{
    public static string RemoveSpecialCharactersAndLetters(string str)
    {
        return Regex.Replace(str, "[^0-9]+", "", RegexOptions.Compiled);
    }
}