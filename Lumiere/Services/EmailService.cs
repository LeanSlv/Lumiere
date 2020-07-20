using Lumiere.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Lumiere.Services
{
    public class EmailService
    {
        private readonly string name;
        private readonly string login;
        private readonly string password;

        public EmailService(string emailLogin, string emailPassword)
        {
            name = "Lumiere.ru";
            login = emailLogin;
            password = emailPassword;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(name, login));
            emailMessage.To.Add(new MailboxAddress(message.ToName, message.ToEmail));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.MessageBody
            };

            /* TODO: Для отправки писем на хостинге нужен сертификат.
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, false);
                await client.AuthenticateAsync(login, password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
            */
        }

        public EmailMessage GetEmailConfirmMessage(string userName, string userEmail, string callbackUrl)
        {
            string messageBody = GenerateConfirmEmailMessageBody(userName, callbackUrl);

            return new EmailMessage 
            {
                ToName = userName,
                ToEmail = userEmail,
                Subject = $"Подтвердите ваш email aдрес для регистрации на сайте {name}",
                MessageBody = messageBody
            };
        }

        public EmailMessage GetResetPasswordMessage(string userName, string userEmail, string callbackUrl)
        {
            string messageBody = GenerateResetPasswordMessageBody(userName, callbackUrl);

            return new EmailMessage
            {
                ToName = userName,
                ToEmail = userEmail,
                Subject = $"Сброс пароля на сайте {name}",
                MessageBody = messageBody
            };
        }

        private string GenerateConfirmEmailMessageBody(string userName, string callbackUrl)
        {
            
            return
                "<div>" +
                    "<div style = 'padding: 0.5em; margin: 0 auto; width: 50%; font-size: 1.2rem; font-family: Arial, Helvetica, sans-serif;'>" +
                        $"<div style = 'margin-bottom: 0.7em;' > Здравствуйте {userName},</div>" +
                        "<div style = 'margin-bottom: 1.5em;' > Для того, чтобы продолжить регистрацию на сайте Lumiere.ru, пожалуйста, подтвердите ваш email адрес:</div>" +
                        $"<a style = 'display: flex; padding: 1.5em; background-color: #ffa600; color: white; justify-content: center; text-decoration: none; border-radius: 25px;' href = '{callbackUrl}' > Подтверить email адрес</a>" +
                     "</div>" +
                "</div>";
        }

        private string GenerateResetPasswordMessageBody(string userName, string callbackUrl)
        {
            return
                "<div>" +
                    "<div style = 'padding: 0.5em; margin: 0 auto; width: 50%; font-size: 1.2rem; font-family: Arial, Helvetica, sans-serif;'>" +
                        $"<div style = 'margin-bottom: 0.7em;' > Здравствуйте {userName},</div>" +
                        "<div style = 'margin-bottom: 1.5em;' > Для сброса пароля пройдите по ссылке, нажав на кнопку ниже.</div>" +
                        $"<a style = 'display: flex; padding: 1.5em; background-color: #ffa600; color: white; justify-content: center; text-decoration: none; border-radius: 25px;' href = '{callbackUrl}'>Сбросить пароль</a>" +
                     "</div>" +
                "</div>";
        }
    }
}
