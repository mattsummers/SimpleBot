using NPoco;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UserRole = SimpleBotWeb.Models.Values.UserRole;

namespace SimpleBotWeb.Models.DataHelpers
{
    public class UserHelper
    {
        private Database dc;

        public UserHelper(Database dataContext)
        {
            dc = dataContext;
        }

        #region Users
        public Page<User> GetUsers(UserRole? role = null, string searchField = "", bool? showActive = null, int currentPage = 1, int pageSize = 20)
        {
            var qb = new Sql();
            var conditionalClause = "Where";

            if (role.HasValue)
            {
                qb.Append("Select Users.* from Users left outer join UserRoles on Users.MemberId=UserRoles.MemberId ");
            }
            else
            {
                qb.Append("Select * from Users ");
            }


            if (!String.IsNullOrWhiteSpace(searchField))
            {
                qb.Append(conditionalClause + " (");
                qb.Append("(Email like @0) OR ", "%" + searchField + "%");
                qb.Append("(Name like @0) OR ", "%" + searchField + "%");
                qb.Append(")");
                conditionalClause = "And";
            }

            if (showActive.HasValue)
            {
                qb.Append(conditionalClause + " Approved = @0 ", showActive.Value);
                conditionalClause = "And";
            }

            if (role.HasValue)
            {
                qb.Append(conditionalClause + " Role=@0 ", (int)role);
                conditionalClause = "And";
            }

            qb.Append("Order By Email asc ");

            return dc.Page<User>(currentPage, pageSize, qb);
        }

        public User GetUserById(int id)
        {
            return dc.FirstOrDefault<User>("Select * from Users where MemberId=@0", id);
        }

        public User GetUserByEmail(string email)
        {
            return dc.FirstOrDefault<User>("Select * from Users where Email=@0", email);
        }

        public void SaveUser(User entity)
        {
            dc.Save(entity);
        }

        public void DeleteUser(User entity)
        {
            dc.Delete<User>(entity);
        }

        public void DeleteUserById(int id)
        {
            DeleteUser(GetUserById(id));
        }

        /// <summary>
        /// Salts and hashes a user's password and records it in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="plaintextPassword"></param>
        /// <returns>The hashed password + salt</returns>
        public string SetPassword(int id, string plaintextPassword)
        {
            if (String.IsNullOrWhiteSpace(plaintextPassword))
            {
                throw new ArgumentException("A password was not supplied.");
            }

            var user = GetUserById(id);

            if (user == null)
            {
                throw new ArgumentException(String.Format("A member with the id of {0} was not found.", id));
            }

            var newSalt = EncryptionHelper.GenerateRandomNoise(20);
            var hashedPassword = EncryptionHelper.GenerateSCryptHash(plaintextPassword, newSalt);

            user.Salt = newSalt;
            user.Password = hashedPassword;
            SaveUser(user);
            return hashedPassword;
        }
        #endregion

        #region UserRoles

        public Page<SimpleBotWeb.Models.DataObjects.UserRole> GetUserRoles(int currentPage = 1, int pageSize = 20)
        {
            return dc.Page<SimpleBotWeb.Models.DataObjects.UserRole>(currentPage, pageSize, "Select * from UserRoles order by UserRoleId");
        }

        public SimpleBotWeb.Models.DataObjects.UserRole GetUserRoleById(int id)
        {
            return dc.FirstOrDefault<SimpleBotWeb.Models.DataObjects.UserRole>("Select * from UserRoles where UserRoleId=@0", id);
        }

        public SimpleBotWeb.Models.DataObjects.UserRole GetUserRoleByMemberId(int memberId, UserRole role)
        {
            return dc.FirstOrDefault<SimpleBotWeb.Models.DataObjects.UserRole>("Select * from UserRoles where MemberId=@0 and Role=@1", memberId, (int)role);
        }

        public IList<SimpleBotWeb.Models.DataObjects.UserRole> GetUserRolesByMemberId(int id)
        {
            return dc.Query<SimpleBotWeb.Models.DataObjects.UserRole>("Select * from UserRoles where MemberId=@0", id).ToList();
        }

        public void SaveUserRole(SimpleBotWeb.Models.DataObjects.UserRole entity)
        {
            if (entity != null)
            {
                var existing = GetUserRoleByMemberId(entity.MemberId, entity.Role);
                if (existing == null)
                {
                    dc.Save(entity);
                }
            }
        }

        public void DeleteUserRole(SimpleBotWeb.Models.DataObjects.UserRole entity)
        {
            dc.Delete<SimpleBotWeb.Models.DataObjects.UserRole>(entity);
        }

        public void DeleteUserRoleById(int id)
        {
            DeleteUserRole(GetUserRoleById(id));
        }
        #endregion
    }
}
