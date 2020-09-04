using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Utility.Exceptions;
using Utility.Models;

namespace API.Middlewares
{
    public class ExecptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExecptionMiddleware(RequestDelegate next,IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                
                await _next(context);

            }
            catch (Exception ex)
            {
                await HandleExceptionAsynce(context,ex,_env);
            }
        }

        private async Task HandleExceptionAsynce(HttpContext context, Exception exception,IWebHostEnvironment env)
        {
            var code = HttpStatusCode.InternalServerError;
            var errors = new ApiErrorResponse()
            {
                StatusCode = (int) code
            };
            if (_env.IsDevelopment())
            {
                errors.Details = exception.StackTrace;
            }
            else
            {
                errors.Details = exception.Message;
            }

            switch (exception)
            {
                case AplicationValidationException e:
                    errors.Message = e.Message;
                    errors.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    break;
                default:
                    errors.Message = "Something is wrong in our system";
                    break;
            }

            var result = JsonConvert.SerializeObject(errors);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errors.StatusCode;
            await context.Response.WriteAsync(result);
        }
    }
    }