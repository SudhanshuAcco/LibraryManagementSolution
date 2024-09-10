using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Tests
{
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

