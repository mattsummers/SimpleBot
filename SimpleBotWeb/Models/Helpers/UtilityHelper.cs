using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace SimpleBotWeb.Models.Helpers
{
    public enum HtmlDecodeMethod
    {
        SafeHtml = 0,
        NoHtml = 1,
        RemoveTags = 2
    }

    public static class UtilityHelper
    {
        /// <summary>
        ///     Beginning at the targetlength, look for the next whitespace and trim the string at that location.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetLength"></param>
        /// <returns></returns>
        public static string TrimAtNextWhitespace(string body, int targetLength)
        {
            if (body.Length > targetLength)
            {
                var iEndPos = body.IndexOf(" ", targetLength, StringComparison.Ordinal);

                if (iEndPos > 0)
                    return body.Substring(0, iEndPos);
                return body.Substring(0, targetLength);
            }
            return body;
        }

        public static bool IsValidRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static string SafeDecode(string inputHtml, HtmlDecodeMethod decodeMethod, bool convertCrlfToBr)
        {
            string resultHtml;
            // Converts HTML to "safe" HTML
            // If StripHtml is true, then HTML is completely removed, rather than any attempt at preservation
            // If ConvertCrlfToBr is true, then <br>, newline, and <P> tags are converted to <br>

            if (string.IsNullOrEmpty(inputHtml))
                return "";

            if (decodeMethod == HtmlDecodeMethod.SafeHtml)
                resultHtml = FilterHtml(inputHtml);
            else if (decodeMethod == HtmlDecodeMethod.RemoveTags)
                resultHtml = StripHtml(inputHtml);
            else if (decodeMethod == HtmlDecodeMethod.NoHtml)
                resultHtml = WebUtility.HtmlEncode(inputHtml);
            else
                resultHtml = StripHtml(inputHtml);

            resultHtml = AutoHyperlink(resultHtml);

            if (convertCrlfToBr)
                resultHtml = resultHtml.Replace(Environment.NewLine, "<br>");

            return resultHtml;
        }

        public static string StripHtml(string inputHtml)
        {
            var htmlpattern = @"<(.|\n)*?>";
            return HttpUtility.HtmlDecode(Regex.Replace(inputHtml, htmlpattern, string.Empty));
        }

        public static string FilterHtml(string inputHtml)
        {
            inputHtml = ReplaceEx(inputHtml, "<link", "&lt;link");
            inputHtml = ReplaceEx(inputHtml, "<iframe", "&lt;iframe");
            inputHtml = ReplaceEx(inputHtml, "<applet", "&lt;applet");
            inputHtml = ReplaceEx(inputHtml, "<body", "&lt;body");
            inputHtml = ReplaceEx(inputHtml, "<embed", "&lt;embed");
            inputHtml = ReplaceEx(inputHtml, "<form", "&lt;form");
            inputHtml = ReplaceEx(inputHtml, "<frame", "&lt;frame");
            inputHtml = ReplaceEx(inputHtml, "<script", "&lt;script");
            inputHtml = ReplaceEx(inputHtml, "<object", "&lt;object");
            inputHtml = ReplaceEx(inputHtml, "<meta", "&lt;meta");
            inputHtml = ReplaceEx(inputHtml, "<style", "&lt;style");
            inputHtml = ReplaceEx(inputHtml, "<noscript", "&lt;noscript");
            inputHtml = ReplaceEx(inputHtml, "<style", "&lt;style");
            inputHtml = ReplaceEx(inputHtml, "<style", "&lt;style");
            inputHtml = inputHtml.Replace("&#40", "&amp;#40");
            inputHtml = inputHtml.Replace("&#41", "&amp;#41");
            inputHtml = inputHtml.Replace("&#0", "&amp;#0");
            inputHtml = inputHtml.Replace("&#x28", "&amp;#x28");
            inputHtml = inputHtml.Replace("&#x29", "&amp;#x29");
            inputHtml = inputHtml.Replace("(", "<b></b>(");
            inputHtml = inputHtml.Replace(")", "<b></b>)");
            return inputHtml;
        }

        public static string AutoHyperlink(string inputHtml)
        {
            // First replace links under 60 characters long
            inputHtml = Regex.Replace(inputHtml, @"(\s|^)(http[s]?:)[/|\\]{2}([^\s\x3C]{0,60})(\s|\x3C|$)",
                @"$1<a href=""$2//$3"" target=""_blank"" title=""$2//$3"">$2//$3</a>$4");

            // Now replace links > 60 characters long with a trimmed-down hyperlink
            inputHtml = Regex.Replace(inputHtml, @"(\s|^)(http[s]?:)[/|\\]{2}(\S{0,60})([^\s\x3C]*)",
                @"$1<a href=""$2//$3$4"" target=""_blank"" title=""$2//$3$4"">$2//$3...</a>");

            return inputHtml;
        }

        public static string ReplaceEx(string original, string pattern, string replacement)
        {
            // A case insensitive string replacement without regex for speed
            // From http://www.codeproject.com/KB/string/fastestcscaseinsstringrep.aspx
            int count, position0, position1;
            count = position0 = 0;
            var upperString = original.ToUpper();
            var upperPattern = pattern.ToUpper();
            var inc = original.Length / pattern.Length *
                      (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0, StringComparison.Ordinal)) != -1)
            {
                for (var i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (var i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (var i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }
    }
}