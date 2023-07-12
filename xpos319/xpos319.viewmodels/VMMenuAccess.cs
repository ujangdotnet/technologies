using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xpos319.viewmodels
{
    public class VMMenuAccess
    {
        public int Id { get; set; }
        public string? MenuName { get; set; }
        public string? MenuController { get; set; }
        public string? MenuAction { get; set; }
        public string? MenuIcon { get; set; }
        public int? MenuSorting { get; set;}
        public int? IsParent { get; set; }
        public int? IdRole { get; set; }
        public string? RoleName { get; set; }
        public int? IdMenu { get; set; }
        public bool is_selected { get; set; }

        public List<VMMenuAccess> role_menu { get; set; }
    }
}
