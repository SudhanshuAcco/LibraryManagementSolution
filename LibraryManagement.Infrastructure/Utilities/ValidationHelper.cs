using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Utilities
{
    public static class ValidationHelper
    {
        public static void IsNotNull(object value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
        }

        public static void IsNotEmpty(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{paramName} cannot be empty or whitespace.", paramName);
        }

        public static void IsPositive(int value, string paramName)
        {
            if (value <= 0)
                throw new ArgumentException($"{paramName} must be a positive integer.", paramName);
        }

        public static void IsValidDate(DateTime? date, string paramName)
        {
            if (date.HasValue && date.Value < DateTime.Now)
                throw new ArgumentException($"{paramName} cannot be in the past.", paramName);
        }

        public static void IsBoolean(bool value, string paramName)
        {
            // In C#, a boolean is always a boolean, but this is a placeholder for more complex checks
        }

        public static void IsNumeric(string value, string paramName)
        {
            if (!decimal.TryParse(value, out _))
                throw new ArgumentException($"{paramName} must be a numeric value.", paramName);
        }
    }
}
