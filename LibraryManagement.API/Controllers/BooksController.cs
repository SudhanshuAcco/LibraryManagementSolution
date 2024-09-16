using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Extensions;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(IBookService bookService) : LibraryControllerBase
    {
        private readonly IBookService bookService = bookService.AssertIsNotNull();

        [HttpPost("{bookId}/checkout")]
        public IActionResult CheckOutBook(int bookId, [FromQuery] DateTime dueDate)
        {
            return ExecuteAsync(() =>
            {
                bookService.CheckOutBook(bookId, dueDate);
                return Ok();
            }); 
        }

        [HttpPost("{bookId}/return")]
        public IActionResult ReturnBook(int bookId)
        {
            return ExecuteAsync(() =>
            {
                bookService.ReturnBook(bookId);
                return Ok();
            });
        }

        [HttpGet("checkedout")]
        public IActionResult GetCheckedOutBooks()
        {
            return ExecuteAsync(() =>
            {
                var books = bookService.GetCheckedOutBooks();
                return Ok(books);
            });
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            return ExecuteAsync(() =>
            {
                bookService.AddBook(book);
                return CreatedAtAction(nameof(GetCheckedOutBooks), new { id = book.Id }, book);
            });
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return ExecuteAsync(() =>
            {
                var books = bookService.GetAllBooks();
                return Ok(books);
            });
        }

        [HttpDelete("{bookId}")]
        public IActionResult RemoveBook(int bookId)
        {
            return ExecuteAsync(() =>
            {
                bookService.RemoveBook(bookId);
                return NoContent(); // HTTP 204 No Content
            });
        }

        [HttpGet("overdue")]
        public IActionResult GetOverdueBooks([FromQuery] DateTime currentDate)
        {
            return ExecuteAsync(() =>
            {
                var overdueBooks = bookService.GetOverdueBooks(currentDate);
                return Ok(overdueBooks);
            });
        }

        [HttpGet("{bookId}/latefee")]
        public IActionResult GetLateFee(int bookId, [FromQuery] DateTime currentDate)
        {
            return ExecuteAsync(() =>
            {
                var book = bookService.GetAllBooks().FirstOrDefault(b => b.Id == bookId);
                if (book == null)
                    return NotFound("Book not found");

                var lateFee = bookService.CalculateLateFees(book, currentDate);
                return Ok(new { BookId = bookId, LateFee = lateFee });
            });
        }


        [HttpGet("search")]
        public IActionResult SearchBooks([FromQuery] string name)
        {
            return ExecuteAsync(() =>
            {
                var books = bookService.SearchBooksByName(name);
                return Ok(books);
            });
        }
    }
}
