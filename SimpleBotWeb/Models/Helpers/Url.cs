using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;

namespace SimpleBotWeb.Models.Helpers
{
    public static class UrlHelpers
    {
        public static string PasswordResetLink(this IUrlHelper url, int memberId, string resetCode, bool returnFullyQualifiedName)
        {
            string endpoint = url.Action("ResetPassword", "Account", new RouteValueDictionary { { "memberid", memberId }, { "key", resetCode } }, returnFullyQualifiedName ? "http" : null);
            return endpoint;
        }

        public static string GetBaseUrl(this IUrlHelper url)
        {
            return new Uri(url.ActionContext.HttpContext.Request.GetDisplayUrl()).GetLeftPart(UriPartial.Authority) + url.Content("~/");
        }

        public static string ToFriendlyUrl(this IUrlHelper url, string encodeText)
        {
            return ValidationHelper.GetSeoFriendlyName(encodeText);
        }
    }
}
