using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
