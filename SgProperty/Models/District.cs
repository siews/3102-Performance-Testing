using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SgProperty.Models
{
    public class District
    {
        [Key]
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
    }
}