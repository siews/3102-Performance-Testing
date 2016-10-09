using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SgProperty.Models
{
    public class PopularityStatistic
    {

        //Popularity Statistic Model Attributes
        public string PropertyType { get; set; }
        public int CountClicked { get; set; }

    }
}