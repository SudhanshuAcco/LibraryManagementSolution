using LibraryManagement.Domain.Models;

namespace LibraryManagement.Application.Services
{
    public interface IBookService
    {
        void CheckOutBook(int bookId, DateTime dueDate);
        void ReturnBook(int bookId);
        IEnumerable<Book> GetCheckedOutBooks();
        void AddBook(Book book);

        IEnumerable<Book> GetAllBooks();
        void RemoveBook(int bookId);

        IEnumerable<Book> GetOverdueBooks(DateTime currentDate);
        decimal CalculateLateFees(Book book, DateTime currentDate);

        IEnumerable<Book> SearchBooksByName(string name);
    }
}
