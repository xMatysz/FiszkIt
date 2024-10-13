namespace FiszkIt.Core.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string str)
        => string.IsNullOrWhiteSpace(str);
}
