using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models.Pagination
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> items, int totalPages, int pageNumber)
        {
            this.AddRange(items);
            this.TotalPages = totalPages;
            this.PageNumber = pageNumber;
        }

        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public bool HasPrevPage
        {
            get
            {
                return this.PageNumber > 1;
            }
        }
        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }
    }
}
