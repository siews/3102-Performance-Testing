using System;
using System.Collections.Generic;
using SgProperty.DAL;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SgProperty.Models;
using System.Web.Helpers;

namespace SgProperty.Controllers
{
    public class StatisticController : Controller
    {
        private StatisticMapper statisticMapper = new StatisticMapper();

        public string StatisticSelection;

        // GET: PopularityStatistic
        public ActionResult Index()
        {
            List<Dropdownlist> dropdownlists = createDropDownList();

            // If no statistic options were chosen
            if (TempData["statisticsOption"] == null)
            {
                // Display default choice (First dropdownlist, First Option value)
                ViewBag.StatisticSelection = dropdownlists.First().listOptions.First().Value;
            }
            else
            {
                ViewBag.StatisticSelection = (string)TempData["statisticsOption"];
            }

            return View(dropdownlists);
        }

        // POST: PopularityStatistic/Index
        [HttpPost]
        public ActionResult Index(string StatisticDropDownList)
        {
            TempData["statisticsOption"] = StatisticDropDownList;

            return RedirectToAction("Index");
        }

        public ActionResult Graph(string type)
        {
            System.Diagnostics.Debug.WriteLine(type);

            // Initialize variables to temporarily hold xval and yval for graph
            List<string> xval = new List<string>();
            List<string> yval = new List<string>();

            if (type == "Property Type Popularity" || type == null)
            {
                IEnumerable<PopularityStatistic> popularityStatisticList = statisticMapper.GetPopularityStatistic();

                string currentPropertyType = popularityStatisticList.First().PropertyType;
                int clickedCountForPropertyType = 0;
                foreach (PopularityStatistic popularityStatistic in popularityStatisticList)
                {
                    if (currentPropertyType.Equals(popularityStatistic.PropertyType))
                    {
                        clickedCountForPropertyType += popularityStatistic.CountClicked;
                    }
                    else
                    {
                        // Next property type, save previous property type data
                        xval.Add(currentPropertyType);
                        yval.Add(clickedCountForPropertyType.ToString());
                        currentPropertyType = popularityStatistic.PropertyType;
                        clickedCountForPropertyType = popularityStatistic.CountClicked;
                    }
                }
                // Save last iterated Property Type and clicked count
                xval.Add(currentPropertyType);
                yval.Add(clickedCountForPropertyType.ToString());

                string[] _xval = xval.ToArray();
                string[] _yval = yval.ToArray();

                var bytes = new Chart(800, 400).AddSeries(
                chartType: "pie",
                legend: "Property Type Popularity",
                xValue: _xval,
                yValues: _yval
                )
                .GetBytes("png");
                return File(bytes, "image/png");
            }
            if (type == "Population in Districts")
            {
                IEnumerable<PopulationStatistic> populationStatisticList = statisticMapper.GetPopulationStatistic();
                
                string currentDistrict = populationStatisticList.First().DistrictName;
                int totalPopulationInDistrict = 0;
                foreach (PopulationStatistic populationStatistic in populationStatisticList)
                {
                    if (currentDistrict.Equals(populationStatistic.DistrictName)) {
                        totalPopulationInDistrict += populationStatistic.TotalPopulation;
                    }
                    else
                    {
                        // Next district, save previous district data
                        xval.Add(currentDistrict);
                        yval.Add(totalPopulationInDistrict.ToString());
                        currentDistrict = populationStatistic.DistrictName;
                        totalPopulationInDistrict = populationStatistic.TotalPopulation;
                    }
                }
                // Save last iterated district and total population
                xval.Add(currentDistrict);
                yval.Add(totalPopulationInDistrict.ToString());

                string[] _xval = xval.ToArray();
                string[] _yval = yval.ToArray();

                //here the chart is going on
                var bytes = new Chart(width: 800, height: 400)
                .AddSeries(
                chartType: "Column", legend: "Population in Districts",
                 xValue: _xval,
                 yValues: _yval)
                .GetBytes("png");
                return File(bytes, "image/png");
            }

            return View();
        }

        private List<Dropdownlist> createDropDownList()
        {
            List<Dropdownlist> allDropdownlists = new List<Dropdownlist>();

            // Get dropdownlist selected options (saved in Index POST function)
            string statisticsOption = (TempData["statisticsOption"] == null) ? "" : (string)TempData["statisticsOption"];

            // Create Statistic choice dropdownlist
            Dropdownlist districtList = new Dropdownlist();
            districtList.listLabel = "Select Statistic:";
            districtList.listName = "StatisticDropDownList";
            districtList.listOptions = new List<SelectListItem>();
            string[] statisticsOptions = { "Property Type Popularity", "Population in Districts" };
            foreach (string option in statisticsOptions)
            {
                districtList.listOptions.Add(
                    new SelectListItem()
                    {
                        Text = option,
                        Value = option,
                        Selected = (statisticsOption.Equals(option)) ? true : false
                    });
            }
            allDropdownlists.Add(districtList);
            return allDropdownlists;
        }
    }
}