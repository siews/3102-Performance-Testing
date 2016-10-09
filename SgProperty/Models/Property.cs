using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SgProperty.Models
{
    public class Property
    {
        //internal object DistrictID;

        //TO-DO Property Model Attributes
        [Key]
        public int PropertyID { get; set; }

        public string PropertyName { get; set; }
        public string Address { get; set; }
        public string PropertyType { get; set; }
        public double AskingPrice { get; set; }
        public double AgreedPrice { get; set; }
        public string Image { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ListingType { get; set; }
        public double Size { get; set; }
        public int CountClicked { get; set; }

        [ForeignKey("EstateID")]
        public int fEstateID { get; set; }
        [ForeignKey("AgentID")]
        public int fAgentID { get; set; }

        public virtual Estates EstateID { get; set; }
        public virtual Agent AgentID { get; set; }
        
    }
}