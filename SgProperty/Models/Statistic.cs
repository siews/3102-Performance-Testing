using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SgProperty.Models
{
    public class Statistic
    {
        //TO-DO Statistic Model Attributes
        [Key]
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public int PageCounts { get; set; }
        public int Popularity { get; set; }
        public int EstateId { get; set; }
        public string EstateName { get; set; }
        public int Price { get; set; }
        public long Size { get; set; }
        public decimal AveragePrice { get; set; }
        public string CountListed { get; set; }
    }
}