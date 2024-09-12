using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using LibraryManagement.API.Controllers;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Tests
{
    /// <summary>
    /// it is not a best practice to have the mock verify calls in the actual test method/
    /// what happens if there is an error/exception that occurs before verify is called?
    /// the mock.verifyAll() should always be called even if the test method fails unexpectedly.
    ///
    /// most tests likely have more than one setup of a mock,  if you don't know which setup method worked
    /// you have to use a debugger to debug your unit tests.
    /// may as well not have written the unit tests in the first place.
    /// </summary>
    public class BooksControllerTests : IDisposable
    {
        private readonly BooksController _bookscontroller;
        private readonly Mock<IBookService> _mockBookService;

        public BooksControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _bookscontroller = new BooksController(_mockBookService.Object);
        }

        public void Dispose() => _mockBookService.VerifyAll();

        [Fact]
        public void CheckOutBook_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            int bookId = 1;
            DateTime dueDate = DateTime.UtcNow.AddDays(7);
            _mockBookService.Setup(service => service.CheckOutBook(bookId, dueDate)).Verifiable();

            // Act
            var result = _bookscontroller.CheckOutBook(bookId, dueDate);

            // Assert
            var actionResult = Assert.IsType<OkResult>(result);
            _mockBookService.Verify();
        }

        [Fact]
        public void ReturnBook_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            int bookId = 1;
            _mockBookService.Setup(service => service.ReturnBook(bookId)).Verifiable();

            // Act
            var result = _bookscontroller.ReturnBook(bookId);

            // Assert
            var actionResult = Assert.IsType<OkResult>(result);
            _mockBookService.Verify();
        }

        [Fact]
        public void GetCheckedOutBooks_ShouldReturnOkWithBooks_WhenSuccessful()
        {
            // Arrange
            var books = new List<Book> { new Book { Id = 1, Title = "Book 1" } };
            _mockBookService.Setup(service => service.GetCheckedOutBooks()).Returns(books);

            // Act
            var result = _bookscontroller.GetCheckedOutBooks();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Book>>(actionResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public void AddBook_ShouldReturnCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "New Book" };
            _mockBookService.Setup(service => service.AddBook(book)).Verifiable();

            // Act
            var result = _bookscontroller.AddBook(book);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(BooksController.GetCheckedOutBooks), actionResult.ActionName);
            _mockBookService.Verify();
        }

        [Fact]
        public void GetAllBooks_ShouldReturnOkWithBooks_WhenSuccessful()
        {
            // Arrange
            var books = new List<Book> { new Book { Id = 1, Title = "Book 1" } };
            _mockBookService.Setup(service => service.GetAllBooks()).Returns(books);

            // Act
            var result = _bookscontroller.GetAllBooks();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Book>>(actionResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public void GetOverdueBooks_ShouldReturnOkWithBooks_WhenSuccessful()
        {
            // Arrange
            var currentDate = DateTime.UtcNow;
            var overdueBooks = new List<Book> { new Book { Id = 1, Title = "Overdue Book" } };
            _mockBookService.Setup(service => service.GetOverdueBooks(currentDate)).Returns(overdueBooks);

            // Act
            var result = _bookscontroller.GetOverdueBooks(currentDate);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Book>>(actionResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public void GetLateFee_ShouldReturnOkWithLateFee_WhenBookExists()
        {
            // Arrange
            int bookId = 1;
            DateTime currentDate = DateTime.UtcNow;
            var book = new Book { Id = 1, Title = "Book 1" };
            decimal lateFee = 5.00m;
            _mockBookService.Setup(service => service.GetAllBooks()).Returns(new List<Book> { book });
            _mockBookService.Setup(service => service.CalculateLateFees(book, currentDate)).Returns(lateFee);

            // Act
            var result = _bookscontroller.GetLateFee(bookId, currentDate);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            dynamic returnValue = actionResult.Value;

            Assert.NotNull(returnValue);        

        }  
              

        [Fact]
        public void CheckOutBook_ShouldReturnStatusCode500_WhenDivideByZeroExceptionOccurs()
        {
            // Arrange
            int bookId = 1;
            DateTime dueDate = DateTime.UtcNow.AddDays(7);
            _mockBookService.Setup(service => service.CheckOutBook(bookId, dueDate))
                            .Throws(new DivideByZeroException());

            // Act
            var result = _bookscontroller.CheckOutBook(bookId, dueDate);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, actionResult.StatusCode);
            Assert.Equal("Attempted to divide by zero.", actionResult.Value);
        }

        [Fact]
        public void SearchBooks_ShouldReturnOkWithBooks_WhenSuccessful()
        {
            // Arrange
            string name = "Book";
            var books = new List<Book> { new Book { Id = 1, Title = "Book 1" } };
            _mockBookService.Setup(service => service.SearchBooksByName(name)).Returns(books);

            // Act
            var result = _bookscontroller.SearchBooks(name);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Book>>(actionResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public void CheckOutBook_ShouldReturnStatusCode400_WhenArgumentExceptionOccurs()
        {
            // Arrange
            int bookId = 1;
            DateTime dueDate = DateTime.UtcNow.AddDays(7);
            _mockBookService.Setup(service => service.CheckOutBook(bookId, dueDate))
                            .Throws(new ArgumentException());

            // Act
            var result = _bookscontroller.CheckOutBook(bookId, dueDate);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Equal("Value does not fall within the expected range.", actionResult.Value);
        }

        [Fact]
        public void RemoveBook_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            int bookId = 1;
            _mockBookService.Setup(service => service.RemoveBook(bookId)).Verifiable();

            // Act
            var result = _bookscontroller.RemoveBook(bookId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            _mockBookService.Verify();
        }


    }

    public class LateFeeResult
    {
        public int BookId { get; set; }
        public decimal LateFee { get; set; }
    }
}
