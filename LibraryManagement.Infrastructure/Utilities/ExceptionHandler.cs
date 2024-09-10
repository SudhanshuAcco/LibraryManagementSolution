using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Utilities
{
    public class ExceptionHandler
    {
        // This method will handle the exceptions globally
        public static int HandleException(Exception ex)
        {
            // Log the exception to the console (or file, database, etc.)
            Console.WriteLine($"Exception Type: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            // Determine the status code based on exception type
            int statusCode;

            if (ex is DivideByZeroException) {
                Console.WriteLine("A divide by zero exception occurred.");
                statusCode = 500; // Internal Server Error
            } else if (ex is NullReferenceException) {
                Console.WriteLine("A null reference exception occurred.");
                statusCode = 400; // Bad Request
            } else if (ex is FileNotFoundException) {
                Console.WriteLine("A file not found exception occurred.");
                statusCode = 404; // Not Found
            } else if (ex is ArgumentException) {
                Console.WriteLine("An argument exception occurred.");
                statusCode = 400; // Bad Request
            } else if (ex is KeyNotFoundException) {
                Console.WriteLine("A key not found exception occurred.");
                statusCode = 404; // Not Found
            } else if (ex is InvalidOperationException) {
                Console.WriteLine("An invalid operation exception occurred.");
                statusCode = 400; // Bad Request
            } else {
                Console.WriteLine("An unknown exception occurred.");
                statusCode = 500; // Internal Server Error
            }

            // Log exception details to a file
            LogExceptionToFile(ex);

            return statusCode;
        }

        private static void LogExceptionToFile(Exception ex)
        {
            // Example of logging exception details to a file
            string filePath = "exception_log.txt";
            File.AppendAllText(filePath, $"{DateTime.Now}: {ex.GetType().Name} - {ex.Message}{Environment.NewLine}");
        }
    }
}
