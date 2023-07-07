using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace xpos319.datamodels
{
    [Table("TblRole")]
    public partial class TblRole
    {
        [Key]
        public int Id { get; set; }
        [StringLength(80)]
        [Unicode(false)]
        public string? RoleName { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
