using App.Application.Abstractions;
using App.Application.Abstractions.Services;
using App.Application.DTOs;
using App.Domain.Entities;
using App.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class AppointmentNotificationService : IHostedService, IDisposable
    {
        private readonly ILogger<AppointmentNotificationService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public AppointmentNotificationService(
            ILogger<AppointmentNotificationService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Appointment Notification Service started.");
            _timer = new Timer(CheckAppointments, null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
            return Task.CompletedTask;
        }

        private async void CheckAppointments(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var emailTemplateService = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();
                var reservationQueryService = scope.ServiceProvider.GetRequiredService<IReservationQueryService>();
                var configValueQueryService = scope.ServiceProvider.GetRequiredService<IConfigValueQueryService>();
                var reservationCommandService = scope.ServiceProvider.GetRequiredService<IReservationCommandService>();

                var upcomingReservations = await reservationQueryService.GetUpcomingReservationsAsync(TimeSpan.FromMinutes(60));
                if (upcomingReservations?.Any() ?? false)
                {
                    var providersEmail = await configValueQueryService.GetConfigValuesByTypeAsync(CacheValueType.EmailAddress);
                    var providersphoneFacebook = await configValueQueryService.GetConfigValuesByTypeAsync(CacheValueType.PhoneFacebook);
                    var providersTiktokInstagram = await configValueQueryService.GetConfigValuesByTypeAsync(CacheValueType.TikTokInstagram);
                    var providerLocation = await configValueQueryService.GetConfigValuesByTypeAsync(CacheValueType.StudioLocation);
                    var emailAdress = providersEmail?.FirstOrDefault();
                    var phoneFacebook = providersphoneFacebook?.FirstOrDefault();
                    var tiktokInstagram = providersTiktokInstagram?.FirstOrDefault();
                    var location = providerLocation?.FirstOrDefault();
                    double lat = 27.99819074; // Valor por defecto
                    double lng = -82.49170303; // Valor por defecto
                    if (!string.IsNullOrWhiteSpace(location?.Value ?? ""))
                    {
                        var coords = location.Value.Split(',');
                        if (coords.Length == 2 &&
                            double.TryParse(coords[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out lat) &&
                            double.TryParse(coords[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out lng))
                        {
                            //doSomething
                        }
                    }
                    foreach (var reservation in upcomingReservations)
                    {
                        var model = new ReminderEmailModel
                        {
                            ClientName = reservation.ClientName,
                            ReservationDate = reservation.ReservationDateStart,
                            Description = reservation.Details,
                            ProviderName = "Candysoft Studio", // Agrega el nombre real del proveedor aquí
                            Address = emailAdress.ValueDescription,
                            PhoneNumber = phoneFacebook.Value,
                            Facebook = phoneFacebook.ValueDescription,
                            Instagram = tiktokInstagram.ValueDescription,
                            TikTok = tiktokInstagram.Value,
                            Latitude = lat,
                            Longitude = lng
                        };

                        var clientEmailBody = reservation.Lang == "en" ? await emailTemplateService.GetTemplateAsync("ClientReminderEmail", model) : await emailTemplateService.GetTemplateAsync("ClientReminderEmailEs", model);
                        var userEmailBody = await emailTemplateService.GetTemplateAsync("UserReminderEmail", model);

                        var clientEmailRequest = new EmailRequestDto
                        {
                            To = reservation.ClientEmail,
                            Subject = reservation.Lang=="en" ? "Appointment Remainder" : "Recordatorio de cita",
                            Body = clientEmailBody
                        };

                        var userEmailRequest = new EmailRequestDto
                        {
                            To = emailAdress.Value ?? "", // Reemplaza con el correo del proveedor real
                            Subject = "Recordatorio de cita con cliente",
                            Body = userEmailBody
                        };

                        if (!string.IsNullOrWhiteSpace(clientEmailRequest.To))
                        {
                            await emailService.SendEmailAsync(clientEmailRequest,[]);
                        }

                        if (!string.IsNullOrWhiteSpace(userEmailRequest.To))
                        {
                            await emailService.SendEmailAsync(userEmailRequest, reservation.ImagePaths);
                        }

                        // Marcar la reserva como notificada
                        reservation.Notified = true;
                        await reservationCommandService.AddOrUpdateReservationAsync(reservation);
                    }
                }
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Appointment Notification Service stopped.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
