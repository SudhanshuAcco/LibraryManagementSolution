using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Tests
{
    
    /// <summary>
    /// noticed that every test  sets up the books it needs.    What happens if we update the book object
    /// by adding additonal fields?   You now have to update every test instead of having some reusable
    /// utility for this.
    ///
    /// I am also noticing the same pattern of assertions in most tests.  copy/paste likely is how they were created.
    /// If we are trying to reuse code, is there not a way to create a function to contain all these assertions
    /// and call them from multiple tests?
    /// 
    /// </summary>
    public class InMemoryBookRepositoryTests
    {
        private readonly InMemoryBookRepository _repository;

        public InMemoryBookRepositoryTests()
        {
            _repository = new InMemoryBookRepository();
        }

        [Fact]
        public void Add_ShouldAddNewBook_WhenBookIsNotInRepository()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book Title", Author = "Author" };

            // Act
            _repository.Add(book);

            // Assert
            var result = _repository.Get(1);
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Title, result.Title);
            Assert.Equal(book.Author, result.Author);
        }

        [Fact]
        public void Add_ShouldUpdateBook_WhenBookWithSameIdExists()
        {
            // Arrange
            var book1 = new Book { Id = 1, Title = "Old Title", Author = "Old Author" };
            _repository.Add(book1);
            var book2 = new Book { Id = 1, Title = "New Title", Author = "New Author" };

            //honestly this likely should throw an error since you are effectivly replacing an existing object.
            //that though is more of a business decision.
            // for the purposes of this exercise I would expect that you do NOT allow books to be overwritten on an add
            // that would be an update to an existing book.
            
            // Act
            _repository.Add(book2);

            // Assert
            var result = _repository.Get(1);
            Assert.NotNull(result);
            Assert.Equal(book2.Title, result.Title);
            Assert.Equal(book2.Author, result.Author);
        }

        [Fact]
        public void Remove_ShouldRemoveBook_WhenBookExists()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book Title", Author = "Author" };
            _repository.Add(book);

            // Act
            _repository.Remove(1);

            // Assert
            var result = _repository.Get(1);
            Assert.Null(result);
        }

        [Fact]
        public void Get_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book Title", Author = "Author" };
            _repository.Add(book);

            // Act
            var result = _repository.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Title, result.Title);
            Assert.Equal(book.Author, result.Author);
        }

        [Fact]
        public void Get_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Act
            var result = _repository.Get(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllBooks()
        {
            // Arrange
            var book1 = new Book { Id = 1, Title = "Book 1", Author = "Author 1" };
            var book2 = new Book { Id = 2, Title = "Book 2", Author = "Author 2" };
            _repository.Add(book1);
            _repository.Add(book2);

            // Act
            var result = _repository.GetAll().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, b => b.Id == book1.Id && b.Title == book1.Title);
            Assert.Contains(result, b => b.Id == book2.Id && b.Title == book2.Title);
        }

        [Fact]
        public void GetCheckedOutBooks_ShouldReturnOnlyCheckedOutBooks()
        {
            // Arrange
            var checkedOutBook = new Book { Id = 1, Title = "Checked Out Book", Author = "Author", IsCheckedOut = true };
            var notCheckedOutBook = new Book { Id = 2, Title = "Not Checked Out Book", Author = "Author", IsCheckedOut = false };
            _repository.Add(checkedOutBook);
            _repository.Add(notCheckedOutBook);

            // Act
            var result = _repository.GetCheckedOutBooks().ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(checkedOutBook.Id, result[0].Id);
        }
    }
}

