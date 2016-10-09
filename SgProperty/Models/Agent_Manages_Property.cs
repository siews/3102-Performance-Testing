using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SgProperty.Models
{
    public class Agent_Manages_Property
    {
        [Key]
        public int AgentManagesPropertyID { get; set; }
        public string DatePosted { get; set; }
        public string ExclusiveStartDate { get; set; }
        public string ExpiryDate { get; set; }

        [ForeignKey("AgentID")]
        public int fAgentID { get; set; }
        [ForeignKey("PropertyID")]
        public int fPropertyID { get; set; }

        public virtual Agent AgentID { get; set; }
        public virtual Property PropertyID { get; set; }
    }
}