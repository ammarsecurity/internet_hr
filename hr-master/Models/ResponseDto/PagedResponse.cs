using hr_master.Models.Response.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.ResponseDto
{
    public class PagedResponse<T> : ResponseList<T>
    {
        public int PageNumber { get; set; }

        public decimal totelprice { get; set; }

        public decimal pr { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int TotalRecords, decimal totelprice, decimal pr)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = TotalRecords;
            this.pr = pr;
            this.Data = data;
            this.Message = totelprice.ToString();
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
