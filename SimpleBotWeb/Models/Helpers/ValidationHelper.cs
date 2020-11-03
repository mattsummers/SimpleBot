using System;
using System.Text.RegularExpressions;

namespace SimpleBotWeb.Models.Helpers
{
    public static class ValidationHelper
    {
        public static DateTime MinimumSqlDateTime => new DateTime(1900, 1, 1);

        /// <summary>
        ///     One way translation of a string to one suitable for SEO friendly URLs
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static string GetSeoFriendlyName(string inputText)
        {
            inputText = (inputText ?? "").Trim();
            inputText = Regex.Replace(inputText, @"[^a-zA-Z0-9]", "-"); // invalid chars
            inputText = Regex.Replace(inputText, @"-+", "-").Trim(); // convert multiple dashes into one
            inputText = inputText.TrimStart('-');
            inputText = inputText.TrimEnd('-');
            return inputText;
        }

        public static bool ParseIntToBool(object value)
        {
            return ParseIntToBool(value, false);
        }

        public static bool ParseIntToBool(object value, bool defaultValue)
        {
            return ParseBool(value, defaultValue);
        }

        /// <summary>
        ///     Performs standard Boolean.TryParse conversions with the addition that '1' will be also be treated as 'true'. If the
        ///     conversion fails, false will be returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True or false. If the conversion was unsuccessful, false is returned</returns>
        public static bool ParseBool(object value)
        {
            return ParseBool(value, false);
        }

        /// <summary>
        ///     Performs standard Boolean.TryParse conversions with the addition that '1' will be also be treated as 'true'
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ParseBool(object value, bool defaultValue)
        {
            bool returnValue;

            if (value != null)
            {
                if (!bool.TryParse(value.ToString(), out returnValue))
                    if (value.ToString() == "1")
                        returnValue = true;
                    else if (value.ToString() == "0")
                        returnValue = false;
                    else
                        returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        ///     Performs standard int.TryParse conversions and handles nulls. If the converversion fails, 0 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ParseInt(object value)
        {
            return ParseInt(value, 0);
        }

        public static int ParseInt(object value, int defaultValue)
        {
            int returnValue;

            if (value != null)
            {
                if (!int.TryParse(value.ToString(), out returnValue))
                    returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        ///     Performs standard int.TryParse conversions and handles nulls. If the conversion fails, 0 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ParseLong(object value)
        {
            return ParseLong(value, 0);
        }

        public static long ParseLong(object value, long defaultValue)
        {
            long returnValue;

            if (value != null)
            {
                if (!long.TryParse(value.ToString(), out returnValue))
                    returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        public static DateTime ParseDateTime(object value)
        {
            return ParseDateTime(value, DateTime.MinValue);
        }

        public static DateTime ParseDateTime(object value, DateTime defaultValue)
        {
            DateTime returnValue;

            if (value != null)
            {
                if (!DateTime.TryParse(value.ToString(), out returnValue))
                    returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        public static decimal ParseDecimal(object value)
        {
            return ParseDecimal(value, 0);
        }

        public static decimal ParseDecimal(object value, decimal defaultValue)
        {
            decimal returnValue;

            if (value != null)
            {
                if (!decimal.TryParse(value.ToString(), out returnValue))
                    returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        public static float ParseFloat(object value)
        {
            return ParseFloat(value, 0);
        }

        public static float ParseFloat(object value, float defaultValue)
        {
            float returnValue;

            if (value != null)
            {
                if (!float.TryParse(value.ToString(), out returnValue))
                    returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        public static double ParseDouble(object value)
        {
            return ParseDouble(value, 0);
        }

        public static double ParseDouble(object value, double defaultValue)
        {
            double returnValue;

            if (value != null)
            {
                if (!double.TryParse(value.ToString(), out returnValue))
                    returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        public static short ParseShort(object value)
        {
            return ParseShort(value, 0);
        }

        public static short ParseShort(object value, short defaultValue)
        {
            short returnValue;

            if (value != null)
            {
                if (!short.TryParse(value.ToString(), out returnValue))
                    returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        public static byte ParseByte(object value)
        {
            return ParseByte(value, 0);
        }

        public static byte ParseByte(object value, byte defaultValue)
        {
            byte returnValue;

            if (value != null)
            {
                if (!byte.TryParse(value.ToString(), out returnValue))
                    returnValue = defaultValue;
            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        ///     Casts an object to a string, or returns an empty string if unable. Essentially a wrapper for ??
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ParseString(object value)
        {
            return ParseString(value, string.Empty);
        }

        /// <summary>
        ///     Casts an object to a string, or returns the default value if unable. Essentially a wrapper for ??
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ParseString(object value, string defaultValue)
        {
            string returnValue;

            if (value == null)
                returnValue = defaultValue;
            else
                return value.ToString();

            return returnValue;
        }

        public static bool IsValidDate(string tryString)
        {
            return DateTime.TryParse(tryString, out _);
        }

        public static string MaxLength(string inputString, int maxLength)
        {
            if (inputString == null)
                return "";

            if (inputString.Length <= maxLength)
                return inputString;
            return inputString.Substring(0, maxLength);
        }

        // Ensure a string begins with http:// or https://
        public static string ValidateUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            var tempval = input.ToLowerInvariant();
            if (tempval.IndexOf("http://", StringComparison.Ordinal) != 0 &&
                tempval.IndexOf("https://", StringComparison.Ordinal) != 0)
                input = "http://" + input;
            return input;
        }
    }
}