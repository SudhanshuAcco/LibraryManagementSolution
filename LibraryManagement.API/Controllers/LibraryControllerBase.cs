using LibraryManagement.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

public abstract class LibraryControllerBase : ControllerBase
{
    protected IActionResult ExecuteAsync(Func<IActionResult> task)
    {
        try
        {
            return task();
        }
        catch (Exception ex)
        {
            var statusCode = ex.GetStatusCode();
            return StatusCode(statusCode, ex.Message);
        }
    }
        
}