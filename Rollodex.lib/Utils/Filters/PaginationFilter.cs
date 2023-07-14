using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rolodex.Lib.Utils.Filters
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string sortBy { get; set; }
        public string filterBy { get; set; }
        public string sortOrder { get; set; }
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
            sortBy = "";
            filterBy = "";
            sortOrder = "";

        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize == 0 ? 10 : pageSize;
        }
    }


     public class LogPaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string sortBy { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime ?endDate { get; set; }
        public string Type { get; set; }
        public string sortOrder { get; set; }
        public int ClientId { get; set; }
        public bool ExportResult { get; set; }

        public LogPaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
            sortBy = "";
            sortOrder = "";
            Type = "";
            ExportResult = false;
        }

        public LogPaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize == 0 ? 10 : pageSize;
        }
    }


    public class PagedParameter
    {
        public PaginationFilter PaginationFilter { get; set; }
        public string Route { get; set; }
    }

    //search filter



}
