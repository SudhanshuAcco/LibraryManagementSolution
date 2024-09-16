using LibraryManagement.Domain.Models;

namespace LibraryManagement.Application;

public interface ILateFeeCalculator
{
    decimal CalculateLateFee(Book lateBook, DateTime returnDate);
}

public class LateFeeCalculator : ILateFeeCalculator
{
    public const decimal LateFeePerDay = 0.5m;
    
    public decimal CalculateLateFee(Book book, DateTime returnDate)
    {
        //   
        if (!book.DueDate.HasValue || returnDate <= book.DueDate.Value)
            return 0;

        var lateDays = (returnDate - book.DueDate.Value).Days;
        return lateDays * LateFeePerDay;
    }
}