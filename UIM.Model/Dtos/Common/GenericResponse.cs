using System;
using Newtonsoft.Json;
using UIM.Common.ResponseMessages;

namespace UIM.Model.Dtos.Common
{
    public class GenericResponse
    {
        public GenericResponse(bool succeeded = true, string message = SuccessResponseMessages.RequestSucceeded)
        {
            Message = message;
            Succeeded = succeeded;
            TimeStamp = ConvertDateTime(DateTime.UtcNow);
        }

        public GenericResponse(object result, bool succeeded = true, string message = SuccessResponseMessages.RequestSucceeded)
        {
            Result = result;
            Message = message;
            Succeeded = succeeded;
            TimeStamp = ConvertDateTime(DateTime.UtcNow);
        }

        public bool Succeeded { get; }
        public string Message { get; }
        public string TimeStamp { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Result { get; }

        static string ConvertDateTime(DateTime time) => time.ToString("yyyy-MM-dd HH:mm:ss tt");
    }
}