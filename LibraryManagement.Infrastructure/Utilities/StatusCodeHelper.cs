using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Utilities
{
    public static class StatusCodeHelper
    {
        public const int BadRequest = 400;
        public const int NotFound = 404;
        public const int InternalServerError = 500;

        public static int GetStatusCodeForException(Exception ex)
        {
            return ExceptionHandler.HandleException(ex);
        }
    }
}
