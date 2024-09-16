using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Utilities;

namespace LibraryManagement.Application.Extensions
{
    public static class BookValidationExtensions
    {
        public static void Validate(this Book book, ValidationType validationType)
        {
            book.IsNotNull(nameof(book));
            book.Id.IsPositive(nameof(book.Id));
            book.Title.IsNotEmpty(nameof(book.Title));

            if (validationType == ValidationType.CheckOut || validationType == ValidationType.Return) {
                book.IsCheckedOut.IsBoolean(nameof(book.IsCheckedOut));

                if (book.DueDate.HasValue) {
                    book.DueDate.IsValidDate(nameof(book.DueDate));
                }
            }

            switch (validationType) {
                case ValidationType.CheckOut:
                    if (book.IsCheckedOut)
                        throw new InvalidOperationException("Book is already checked out.");
                    break;

                case ValidationType.Return:
                    if (!book.IsCheckedOut)
                        throw new InvalidOperationException("Book is not checked out.");
                    break;

                case ValidationType.Removal:
                    // Example validation for removal, if needed
                    break;

                case ValidationType.Add:
                    //
                    break;

                case ValidationType.IDCheck:
                    //
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(validationType), "Invalid validation type.");
            }
        }

        public enum ValidationType
        {
            CheckOut,
            Return,
            Removal,
            Add,
            IDCheck
        }
    }
}
