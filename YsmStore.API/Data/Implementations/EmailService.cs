using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Utils;

namespace YsmStore.API.Data.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> _options;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async void SendRecoveryEmail(string login, string recoveryPassword)
        {
            using MimeMessage message = new();
            message.From.Add(new MailboxAddress(_options.Value.CompanyName, _options.Value.Email));
            message.To.Add(new MailboxAddress("", login));
            message.Subject = "Восстановление доступа к аккаунту";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = $"Ваш пароль восстановления {recoveryPassword}. Если вы не запрашивали пароль - игнорируйте это сообщение."
            };

            using (SmtpClient client = new())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(_options.Value.Email, _options.Value.Password);
                await client.SendAsync(message);

                _logger.LogInformation("Отправлен пароль восстановления для {login}", login);

                await client.DisconnectAsync(true);
            }
        }
    }
}
