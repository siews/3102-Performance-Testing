using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgProperty.Models
{
    public class AverageViewModel
    {
        public List<Average> districtAverages { get; set; }
        public List<Average> propertyTypesAverages { get; set; }
    }
}