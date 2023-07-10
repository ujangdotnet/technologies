using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xpos319.viewmodels
{
    public class VMSearchPage
    {
        public string? codeTransaction { get; set; }

        public string? NameProduct { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set;}
        public int? PageNumber { get; set; }
        public int? PageSize { get; set;}
        public int? CurrentPageSize { get; set; }

    }
}
