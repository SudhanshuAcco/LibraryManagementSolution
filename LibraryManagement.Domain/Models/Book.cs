using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime? DueDate { get; set; } 

        public bool IsCheckedOut { get; set; } = false;

        public bool IsOverdue(DateTime currentDate)
        {
            return IsCheckedOut && DueDate < currentDate;
        }
    }
}
