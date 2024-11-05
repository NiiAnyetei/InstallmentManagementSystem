using DataLayer.Models.DTOs;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Provider
{
    public class NotificationService : INotificationService
    {
        private readonly ISmsSender _smsSender;

        public NotificationService(ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }

        public async Task<SendSmsResponse> SendSmsAsync(string phoneNumber, string message)
        {
            return await _smsSender.SendSmsAsync(phoneNumber, message);
        }
    }
}
