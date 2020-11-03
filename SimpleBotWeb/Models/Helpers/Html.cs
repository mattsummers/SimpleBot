using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SimpleBotWeb.Models.Helpers
{
    public static class HtmlHelpers
    {
        /// <summary>a
        /// A very simple class that makes applying defaults in an HTML select list a little nicer to read
        /// </summary>
        /// <param name="html"></param>
        /// <param name="conditional"></param>
        /// <returns></returns>
        public static string IsSelected(this IHtmlHelper html, bool conditional)
        {
            return conditional ? " selected " : "";
        }

        /// <summary>
        /// A very simple class that makes applying defaults in HTML checkboxes and radio options a little nicer to read
        /// </summary>
        /// <param name="html"></param>
        /// <param name="conditional"></param>
        /// <returns></returns>
        public static string IsChecked(this IHtmlHelper html, bool conditional)
        {
            return conditional ? " checked " : "";
        }

        public static HtmlString SafeDecode(this IHtmlHelper html, string inputHtml, HtmlDecodeMethod method, bool convertCrlfToBr)
        {
            return new HtmlString(UtilityHelper.SafeDecode(inputHtml, method, convertCrlfToBr));
        }

        public static HtmlString FormatJavascriptString(this IHtmlHelper html, string javascript, bool useHtmlLineBreaks)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(javascript));
        }

        public static HtmlString FormatListItems(this IHtmlHelper html, string value)
        {
            return new HtmlString(value);
        }

        public static HtmlString GetCheckedImage(this IHtmlHelper html, bool isChecked)
        {
            if (isChecked)
            {
                return new HtmlString("<img src=\"/img/check_blue.png\" alt=\"ck\" />");
            }
            return new HtmlString("&nbsp;");
        }

        #region Notification boxes

        public static HtmlString NotificationMessage(this IHtmlHelper html, string message)
        {
            return MessageBox(html, "bg-primary text-light", message);
        }

        public static HtmlString NotificationMessage(this IHtmlHelper html, string[] messages)
        {
            return MessageBox(html, "bg-primary text-light", messages);
        }

        public static HtmlString ErrorMessage(this IHtmlHelper html, string message)
        {
            return MessageBox(html, "bg-danger text-light", message);
        }

        public static HtmlString ErrorMessage(this IHtmlHelper html, string[] messages)
        {
            return MessageBox(html, "bg-danger text-light", messages);
        }

        public static HtmlString MessageBox(this IHtmlHelper html, string cssClass, string message)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                return new HtmlString("<div class='" + WebUtility.HtmlDecode(cssClass) + "' style='margin: 10px 0; padding: 15px; border-radius: 5px;'><strong>" + WebUtility.HtmlDecode(message) + "</strong></div>");
            }
            return new HtmlString("");
        }

        public static HtmlString WriteMessages(this IHtmlHelper html, ITempDataDictionary data)
        {
            string result = "";
            if (data["message"] != null)
            {
                result += NotificationMessage(html, data["message"].ToString()).ToString();
            }
            if (data["error"] != null)
            {
                result += ErrorMessage(html, data["error"].ToString()).ToString();
            }
            return new HtmlString(result);
        }

        private static HtmlString MessageBox(this IHtmlHelper html, string cssClass, string[] messages)
        {
            if (messages.Any())
            {
                var sb = new StringBuilder(messages.Count() + 2);
                sb.AppendFormat("<div class='{0}' style='margin: 10px 0; padding: 15px; border-radius: 5px;'>\r\n<ul>\r\n", WebUtility.HtmlDecode(cssClass));
                foreach (var s in messages)
                {
                    sb.Append("<li><strong>" + WebUtility.HtmlDecode(s) + "</strong></li>\r\n");
                }
                sb.Append("</ul>\r\n</div>\r\n");
                return new HtmlString(sb.ToString());
            }
            return new HtmlString("");
        }
        #endregion
    }
}
