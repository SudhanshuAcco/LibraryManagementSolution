// File path: LibraryManagement/Infrastructure/Middleware/ExceptionHandlingMiddleware.cs
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using LibraryManagement.Infrastructure.Utilities;
using System.Net.Http;
using System.Text.Json;

namespace LibraryManagement.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try {
                await _next(context);
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCode(ex);
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                // Optional: Write a JSON response body with exception details
                var response = new
                {
                    message = ex.Message,
                    statusCode = statusCode
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
