using App.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequestDto emailRequest, string[] paths);
    }
}
