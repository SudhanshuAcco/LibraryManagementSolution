using LibraryManagement.Application;
using Moq;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Tests
{
    public class BookServiceTests : BaseMockTest
    {
        private readonly Mock<IBookRepository> bookRepository;
        private readonly Mock<ILateFeeCalculator> lateFeeCalcutor;
        private readonly BookService bookService;

        public BookServiceTests()
        {
            lateFeeCalcutor = Strict<ILateFeeCalculator>();
            bookRepository = Strict<IBookRepository>();
            bookService = new BookService(bookRepository.Object, lateFeeCalcutor.Object);
        }

        [Fact]
        public void CheckOutBook_ShouldSetDueDate()
        {
            var bookId = 1;
            var dueDate = DateTime.Now.AddDays(7);
            var book = new Book { Id = bookId, Title = "Test Book" };
            bookRepository.Setup(repo => repo.Get(bookId)).Returns(book);

            bookService.CheckOutBook(bookId, dueDate);

            Assert.True(book.IsCheckedOut);
            Assert.Equal(dueDate, book.DueDate);
        }

        [Fact]
        public void ReturnBook_ShouldClearDueDate()
        {
            var bookId = 1;
            var book = new Book { Id = bookId, Title = "Test Book", DueDate = DateTime.Now.AddDays(7) };
            bookRepository.Setup(repo => repo.Get(bookId)).Returns(book);

            bookService.ReturnBook(bookId);

            Assert.False(book.IsCheckedOut);
            Assert.Null(book.DueDate);
        }

        [Fact]
        public void GetOverdueBooks_ShouldReturnOverdueBooks()
        {
            var currentDate = DateTime.Now;
            var overdueBook = new Book { Id = 1, DueDate = currentDate.AddDays(-1) ,IsCheckedOut = true };
            var notOverdueBook = new Book { Id = 2, DueDate = currentDate.AddDays(1) , IsCheckedOut = true };
            bookRepository.Setup(repo => repo.GetAll()).Returns(new List<Book> { overdueBook, notOverdueBook });

            var result = bookService.GetOverdueBooks(currentDate);

            Assert.Contains(overdueBook, result);
            Assert.DoesNotContain(notOverdueBook, result);
        }

        [Fact]
        public void CalculateLateFees_ShouldReturnCorrectAmount()
        {
            var book = new Book { Id = 1, DueDate = DateTime.Now.AddDays(-5), IsCheckedOut=true };
            var currentDate = DateTime.Now;

            lateFeeCalcutor.Setup(x => x.CalculateLateFee(book, currentDate)).Returns(10);
            
            var result = bookService.CalculateLateFees(book, currentDate);

            Assert.Equal(10, result);
        }

        //These tests should be on the LateFeeCalculator class. That is its sole responsbility  SOLID Principals.z  
        /*
        [Fact]
        public void CalculateLateFee_ShouldReturnZero_WhenDueDateIsNull()
        {
            DateTime? dueDate = null;
            DateTime returnDate = DateTime.UtcNow;

            var result = bookService.CalculateLateFee(dueDate, returnDate);

            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateLateFee_ShouldReturnZero_WhenReturnDateIsBeforeOrOnDueDate()
        {
            var dueDate = DateTime.UtcNow.AddDays(-5);
            var returnDate = DateTime.UtcNow.AddDays(-5); // Same day as dueDate

            var result = bookService.CalculateLateFee(dueDate, returnDate);

            Assert.Equal(0, result);
        }
        */
    }
}
