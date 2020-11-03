using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;
using SimpleBotWeb.Models.Helpers;
using System;

namespace SimpleBotWeb.Models.Views.Account
{
    public class AccountResetPasswordViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }

        public User UserInfo { get; set; }
        public int MemberId { get; set; }
        public bool KeyIsValid { get; set; }
        public string Key { get; set; }

        public AccountResetPasswordViewModel(int memberId, string key)
        {
            Message = String.Empty;
            MemberId = memberId;
            Key = key;
            Success = true;

            using (var dc = DatacontextFactory.GetDatabase())
            {
                var uh = new UserHelper(dc);
                var user = uh.GetUserById(memberId);

                KeyIsValid = (user != null && String.Compare(user.Salt, key, StringComparison.InvariantCulture) == 0);

                if (!KeyIsValid)
                {
                    Success = false;
                    Error = "This link to reset your password has expired. Please try again.\r\n";
                    return;
                }
            }
        }

        public void ResetPassword(string newPassword)
        {
            // Valid state?
            if (!Success)
            {
                return;
            }

            if (!PasswordHelper.IsValid(newPassword))
            {
                Success = false;
                Message += "Please enter a better password. This one is garbage.\r\n";
                return;
            }

            using (var dc = DatacontextFactory.GetDatabase())
            {
                var uh = new UserHelper(dc);
                uh.SetPassword(MemberId, newPassword);
                UserInfo = uh.GetUserById(MemberId);
                Message = "Your password has been changed. Try not to screw it up this time.";
            }
        }

    }
}
