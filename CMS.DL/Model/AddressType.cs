﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Model
{
    public class AddressType
    {
        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string Name { get; set; }
    }
}
