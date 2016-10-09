using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SgProperty.Models
{
    public class Population
    {
        [Key]
        public int PopulationID { get; set; }
        public int lessthan20 { get; set; }
        public int between20n39 { get; set; }
        public int between40n59 { get; set; }
        public int lessthan60 { get; set; }
        public int TotalPopulation { get; set; }
    }
}