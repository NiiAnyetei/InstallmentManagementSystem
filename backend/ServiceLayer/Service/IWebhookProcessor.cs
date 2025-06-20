﻿using DataLayer.Paystack.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface IWebhookProcessor
    {
        Task ProcessWebhookEventAsync(PaystackWebhookDto dto);
    }
}
