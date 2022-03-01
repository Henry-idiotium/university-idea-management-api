using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UIM.Common;
using UIM.Common.ResponseMessages;
using UIM.Model.Dtos.Common;

namespace UIM.API.Middlewares
{
    public static class ExceptionHandlingMiddlewareExt
    {
        public static void UseExceptionHandlingExt(this IApplicationBuilder app) =>
            app.UseMiddleware<HttpStatusExceptionHandlerMiddleware>();
    }

    public class HttpStatusExceptionHandlerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public HttpStatusExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<HttpStatusExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is not HttpException)
                    _logger.LogError(ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = (int)HttpStatusCode.InternalServerError; // Internal Server Error by default
            if (exception is HttpException httpException)
            {
                code = (int)httpException.Status;
                context.Response.Headers.Add("X-Log-Status-Code", httpException.Status.ToString());
                context.Response.Headers.Add("X-Log-Message", exception.Message);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            var response = JsonConvert.SerializeObject(new GenericResponse
            (
                message: (exception.Message != null) && (exception is HttpException) ?
                          exception.Message : ErrorResponseMessages.UnexpectedError,
                succeeded: false
            ),
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });

            await context.Response.WriteAsync(response);
        }
    }
}
