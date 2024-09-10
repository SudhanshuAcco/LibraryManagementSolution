using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public interface IBookService
    {
        void CheckOutBook(int bookId, DateTime dueDate);
        void ReturnBook(int bookId);
        decimal CalculateLateFee(DateTime? dueDate, DateTime returnDate);
        IEnumerable<Book> GetCheckedOutBooks();
        void AddBook(Book book);

        IEnumerable<Book> GetAllBooks();
        void RemoveBook(int bookId);

        IEnumerable<Book> GetOverdueBooks(DateTime currentDate);
        decimal CalculateLateFees(Book book, DateTime currentDate);

        IEnumerable<Book> SearchBooksByName(string name);
    }
}
