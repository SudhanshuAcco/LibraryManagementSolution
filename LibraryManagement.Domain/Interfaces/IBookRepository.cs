using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IBookRepository
    {
        void Add(Book book);
        void Remove(int bookId);
        Book Get(int bookId);
        IEnumerable<Book> GetAll();
        IEnumerable<Book> GetCheckedOutBooks();
    }
}
