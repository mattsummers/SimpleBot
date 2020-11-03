using NPoco;
using SimpleBotWeb.Models.Helpers;

namespace SimpleBotWeb.Models.DataObjects
{
    [TableName("entries")]
    [PrimaryKey("EntryId", AutoIncrement = true)]
    [ExplicitColumns]
    public class Entry
    {
        private string _phrase;
        private string _response;

        [Column]
        public int EntryId { get; set; }

        [Column]
        public string Phrase
        {
            get => ValidationHelper.MaxLength(_phrase, 50).ToLowerInvariant();
            set => _phrase = value;
        }

        [Column]
        public string Response
        {
            get => ValidationHelper.MaxLength(_response, 60000);
            set => _response = value;
        }

        [Column]
        public bool StartsWith { get; set; }

        [Column]
        public bool Hidden { get; set; }

        [Column]
        public int MemberId { get; set; }

        [ResultColumn]
        public string Owner { get; set; }

        [ResultColumn]
        public string LastEdited { get; set; }

        [Column]
        public bool AllowRepeat { get; set; }
    }
}
