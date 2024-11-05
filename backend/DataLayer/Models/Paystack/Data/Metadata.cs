using DataLayer.Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.Models.Paystack.Data
{
    public class Metadata
    {
        [JsonProperty("installmentId")]
        [JsonPropertyName("installmentId")]
        public Guid InstallmentId { get; set; }

        [JsonProperty("billId")]
        [JsonPropertyName("billId")]
        public Guid? BillId { get; set; }
    }
}
