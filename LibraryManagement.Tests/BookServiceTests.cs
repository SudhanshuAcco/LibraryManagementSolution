using Moq;
using Xunit;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_mockBookRepository.Object);
        }

        [Fact]
        public void CheckOutBook_ShouldSetDueDate()
        {
            // Arrange
            var bookId = 1;
            var dueDate = DateTime.Now.AddDays(7);
            var book = new Book { Id = bookId, Title = "Test Book" };
            _mockBookRepository.Setup(repo => repo.Get(bookId)).Returns(book);

            // Act
            _bookService.CheckOutBook(bookId, dueDate);

            // Assert
            Assert.True(book.IsCheckedOut);
            Assert.Equal(dueDate, book.DueDate);
        }

        [Fact]
        public void ReturnBook_ShouldClearDueDate()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { Id = bookId, Title = "Test Book", DueDate = DateTime.Now.AddDays(7) };
            _mockBookRepository.Setup(repo => repo.Get(bookId)).Returns(book);

            // Act
            _bookService.ReturnBook(bookId);

            // Assert
            Assert.False(book.IsCheckedOut);
            Assert.Null(book.DueDate);
        }

        [Fact]
        public void GetOverdueBooks_ShouldReturnOverdueBooks()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var overdueBook = new Book { Id = 1, DueDate = currentDate.AddDays(-1) ,IsCheckedOut = true };
            var notOverdueBook = new Book { Id = 2, DueDate = currentDate.AddDays(1) , IsCheckedOut = true };
            _mockBookRepository.Setup(repo => repo.GetAll()).Returns(new List<Book> { overdueBook, notOverdueBook });

            // Act
            var result = _bookService.GetOverdueBooks(currentDate);

            // Assert
            Assert.Contains(overdueBook, result);
            Assert.DoesNotContain(notOverdueBook, result);
        }

        [Fact]
        public void CalculateLateFees_ShouldReturnCorrectAmount()
        {
            // Arrange
            var book = new Book { Id = 1, DueDate = DateTime.Now.AddDays(-5), IsCheckedOut=true };
            var currentDate = DateTime.Now;
            var expectedLateFee = 5 * BookService.LateFeePerDay; // Assuming $1 per day

            // Act
            var result = _bookService.CalculateLateFees(book, currentDate);

            // Assert
            Assert.Equal(expectedLateFee, result);
        }
    }
}
