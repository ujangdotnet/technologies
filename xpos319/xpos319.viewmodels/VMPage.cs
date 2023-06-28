﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xpos319.viewmodels
{
    public class VMPage
    {
        public string sortOrder { get; set; }
        public string searchString { get; set; }
        public string currentFilter { get; set; }
        public int? pageNumber { get; set; }
        public int? showData { get; set; }
        public string minSearch { get; set; }
        public string maxSearch { get; set; }
    }
}
