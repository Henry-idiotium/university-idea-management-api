using System;
using System.Text.Json.Serialization;

namespace UIM.Model.Dtos.Common
{
    public class GenericResponse<T>
    {
        public GenericResponse(string message, bool succeeded = true)
        {
            Message = message;
            Succeeded = succeeded;
            TimeStamp = ConvertDateTime(DateTime.UtcNow);
        }

        public GenericResponse(T data, string message, bool succeeded = true)
        {
            Data = data;
            Message = message;
            Succeeded = succeeded;
            TimeStamp = ConvertDateTime(DateTime.UtcNow);
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; private set; }
        public string Message { get; private set; }
        public bool Succeeded { get; private set; }
        public string TimeStamp { get; private set; }

        static string ConvertDateTime(DateTime time) => time.ToString("yyyy-MM-dd HH:mm:ss tt");
    }
}