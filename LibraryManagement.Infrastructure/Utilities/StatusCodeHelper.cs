namespace LibraryManagement.Infrastructure.Utilities
{
    public static class StatusCodeHelper
    {
        public const int BadRequest = 400;
        public const int NotFound = 404;
        public const int InternalServerError = 500;

        public static int GetStatusCode(this Exception ex)
        {
            return ExceptionHandler.HandleException(ex);
        }
    }
}
