using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs
{
    public struct MessageInfoDto
    {
        //public ulong ProviderId { get; set; }
        public string Message { get; set; }
        public string? ChatId { get; set; }
    }
}
