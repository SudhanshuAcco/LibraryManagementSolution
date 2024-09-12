using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// clean up these using.  Nutrien expects this.   

namespace LibraryManagement.Application.Extensions
{
    public static class BookValidationExtensions
    {
        public static void Validate(this Book book, ValidationType validationType)
        {
            // why aren't these methods on the static ValidationHelper class not method extensions?
            // provides a much cleaner interface
            ValidationHelper.IsNotNull(book, nameof(book));
            ValidationHelper.IsPositive(book.Id, nameof(book.Id));
            ValidationHelper.IsNotEmpty(book.Title, nameof(book.Title));

            if (validationType == ValidationType.CheckOut || validationType == ValidationType.Return) {
                ValidationHelper.IsBoolean(book.IsCheckedOut, nameof(book.IsCheckedOut));

                if (book.DueDate.HasValue) {
                    ValidationHelper.IsValidDate(book.DueDate, nameof(book.DueDate));
                }
            }

            //
            // I see no reason why we need this.  could you not have had other validation methods depending on
            // the action being performed?  
            // too much conditional logic 
            //
            //  A validator is allowed to have more than a single method.  
            //
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
