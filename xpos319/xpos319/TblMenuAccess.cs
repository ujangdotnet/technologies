using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace xpos319
{
    [Table("TblMenuAccess")]
    public partial class TblMenuAccess
    {
        [Key]
        public int Id { get; set; }
        public int? IdRole { get; set; }
        public int? MenuId { get; set; }
        public bool? IsDelete { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
