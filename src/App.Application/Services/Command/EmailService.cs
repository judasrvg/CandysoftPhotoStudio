using App.Application.Abstractions.Services;
using App.Application.DTOs;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailRequestDto emailRequest, string[] imagePaths = null)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Ink Vibes Tattoo Studio", _configuration["Smtp:Username"]));
                message.To.Add(MailboxAddress.Parse(emailRequest.To));
                message.Subject = emailRequest.Subject;

                var builder = new BodyBuilder { HtmlBody = emailRequest.Body };

                // Adjuntar imágenes si las hay
                if (imagePaths != null)
                {
                    using (var httpClient = new HttpClient())
                    {
                        foreach (var imagePath in imagePaths)
                        {
                            // Verificar si la ruta es una URL
                            if (Uri.IsWellFormedUriString(imagePath, UriKind.Absolute))
                            {
                                var response = await httpClient.GetAsync(imagePath);
                                if (response.IsSuccessStatusCode)
                                {
                                    var stream = await response.Content.ReadAsStreamAsync();
                                    var fileName = Path.GetFileName(imagePath);
                                    builder.Attachments.Add(fileName, stream);
                                }
                            }
                            else if (File.Exists(imagePath))
                            {
                                builder.Attachments.Add(imagePath);
                            }
                        }
                    }
                }

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al enviar el correo: {ex.Message}");
            }
        }
    }
}
