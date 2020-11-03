using NPoco;
using SimpleBotWeb.Models.DataHelpers;

namespace SimpleBotWeb.Models.DataObjects
{
    [TableName("tinyurls")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class TinyUrl
    {
        private string _url;
        private string _shortUrl;

        [Column]
        public int Id { get; set; }

        [Column]
        public string Url
        {
            get => (_url ?? "").Trim();
            set => _url = (value ?? "").Trim();
        }

        [Column]
        public string ShortUrl
        {
            get => _shortUrl ?? "";
            set => _shortUrl = value;
        }

        public void ComputeShortUrl()
        {
            if (Id > 0)
            {
                ShortUrl = TinyUrlHelper.Encode(Id);
            }
        }
    }
}
