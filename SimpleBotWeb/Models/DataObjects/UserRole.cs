using NPoco;

namespace SimpleBotWeb.Models.DataObjects
{
    [TableName("userroles")]
    [PrimaryKey("RoleId", AutoIncrement = true)]
    [ExplicitColumns]
    public class UserRole
    {
        [Column]
        public int RoleId { get; set; }

        [Column]
        public int MemberId { get; set; }

        [Column]
        public Values.UserRole Role { get; set; }
    }
}
