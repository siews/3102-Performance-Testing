using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SgProperty.Models
{
    public class Estates
    {
        [Key]
        public int EstateID { get; set; }
        public string EstateName { get; set; }


        [ForeignKey("DistrictID")]
        public int fDistrictID { get; set; }
        [ForeignKey("PopulationID")]
        public int fPopulationID { get; set; }


        public virtual District DistrictID { get; set; }
        public virtual Population PopulationID { get; set; }
    }
}