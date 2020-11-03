using NPoco;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Values;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleBotWeb.Models.DataHelpers
{
    public class TinyUrlHelper
    {
        private Database dc;
        private static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly int Base = Alphabet.Length;

        public TinyUrlHelper(Database dataContext)
        {
            dc = dataContext;
        }

        #region TinyUrls
        public void DeleteTinyUrlById(int id)
        {
            dc.Execute("delete from TinyUrls where Id=@0", id);
        }

        public void SaveTinyUrl(TinyUrl entity)
        {
            if (entity != null)
            {
                dc.Save(entity);
            }
        }

        public TinyUrl GetTinyUrlById(int id)
        {
            return dc.FirstOrDefault<TinyUrl>("select * from TinyUrls where Id=@0", id);
        }


        public TinyUrl GetTinyUrlByUrl(string url)
        {
            return dc.FirstOrDefault<TinyUrl>("select * from TinyUrls where Url=@0", url);
        }

        public TinyUrl GetTinyUrlByShortUrl(string shortUrl)
        {
            return dc.FirstOrDefault<TinyUrl>("select * from TinyUrls where ShortUrl=@0", shortUrl);
        }

        public string ShortenAllUrlsInString(string inputText)
        {
            Regex regx = new Regex("(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
            MatchCollection mactches = regx.Matches(inputText);
            foreach (Match match in mactches)
            {
                // Don't shorten already shortened links
                if (!match.Value.Contains("tinyurl") && !match.Value.ToLowerInvariant().Contains(".mp4") && match.Value.Length > 30)
                {
                    var tu = new TinyUrl();
                    tu.Url = match.Value;
                    SaveTinyUrl(tu);
                    tu.ComputeShortUrl();
                    SaveTinyUrl(tu);
                    inputText = inputText.Replace(match.Value, AppConfiguration.BaseUrl + "/tinyurl/" + tu.ShortUrl);
                }
            }
            return inputText;
        }

        public static string Encode(int i)
        {
            if (i == 0) return Alphabet[0].ToString();

            var s = string.Empty;

            while (i > 0)
            {
                s += Alphabet[i % Base];
                i = i / Base;
            }

            return string.Join(string.Empty, s.Reverse());
        }

        public static int Decode(string s)
        {
            var i = 0;

            foreach (var c in s)
            {
                i = (i * Base) + Alphabet.IndexOf(c);
            }

            return i;
        }
        #endregion
    }
}
