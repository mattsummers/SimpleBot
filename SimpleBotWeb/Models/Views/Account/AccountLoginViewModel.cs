using Microsoft.AspNetCore.Http;
using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.Factories;
using SimpleBotWeb.Models.Helpers;

namespace SimpleBotWeb.Models.Views.Account
{
    public class AccountLoginViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string Email { get; set; }

        public AccountLoginViewModel()
        {
            Success = true;
            Message = Error = Email = "";
        }

        public async void AttemptLogon(string email, string password, HttpContext request)
        {
            Email = email;
            using (var dc = DatacontextFactory.GetDatabase())
            {
                var uh = new UserHelper(dc);
                var userAccount = uh.GetUserByEmail(email);

                if (userAccount == null || EncryptionHelper.GenerateSCryptHash(password, userAccount.Salt) != userAccount.Password)
                {
                    Success = false;
                    Error = "Invalid account details.";
                    return;
                }

                Message = "Oh goody. You're back.";

                await AuthenticationHelper.Login(request, userAccount);
            }
        }
    }
}
