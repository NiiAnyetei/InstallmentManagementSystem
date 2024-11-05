using System.ComponentModel;
using System.Globalization;
using System;
using System.Text.Json;
using ServiceLayer.Utils;
using Serilog;
using PhoneNumbers;

namespace IMS.Extensions
{
    public static class GenericExtensionMethods
    {
        public static string ToJson<T>(this T data)
        {
            return JsonSerializer.Serialize(data, JsonConverterSettings.Options);
        }
        
        public static string GetNationalNumber(this string phoneNumber)
        {
            try
            {
                var util = PhoneNumberUtil.GetInstance();
                var parsed = util.Parse(phoneNumber, "");
                return $"0{parsed.NationalNumber.ToString()}";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Parsing PhoneNumber, Exception {ex}");
                throw;
            }
        }
    }
}
