using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Services
{
    internal class SendEmailService : ISendEmailService
    {
        private readonly EmailSettings _emailSettings;

        public SendEmailService(IOptions<EmailSettings>options)
        {
            _emailSettings= options.Value;  
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage, CancellationToken ct = default)
        {
            var cleanSenderEmail = _emailSettings.SenderEmail?.Trim();
            var cleanToEmail = toEmail?.Trim();

            if (string.IsNullOrWhiteSpace(cleanSenderEmail))
            {
                throw new InvalidOperationException("SenderEmail in appsettings.json is missing or empty.");
            }

            if (string.IsNullOrWhiteSpace(cleanToEmail))
            {
                throw new ArgumentNullException(nameof(toEmail), "Target recipient email cannot be empty.");
            }

            var message = new MimeMessage();

            // Use Parse to safely construct MailboxAddress objects
            var senderName = string.IsNullOrWhiteSpace(_emailSettings.SenderName) ? "E-Commerce App" : _emailSettings.SenderName;

            message.From.Add(new MailboxAddress(senderName, cleanSenderEmail));
            message.To.Add(MailboxAddress.Parse(cleanToEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            // For Papercut local testing (Port 25 without TLS)
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.None, ct);

            if (!string.IsNullOrEmpty(_emailSettings.Username))
            {
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, ct);
            }

            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);

        }
    }
}
