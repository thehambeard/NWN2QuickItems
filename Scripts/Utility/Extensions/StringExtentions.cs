using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NWN2QuickItems.Utility.Extensions.StringExtentsions
{
    internal static class StringExtentions
    {
        public static string ConvertCamelCaseToWords(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string spaced = Regex.Replace(input, "(?<!^)(?<![A-Z])([A-Z])", " $1");

            return char.ToUpper(spaced[0]) + spaced.Substring(1);
        }
    }
}
