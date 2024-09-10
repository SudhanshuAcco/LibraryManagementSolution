using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Utilities;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            
        }

        [HttpPost("{bookId}/checkout")]
        public IActionResult CheckOutBook(int bookId, [FromQuery] DateTime dueDate)
        {
            try {
                _bookService.CheckOutBook(bookId, dueDate);
                return Ok();
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            } 
        }

        [HttpPost("{bookId}/return")]
        public IActionResult ReturnBook(int bookId)
        {
            try {
                _bookService.ReturnBook(bookId);
                return Ok();
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            }
        }

        [HttpGet("checkedout")]
        public IActionResult GetCheckedOutBooks()
        {
            try { 
            var books = _bookService.GetCheckedOutBooks();
            return Ok(books);
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            try {
                _bookService.AddBook(book);
                return CreatedAtAction(nameof(GetCheckedOutBooks), new { id = book.Id }, book);
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try {
                var books = _bookService.GetAllBooks();
                return Ok(books);
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            }
        }

        [HttpDelete("{bookId}")]
        public IActionResult RemoveBook(int bookId)
        {
            try {
                _bookService.RemoveBook(bookId);
                return NoContent(); // HTTP 204 No Content
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            }
        }

        [HttpGet("overdue")]
        public IActionResult GetOverdueBooks([FromQuery] DateTime currentDate)
        {
            try {
                var overdueBooks = _bookService.GetOverdueBooks(currentDate);
                return Ok(overdueBooks);
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            }
        }

        [HttpGet("{bookId}/latefee")]
        public IActionResult GetLateFee(int bookId, [FromQuery] DateTime currentDate)
        {
            try {
                var book = _bookService.GetAllBooks().FirstOrDefault(b => b.Id == bookId);
                if (book == null)
                    return NotFound("Book not found");

                var lateFee = _bookService.CalculateLateFees(book, currentDate);
                return Ok(new { BookId = bookId, LateFee = lateFee });
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            }
        }


        [HttpGet("search")]
        public IActionResult SearchBooks([FromQuery] string name)
        {
            try {
                var books = _bookService.SearchBooksByName(name);
                return Ok(books);
            } catch (Exception ex) {
                int statusCode = StatusCodeHelper.GetStatusCodeForException(ex);
                return StatusCode(statusCode, ex.Message);
            }

        }
    }
}
