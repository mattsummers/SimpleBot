using Microsoft.AspNetCore.Mvc;
using SimpleBotWeb.Models.DataHelpers;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Factories;
using SimpleBotWeb.Models.Helpers;
using System.Net.Mail;

namespace SimpleBotWeb.Models.Views.Account
{
    public class AccountForgotPasswordViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string Email { get; set; }

        public AccountForgotPasswordViewModel()
        {
            Success = true;
            Message = Error = Email = "";
        }

        public void SendEmail(IUrlHelper url, string email)
        {
            Success = false;
            Email = email;
            User user = null;

            if (!string.IsNullOrWhiteSpace(email))
                using (var dc = DatacontextFactory.GetDatabase())
                {
                    var uh = new UserHelper(dc);
                    user = uh.GetUserByEmail(email);
                }

            Success = user != null;

            if (!Success)
            {
                if (user == null)
                {
                    Message = "This email could not be located in the system. Can you do anything right? ";
                }
            }
            else
            {
                var passwordResetLink = url.PasswordResetLink(user.MemberId, user.Salt, true);
                var subject = "Password reset";
                var body = string.Format("Click the link below to reset your password:\r\n{0}", passwordResetLink);

                var mailer = MailFactory.GetSmtpMailer();
                var mail = new MailMessage();
                mail.From = new MailAddress(MailFactory.DefaultFromAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.To.Add(user.Email);

                mailer.Send(mail);

                Message = "Email sent with a link to reset your stupid password.";
            }
        }
    }
}