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
        /// <summary>
        /// this method is over 50 lines long.  already violating most static code analysis tools.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static int HandleException(Exception ex)
        {
            // Log the exception to the console (or file, database, etc.)
            Console.WriteLine($"Exception Type: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            // Determine the status code based on exception type          

            int statusCode;
            
            // this is not how I would have handled messages based on providing a friendly message
            // by excetion type.   
            // Imagine you have a system that throws over 50 - 100 unique exceptions?
            //

            switch (ex) {
            case DivideByZeroException _:
                Console.WriteLine("A divide by zero exception occurred.");
                statusCode = 500; // Internal Server Error
                break;

            case NullReferenceException _:
                Console.WriteLine("A null reference exception occurred.");
                statusCode = 400; // Bad Request
                break;

            case FileNotFoundException _:
                Console.WriteLine("A file not found exception occurred.");
                statusCode = 404; // Not Found
                break;

            case ArgumentException _:
                Console.WriteLine("An argument exception occurred.");
                statusCode = 400; // Bad Request
                break;

            case KeyNotFoundException _:
                Console.WriteLine("A key not found exception occurred.");
                statusCode = 404; // Not Found
                break;

            case InvalidOperationException _:
                Console.WriteLine("An invalid operation exception occurred.");
                statusCode = 400; // Bad Request
                break;

            default:
                Console.WriteLine("An unknown exception occurred.");
                statusCode = 500; // Internal Server Error
                break;
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
