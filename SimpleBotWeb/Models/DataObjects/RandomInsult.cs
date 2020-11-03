using NPoco;
using SimpleBotWeb.Models.Helpers;

namespace SimpleBotWeb.Models.DataObjects
{
    [TableName("randominsults")]
    [PrimaryKey("RandomInsultId", AutoIncrement = true)]
    [ExplicitColumns]
    public class RandomInsult
    {
        private string _insult;

        [Column]
        public int RandomInsultId { get; set; }

        [Column]
        public string Insult
        {
            get { return ValidationHelper.MaxLength(_insult, 50); }
            set { _insult = value; }
        }
    }
}
