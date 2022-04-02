using System.Text.RegularExpressions;

namespace UIM.Core.Helpers;

public static class StringExtensions
{
    public static string ToSnakeCase(this string o)
    {
        return Regex.Replace(o, @"(\w)([A-Z])", "$1_$2").ToLower();
    }
}
