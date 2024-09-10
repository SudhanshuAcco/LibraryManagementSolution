using LibraryManagement.Application.Extensions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;
        public const decimal LateFeePerDay = 0.5m;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            
        }

        public void CheckOutBook(int bookId, DateTime dueDate)
        {
            var book = _bookRepository.Get(bookId) ?? throw new KeyNotFoundException($"Book with ID {bookId} not found.");
            book.Validate(BookValidationExtensions.ValidationType.CheckOut);
           
            book.IsCheckedOut = true;
            book.DueDate = dueDate;
            _bookRepository.Add(book);
        }

        public void ReturnBook(int bookId)
        {
            
            var book = _bookRepository.Get(bookId) ?? throw new KeyNotFoundException($"Book with ID {bookId} not found.");
            book.Validate(BookValidationExtensions.ValidationType.IDCheck);
            book.IsCheckedOut = false;
            var lateFee = CalculateLateFee(book.DueDate, DateTime.Now);
            book.DueDate = null;
            _bookRepository.Add(book);
        }

        private decimal CalculateLateFee(DateTime? dueDate, DateTime returnDate)
        {
            if (!dueDate.HasValue || returnDate <= dueDate.Value)
                return 0;

            var lateDays = (returnDate - dueDate.Value).Days;
            return lateDays * LateFeePerDay;
        }

        public IEnumerable<Book> GetCheckedOutBooks() => _bookRepository.GetCheckedOutBooks();

        public void AddBook(Book book)
        {
            book.Validate(BookValidationExtensions.ValidationType.Add);
            // Assuming the repository has an Add method
            _bookRepository.Add(book);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public void RemoveBook(int bookId)
        {
            var book = _bookRepository.Get(bookId);
            ValidationHelper.IsPositive(book.Id, nameof(book.Id));
            _bookRepository.Remove(bookId);
        }

        public IEnumerable<Book> GetOverdueBooks(DateTime currentDate)
        {
            return _bookRepository.GetAll().Where(book => book.IsOverdue(currentDate));
        }

        public decimal CalculateLateFees(Book book, DateTime currentDate)
        {
            if (!book.IsCheckedOut || !book.IsOverdue(currentDate))
                return 0;

            var overdueDays = (currentDate - book.DueDate.Value).Days;
            return overdueDays * LateFeePerDay;
        }

        public IEnumerable<Book> SearchBooksByName(string name)
        {
            var allBooks = _bookRepository.GetAll();
            return allBooks.Where(book => book.Title.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
