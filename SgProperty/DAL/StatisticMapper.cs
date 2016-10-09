using SgProperty.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace SgProperty.DAL
{
    public class StatisticMapper
    {
        private SgPropertyContext db = new SgPropertyContext();

        public IEnumerable<PopularityStatistic> GetPopularityStatistic()
        {
            string query = "SELECT PropertyType, CountClicked FROM properties ORDER BY PropertyType";
            return db.Database.SqlQuery<PopularityStatistic>(query).ToList();
        }

        public IEnumerable<PopulationStatistic> GetPopulationStatistic()
        {
            string populationStatisticQuery = "SELECT districts.DistrictName, estates.EstateName, populations.TotalPopulation " +
                                                "FROM populations " +
                                                "INNER JOIN estates ON populations.PopulationID = estates.fPopulationID " +
                                                "INNER JOIN districts ON estates.fDistrictID = districts.DistrictID " +
                                                "ORDER BY districts.DistrictName ASC";

            return db.Database.SqlQuery<PopulationStatistic>(populationStatisticQuery).ToList();
        }

        
    }
}