using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Abstractions.Services
{
    public interface IEventPublisher
    {
        Task Publish<TEvent>(TEvent @event);
    }

}
