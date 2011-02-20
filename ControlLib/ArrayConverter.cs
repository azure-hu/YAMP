using System;
using System.Collections.Generic;
using System.Text;

namespace libIsh
{
    public static class ArrayConv
    {
        /// <summary>
        /// Converts a string[] to a single string.
        /// </summary>
        /// <param name="array">Input string</param>
        /// <param name="separator">String to separate items</param>
        /// <returns>Converted string.</returns>
        public static string StringArrayToString(string[] array, string separator)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                if (array.Length > 1)
                {
                    builder.Append(separator);
                }
            }
            return builder.ToString();
        }
    }
}
