using LibraryManagement.Application.Extensions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Utilities;
using LibraryManagement.Domain.Extensions;

namespace LibraryManagement.Application.Services
{
    public class BookService(IBookRepository bookRepository,
        ILateFeeCalculator feeCalculator) : IBookService
    {
        private readonly IBookRepository bookRepository = bookRepository.AssertIsNotNull();
        private readonly ILateFeeCalculator lateFeeCalculator = feeCalculator.AssertIsNotNull();

        public void CheckOutBook(int bookId, DateTime dueDate)
        {
            var book = bookRepository.Get(bookId);
            book.Validate(BookValidationExtensions.ValidationType.CheckOut);
           
            book.IsCheckedOut = true;
            book.DueDate = dueDate;
            bookRepository.Add(book);
        }

        public void ReturnBook(int bookId)
        {
            var book = bookRepository.Get(bookId);
            book.Validate(BookValidationExtensions.ValidationType.IDCheck);
            book.IsCheckedOut = false;
            
            //what are you going to do with this?
            var lateFee = lateFeeCalculator.CalculateLateFee(book, DateTime.Now);
            book.DueDate = null;
            bookRepository.Add(book);
        }

        public IEnumerable<Book> GetCheckedOutBooks() => bookRepository.GetCheckedOutBooks();

        public void AddBook(Book book)
        {
            book.Validate(BookValidationExtensions.ValidationType.Add);
            // Assuming the repository has an Add method
            bookRepository.Add(book);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return bookRepository.GetAll();
        }

        public void RemoveBook(int bookId)
        {
            var book = bookRepository.Get(bookId);
            ValidationHelper.IsPositive(book.Id, nameof(book.Id));
            bookRepository.Remove(bookId);
        }

        public IEnumerable<Book> GetOverdueBooks(DateTime currentDate)
        {
            return bookRepository.GetAll().Where(book => book.IsOverdue(currentDate));
        }

        public decimal CalculateLateFees(Book book, DateTime returnDate)
         => lateFeeCalculator.CalculateLateFee(book, returnDate);

        public IEnumerable<Book> SearchBooksByName(string name)
        {
            var allBooks = bookRepository.GetAll();
            return allBooks.Where(book => book.Title.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
