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
            string query = "SELECT PropertyType, CountClicked FROM Property ORDER BY PropertyType";
            return db.Database.SqlQuery<PopularityStatistic>(query).ToList();
        }

        public IEnumerable<PopulationStatistic> GetPopulationStatistic()
        {
            string populationStatisticQuery = "SELECT District.DistrictName, Estates.EstateName, Population.TotalPopulation " +
                                                "FROM POPULATION " +
                                                "INNER JOIN Estates ON Population.EstateID = Estates.EstateID " +
                                                "INNER JOIN District ON Estates.DistrictID = District.DistrictID " +
                                                "ORDER BY District.DistrictName ASC";

            return db.Database.SqlQuery<PopulationStatistic>(populationStatisticQuery).ToList();
        }

        
    }
}