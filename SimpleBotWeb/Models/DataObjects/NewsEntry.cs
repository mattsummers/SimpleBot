using NPoco;
using System;

namespace SimpleBotWeb.Models.DataObjects
{
    [TableName("newsentries")]
    [PrimaryKey("NewsId", AutoIncrement = true)]
    [ExplicitColumns]
    public class NewsEntry
    {
        private string _content;

        [Column]
        public int NewsId { get; set; }

        [Column]
        public DateTime DatePostedUtc { get; set; }

        [Column]
        public string Content
        {
            get => _content ?? "";
            set => _content = value;
        }

        public NewsEntry()
        {
            DatePostedUtc = DateTime.UtcNow;
        }
    }
}
