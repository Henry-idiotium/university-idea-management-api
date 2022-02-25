using System;
using System.Globalization;
using System.Net;

namespace UIM.Common
{
    public class HttpException : Exception
    {
        public HttpException(HttpStatusCode status, string message = null) : base(message)
        {
            Status = status;
        }

        public HttpException(HttpStatusCode status, string message = null, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            Status = status;
        }

        public HttpStatusCode Status { get; private set; }
    }
}