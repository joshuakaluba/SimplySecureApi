using System.Globalization;

namespace SimplySecureApi.Common.Extensions.Strings
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title);
        }
    }
}