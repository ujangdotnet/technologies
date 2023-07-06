﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace xpos319.viewmodels
{
    public class VMTblProduct
    {
        public int Id { get; set; }
        public int IdVariant { get; set; }
        public int IdCategory { get; set; }
        public string NameProduct { get; set; } = null!;
        public string NameVariant { get; set; } = null!;
        public string NameCategory { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public bool? IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public IFormFile ImageFile { get; set; } 
    }
}
