using System.Collections.Generic;

namespace UIM.Model.Dtos.Common
{
    public class TableResponse
    {
        public TableResponse(IEnumerable<object> rows, int count, int currentPage, int totalPages)
        {
            Rows = rows;
            Count = count;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }

        public int Count { get; private set; }
        public IEnumerable<object> Rows { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
    }
}