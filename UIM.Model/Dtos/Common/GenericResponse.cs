using System;
using System.Text.Json.Serialization;
using UIM.Common.ResponseMessages;

namespace UIM.Model.Dtos.Common
{
    public class GenericResponse
    {
        public GenericResponse(string message = SuccessResponseMessages.RequestSucceeded)
        {
            Message = message;
            TimeStamp = ConvertDateTime(DateTime.UtcNow);
        }

        public GenericResponse(object data, string message = SuccessResponseMessages.RequestSucceeded)
        {
            Data = data;
            Message = message;
            TimeStamp = ConvertDateTime(DateTime.UtcNow);
        }

        public string Message { get; }
        public string TimeStamp { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Data { get; }

        static string ConvertDateTime(DateTime time) => time.ToString("yyyy-MM-dd HH:mm:ss tt");
    }
}