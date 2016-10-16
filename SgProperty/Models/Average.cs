using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SgProperty.Models
{
    public class Average
    {
        //Keep track of a criteria e.g. a specific district or property type and the average price for that criteria
        [Key]
        public string criteriaName { get; set; }
        public double criteriaAverage { get; set; }
    }
}