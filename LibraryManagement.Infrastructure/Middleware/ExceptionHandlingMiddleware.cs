// File path: LibraryManagement/Infrastructure/Middleware/ExceptionHandlingMiddleware.cs
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using LibraryManagement.Infrastructure.Utilities;
using System.Net.Http;
using System.Text.Json;

namespace LibraryManagement.Infrastructure.Middleware
{
    // typically when throwing an exception 
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
                
                // I think this sould be moved into a place that can be reused.
                // if continue to build this, are you going to copy/paste this code every time 
                // you need to implement exception hanlding logic?
                
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
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
