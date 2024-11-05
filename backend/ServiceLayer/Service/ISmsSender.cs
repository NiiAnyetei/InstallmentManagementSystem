using DataLayer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface ISmsSender
    {
        Task<SendSmsResponse> SendSmsAsync(string to, string message);
    }
}
