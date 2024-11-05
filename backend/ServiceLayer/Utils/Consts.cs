using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Utils
{
    public static class Consts
    {
        //Paystack
        public const string Currency = "GHS";
        public const string ChargeUrl = "/charge";
        public const string InitializeTransactionUrl = "/transaction/initialize";
        public const string PaystackSuccessEvent = "charge.success";

        //Arkesel
        public const string ArkeselSmsUrl = "/api/v2/sms/send";

        //AfricasTalking
        public const string AfricasTalkingSmsUrl = "/version1/messaging";
        
        //Hubtel
        public const string HubtelSmsUrl = "/v1/messages/send";

        public const string SytemUser = "System";
    }
}
