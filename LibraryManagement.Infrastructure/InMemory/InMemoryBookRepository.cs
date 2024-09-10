using System.Collections.Generic;
using System.Linq;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Infrastructure.InMemory
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();

        public void Add(Book book)
        {
            var existingBook = _books.SingleOrDefault(b => b.Id == book.Id);
            if (existingBook != null) {
                _books.Remove(existingBook);
            }
            _books.Add(book);
        }

        public void Remove(int bookId)
        {
            var book = _books.SingleOrDefault(b => b.Id == bookId);
            if (book != null) {
                _books.Remove(book);
            }
        }

        public Book Get(int bookId) => _books.SingleOrDefault(b => b.Id == bookId);

        public IEnumerable<Book> GetAll() => _books;

        public IEnumerable<Book> GetCheckedOutBooks() => _books.Where(b => b.IsCheckedOut);
    }
}
