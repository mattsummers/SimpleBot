using NPoco;
using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.Factories;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBotWeb.Models.DataObjects
{
    [TableName("users")]
    [PrimaryKey("MemberId", AutoIncrement = true)]
    [ExplicitColumns]
    public class User
    {
        private string _salt;
        private string _password;
        private string _email;
        private string _name;
        private IList<UserRole> _userRoles;

        [Column]
        public int MemberId { get; set; }

        [Column]
        public string Email
        {
            get { return (_email ?? "").Trim(); }
            set { _email = (value ?? "").Trim(); }
        }

        [Column]
        public string Password
        {
            get { return _password ?? ""; }
            set { _password = value; }
        }

        [Column]
        public string Salt
        {
            get { return _salt ?? ""; }
            set { _salt = value; }
        }

        [Column]
        public string Name
        {
            get { return _name ?? ""; }
            set { _name = value; }
        }

        public IList<UserRole> UserRoles
        {
            get
            {
                if (_userRoles == null)
                {
                    using (var dc = DatacontextFactory.GetDatabase())
                    {
                        var uh = new UserHelper(dc);
                        _userRoles = uh.GetUserRolesByMemberId(MemberId);
                    }
                }
                return _userRoles;
            }
            set { _userRoles = value; }
        }

        public User()
        {

        }

        public bool IsInRole(SimpleBotWeb.Models.Values.UserRole role)
        {
            return _userRoles.Any(x => x.Role == role);
        }
    }
}
