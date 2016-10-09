using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SgProperty.Models
{
    public class Agent
    {
        [Key]
        public int AgentID { get; set; }
        public string CompanyName { get; set; }
    }
}