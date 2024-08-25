using App.Application.Abstractions.Services;
using App.Application.DTOs;
using App.Domain.Entities;
using App.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Abstractions.Implementations
{
    public class AppointmentCreatedEventHandler : IEventHandler<AppointmentCreatedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AppointmentCreatedEventHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
                
        public async Task Handle(AppointmentCreatedEvent @event)
        {
            var reservation = @event.Reservation;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var emailTemplateService = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();
                var reservationQueryService = scope.ServiceProvider.GetRequiredService<IReservationQueryService>();
                var configValueQueryService = scope.ServiceProvider.GetRequiredService<IConfigValueQueryService>();

                var basicConfigurations = await configValueQueryService.GetBasicsConfigValuesAsync();
                var emailAddress = basicConfigurations?.FirstOrDefault(cv => cv.ValueType == CacheValueType.EmailAddress) ?? new ConfigValueDto();
                var phoneFacebook = basicConfigurations?.FirstOrDefault(cv => cv.ValueType == CacheValueType.PhoneFacebook) ?? new ConfigValueDto();
                var tiktokInstagram = basicConfigurations?.FirstOrDefault(cv => cv.ValueType == CacheValueType.TikTokInstagram) ?? new ConfigValueDto();
                var location = basicConfigurations?.FirstOrDefault(cv => cv.ValueType == CacheValueType.StudioLocation) ?? new ConfigValueDto();

                double lat = 27.99819074; // Default value
                double lng = -82.49170303; // Default value
                if (!string.IsNullOrWhiteSpace(location?.Value ?? ""))
                {
                    var coords = location.Value.Split(',');
                    if (coords.Length == 2 &&
                        double.TryParse(coords[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out lat) &&
                        double.TryParse(coords[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out lng))
                    {
                        // Coordinates parsed successfully
                    }
                }

                var model = new ReminderEmailModel
                {
                    ClientName = reservation.ClientName,
                    ReservationDate = reservation.ReservationDateStart,
                    Description = reservation.Details,
                    ProviderName = "Ink Vibes Tattoo Studio", // Replace with the real provider name
                    Address = emailAddress?.ValueDescription ?? "Unknown Address",
                    PhoneNumber = phoneFacebook?.Value ?? "Unknown Phone",
                    Facebook = phoneFacebook?.ValueDescription ?? "Unknown Facebook",
                    Instagram = tiktokInstagram?.ValueDescription ?? "Unknown Instagram",
                    TikTok = tiktokInstagram?.Value ?? "Unknown Tiktok",
                    Latitude = lat,
                    Longitude = lng
                };

                var clientEmailBody = reservation.Lang == "en" ? await emailTemplateService.GetTemplateAsync("ClientCreationNotificationEmail", model) : await emailTemplateService.GetTemplateAsync("ClientCreationNotificationEmailEs", model);
                var userEmailBody = await emailTemplateService.GetTemplateAsync("UserCreationNotificationEmail", reservation);

                var clientEmailRequest = new EmailRequestDto
                {
                    To = reservation.ClientEmail,
                    Subject = reservation.Lang == "en" ? "Appointment Solicited" : "Cita solicitada",
                    Body = clientEmailBody
                };

                var userEmailRequest = new EmailRequestDto
                {
                    To = emailAddress?.Value ?? "", // Replace with the real provider email
                    Subject = "Client Appointment Solicited",
                    Body = userEmailBody
                };

                if (!string.IsNullOrWhiteSpace(userEmailRequest.To))
                {
                    await emailService.SendEmailAsync(userEmailRequest, reservation.ImagePaths);
                }

                if (!string.IsNullOrWhiteSpace(clientEmailRequest.To))
                {
                    await emailService.SendEmailAsync(clientEmailRequest, []);
                }
            }
        }

    }
}
