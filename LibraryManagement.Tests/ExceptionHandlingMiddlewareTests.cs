using LibraryManagement.Infrastructure.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Tests
{
    public class ExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_ShouldSetStatusCodeAndResponseBody()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var exception = new InvalidOperationException("Test exception");
            var statusCode = 400; // Example status code for the exception

            var mockRequestDelegate = new Mock<RequestDelegate>();
            
            // there is never a reason to use this:  It.IsAny<HttpContext>()
            // YOU are setting up the mock,  you should know what you are passing to it.
            
            mockRequestDelegate
                .Setup(rd => rd(It.IsAny<HttpContext>()))
                .Throws(exception);

            var middleware = new ExceptionHandlingMiddleware(mockRequestDelegate.Object);
            var originalResponseBody = context.Response.Body;
            // you have assigned this local variable but are not using it?  
            
            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(statusCode, context.Response.StatusCode);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            string responseBody = await reader.ReadToEndAsync();

            var expectedResponse = new
            {
                message = exception.Message,
                statusCode
            };
            
            var actualResponse = JsonConvert.DeserializeObject<responseMsg>(responseBody);

            // casting is not necessary here.  
            Assert.Equal(expectedResponse.message, (string) actualResponse.message);
            Assert.Equal(expectedResponse.statusCode, (int) actualResponse.statusCode);
        }
    }

    public class responseMsg
    {
        public string message;
        public int statusCode;
    }
}
