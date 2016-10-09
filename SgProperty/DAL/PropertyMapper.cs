using SgProperty.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SgProperty.DAL
{
    public class PropertyMapper
    {
        private SgPropertyContext db = new SgPropertyContext();

        public IEnumerable<Property> SelectAll()
        {
            string query =
                "SELECT J3.PropertyID, J3.PropertyName, J3.Address, J3.PropertyType, J3.AskingPrice, J3.AgreedPrice, J3.Image, " +
                "J3.Latitude, J3.Longitude, J3.ListingType, J3.Size, J3.CountClicked, J3.EstateID, J3.EstateName, J3.DistrictID, D.DistrictName, " +
                "J3.AgentID, J3.CompanyName, J3.DatePosted, J3.ExclusiveStartDate, J3.ExpiryDate " +
                "FROM (SELECT J2.*, E.EstateName, E.DistrictID " +
                      "FROM (SELECT J1.*, A.CompanyName " +
                             "FROM (SELECT P.*, AMP.AgentID, AMP.DatePosted, AMP.ExclusiveStartDate, AMP.ExpiryDate " +
                                    "FROM Property P " +
                                    "LEFT JOIN Agent_Manages_Property AMP ON AMP.PropertyID = P.PropertyID) as J1 " +
                             "LEFT JOIN Agent A ON A.AgentID = J1.fAgentID) as J2 " +
                      "LEFT JOIN Estates E ON E.EstateID = J2.fEstateID) as J3 " +
                "LEFT JOIN District D ON D.DistrictID = J3.fDistrictID";

            return db.Database.SqlQuery<Property>(query).ToList();
        }

        public Property SelectPropertyById(int? id)
        {
            string query = 
                "SELECT J3.PropertyID, J3.PropertyName, J3.Address, J3.PropertyType, J3.AskingPrice, J3.AgreedPrice, J3.Image, " +
                "J3.Latitude, J3.Longitude, J3.ListingType, J3.Size, J3.CountClicked, J3.EstateID, J3.EstateName, J3.DistrictID, D.DistrictName, " +
                "J3.AgentID, J3.CompanyName, J3.DatePosted, J3.ExclusiveStartDate, J3.ExpiryDate " +
                "FROM (SELECT J2.*, E.EstateName, E.DistrictID " +
                      "FROM (SELECT J1.*, A.CompanyName " +
                             "FROM (SELECT P.*, AMP.AgentID, AMP.DatePosted, AMP.ExclusiveStartDate, AMP.ExpiryDate " +
                                    "FROM Property P " +
                                    "LEFT JOIN Agent_Manages_Property AMP ON AMP.PropertyID = P.PropertyID) as J1 " +
                             "LEFT JOIN Agent A ON A.AgentID = J1.AgentID) as J2 " +
                      "LEFT JOIN Estates E ON E.EstateID = J2.EstateID) as J3 " +
                "LEFT JOIN District D ON D.DistrictID = J3.DistrictID " +
                "WHERE J3.PropertyID = @p0";

            return db.Database.SqlQuery<Property>(query, id).ToList().First();
        }

        public IEnumerable<Property> SearchAll(string keyword)
        {
            string query =
                "SELECT J3.PropertyID, J3.PropertyName, J3.Address, J3.PropertyType, J3.AskingPrice, J3.AgreedPrice, J3.Image, " +
                "J3.Latitude, J3.Longitude, J3.ListingType, J3.Size, J3.CountClicked, J3.EstateID, J3.EstateName, J3.DistrictID, D.DistrictName, " +
                "J3.AgentID, J3.CompanyName, J3.DatePosted, J3.ExclusiveStartDate, J3.ExpiryDate " +
                "FROM (SELECT J2.*, E.EstateName, E.DistrictID " +
                      "FROM (SELECT J1.*, A.CompanyName " +
                             "FROM (SELECT P.*, AMP.AgentID, AMP.DatePosted, AMP.ExclusiveStartDate, AMP.ExpiryDate " +
                                    "FROM Property P " +
                                    "LEFT JOIN Agent_Manages_Property AMP ON AMP.PropertyID = P.PropertyID) as J1 " +
                             "LEFT JOIN Agent A ON A.AgentID = J1.AgentID) as J2 " +
                      "LEFT JOIN Estates E ON E.EstateID = J2.EstateID) as J3 " +
                "LEFT JOIN District D ON D.DistrictID = J3.DistrictID " +
                "WHERE J3.PropertyName LIKE '%"
                + keyword + "%' OR J3.Address LIKE '%"
                + keyword + "%' OR J3.PropertyType LIKE '%"
                + keyword + "%' OR J3.ListingType LIKE '%"
                + keyword + "%' OR J3.Size LIKE '%"
                + keyword + "%' OR J3.EstateName LIKE '%"
                + keyword + "%' OR D.DistrictName LIKE '%" + keyword + "%'";

            DbRawSqlQuery<Property> rawQuery = db.Database.SqlQuery<Property>(query, "");

            return rawQuery.ToList();
        }


        public IEnumerable<string> GetAllEstateNameInDistrict(int? districtID)
        {
            if (districtID == null)     //Retrieve EstateNames for all Districts
                return db.Database.SqlQuery<string>("SELECT DISTINCT EstateName FROM Estates", "").ToList();
            else
                return null;
        }

        public IEnumerable<int> GetDistrictIdByDistrictName(string districtName)
        {
            return db.Database.SqlQuery<int>("SELECT DistrictID FROM District WHERE DistrictName = @p0", districtName);
        }

        public IEnumerable<string> GetAllDistrictName()
        {
            return db.Database.SqlQuery<string>("SELECT DistrictName FROM District", "").ToList();
        }

        public IEnumerable<string> GetAllPropertyTypes()
        {
            return db.Database.SqlQuery<string>("SELECT DISTINCT PropertyType FROM Property ORDER BY PropertyType ASC");
        }

        public IEnumerable<string> GetAllListingTypes()
        {
            return db.Database.SqlQuery<string>("SELECT DISTINCT ListingType FROM Property ORDER BY ListingType ASC");
        }

        public void IncrementCountClickedForPropertyID(int? propertyID)
        {
            if (propertyID != null)
                db.Database.ExecuteSqlCommand("UPDATE Property SET CountClicked = CountClicked + 1 WHERE PropertyID = @p0", propertyID);
        }
    }
}