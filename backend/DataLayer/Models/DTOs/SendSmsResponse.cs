﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.DTOs
{
    public class SendSmsResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
