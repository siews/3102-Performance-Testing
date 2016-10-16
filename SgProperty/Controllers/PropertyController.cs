using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SgProperty.Models;
using System.Data.Entity.Infrastructure;
using SgProperty.DAL;
using System.Diagnostics;
using System.Dynamic;
using PagedList;

namespace SgProperty.Controllers
{
    public class PropertyController : Controller
    {
        private PropertyMapper propertyMapper = new PropertyMapper();
        private static readonly string defaultListOption = "Any";
        

        public ActionResult Index()
        {
            return RedirectToAction("Listing");
        }


        // GET: Property
        public ActionResult Listing(string search, int? page)
        {
            // Create dynamic object that holds models to pass to view
            dynamic listingModel = new ExpandoObject();
            listingModel.Dropdownlists = createDropDownLists();

            ViewBag.search = search;    // Used for hidden form input for filtering data, pagination

            int pageSize = 10;
            int pageNumber = (page ?? 1);   // return the value of page if it has a value, or return 1 if page is null

            // Check for result set availability
            if (TempData["result_set"] != null)
            {
                // There was a previous function that processed data and redirected the action here with a result set
                listingModel.Property = ((IEnumerable<Property>)TempData["result_set"]).ToPagedList(pageNumber, pageSize);
                return View(listingModel);
            }
            else
            {
                // Check if search parameters were entered
                if (!String.IsNullOrWhiteSpace(search))
                {
                    // Perform search
                    listingModel.Property = propertyMapper.SearchAll(search).ToPagedList(pageNumber, pageSize);
                    return View(listingModel);
                }
                else
                {
                    // Search either not performed or invalid
                    listingModel.Property = propertyMapper.SelectAll().ToPagedList(pageNumber, pageSize);
                    return View(listingModel);
                }
            }
        }


        // POST: Property/Listing
        [HttpPost]
        public ActionResult Listing(string DistrictDropDownList, string PropertyTypeDropDownList, string ListTypeDropDownList, string PriceDropDownList, string Search)
        {
            // Save dropdownlist options to TempData
            TempData["districtOption"] = DistrictDropDownList;
            TempData["propertyTypeOption"] = PropertyTypeDropDownList;
            TempData["listingTypeOption"] = ListTypeDropDownList;
            TempData["priceOption"] = PriceDropDownList;

            IEnumerable<Property> properties;

            // If search parameters not set
            if (String.IsNullOrWhiteSpace(Search))
                properties = propertyMapper.SelectAll();   // Filter for all properties
            else
            {
                properties = propertyMapper.SearchAll(Search);    // Filter for searched properties
            }

            // Filtering logic
            if (!DistrictDropDownList.Equals(defaultListOption))
            {
                properties = properties.Where(p => p.DistrictID.Equals(propertyMapper.GetDistrictIdByDistrictName(DistrictDropDownList).First()));
            }

            if (!PropertyTypeDropDownList.Equals(defaultListOption))
            {
                properties = properties.Where(p => p.PropertyType.Equals(PropertyTypeDropDownList));
            }

            if (!ListTypeDropDownList.Equals(defaultListOption))
            {
                properties = properties.Where(p => p.ListingType.Equals(ListTypeDropDownList));
            }
            
            try
            {
                int price = Int32.Parse(PriceDropDownList);
                if (price != 0)
                {
                    properties = properties.Where(p => p.AgreedPrice < price);
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            // TempData to send back to Listing page for display
            TempData["result_set"] = properties;

            if (String.IsNullOrWhiteSpace(Search))
                return RedirectToAction("Listing");
            else
                return RedirectToAction("Listing", new { search = Search });
        }


        // GET: Property/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Listing");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = propertyMapper.SelectPropertyById(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            // Increment ClickCount
            propertyMapper.IncrementCountClickedForPropertyID(id);
            return View(property);
        }

        // GET: Property/Average
        public ActionResult Average()
        {
            return View(getAveragePrices());
        }

        public String Comparison()
        {
            return "hello";
        }

        [HttpPost]
        public ActionResult Compare(int compare1, int compare2)
        {
            //getting the object for property1
            Property compareProperty1 = propertyMapper.SelectPropertyById(compare1);
            //getting the object for property2
            Property compareProperty2 = propertyMapper.SelectPropertyById(compare2);

            // Increment count for Property1 and Property2
            propertyMapper.IncrementCountClickedForPropertyID(compare1);
            propertyMapper.IncrementCountClickedForPropertyID(compare2);

            //calculating the price per square feet
            var perunit1 = compareProperty1.AskingPrice / compareProperty1.Size;
            var perunit2 = compareProperty2.AskingPrice / compareProperty2.Size;

            //this is to round the value of the per unit size to the nearest 2 decimal place. 
            perunit1 = Math.Round(perunit1, 2);
            perunit2 = Math.Round(perunit2, 2);

            List<Property> objcompareproperty = new List<Property>();
            objcompareproperty.Add(compareProperty1);
            objcompareproperty.Add(compareProperty2);

            ViewBag.property1Perunit = perunit1;
            ViewBag.property2Perunit = perunit2;

            // Unset comparison session variables upon comparison (UNSET? OR DONT UNSET?)
            Session.Remove("compare1");
            Session.Remove("compare2");

            return View(objcompareproperty.ToList());
        }


        // Function to create all dropdown lists for property listing
        private List<Dropdownlist> createDropDownLists()
        {
            List<Dropdownlist> allDropdownlists = new List<Dropdownlist>();

            // Get dropdownlist selected options (saved in Listing POST function)
            string districtOption = (TempData["districtOption"] == null) ? "" : (string)TempData["districtOption"];
            string propertyTypeOption = (TempData["propertyTypeOption"] == null) ? "" : (string)TempData["propertyTypeOption"];
            string listingTypeOption = (TempData["listingTypeOption"] == null) ? "" : (string)TempData["listingTypeOption"];
            string priceOption = (TempData["priceOption"] == null) ? "" : (string)TempData["priceOption"];

            // Create District Dropdownlist
            Dropdownlist districtList = new Dropdownlist();
            districtList.listLabel = "District:";
            districtList.listName = "DistrictDropDownList";
            districtList.listOptions = new List<SelectListItem>();
            districtList.listOptions.Add(new SelectListItem() { Text = defaultListOption, Value = defaultListOption });
            foreach (string district in propertyMapper.GetAllDistrictName())
            {
                districtList.listOptions.Add(
                    new SelectListItem()
                    {
                        Text = district,
                        Value = district,
                        Selected = (districtOption.Equals(district)) ? true : false
                    });
            }
            allDropdownlists.Add(districtList);

            // Create Property Type Dropdownlist
            Dropdownlist propertyTypeList = new Dropdownlist();
            propertyTypeList.listLabel = "Property Type:";
            propertyTypeList.listName = "PropertyTypeDropDownList";
            propertyTypeList.listOptions = new List<SelectListItem>();
            propertyTypeList.listOptions.Add(new SelectListItem() { Text = defaultListOption, Value = defaultListOption });
            foreach (string propertyType in propertyMapper.GetAllPropertyTypes())
            {
                propertyTypeList.listOptions.Add(
                    new SelectListItem()
                    {
                        Text = propertyType,
                        Value = propertyType,
                        Selected = (propertyTypeOption.Equals(propertyType)) ? true : false
                    });
            }
            allDropdownlists.Add(propertyTypeList);

            // Create Listing Type DropdownList
            Dropdownlist listingTypeList = new Dropdownlist();
            listingTypeList.listLabel = "Listing Type:";
            listingTypeList.listName = "ListTypeDropDownList";
            listingTypeList.listOptions = new List<SelectListItem>();
            listingTypeList.listOptions.Add(new SelectListItem() { Text = defaultListOption, Value = defaultListOption });
            foreach (string listingType in propertyMapper.GetAllListingTypes())
            {
                listingTypeList.listOptions.Add(
                    new SelectListItem()
                    {
                        Text = listingType,
                        Value = listingType,
                        Selected = (listingTypeOption.Equals(listingType)) ? true : false
                    });
            }
            allDropdownlists.Add(listingTypeList);

            // Create Price DropdownList
            Dropdownlist priceList = new Dropdownlist();
            priceList.listLabel = "Price:";
            priceList.listName = "PriceDropDownList";
            priceList.listOptions = new List<SelectListItem>();
            int[] priceRange = { 0, 2000, 5000, 100000, 500000, 1000000, 5000000 };
            foreach (int price in priceRange)
            {
                priceList.listOptions.Add(
                    new SelectListItem()
                    {
                        Text = (price == 0) ? defaultListOption : "< " + price,
                        Value = price.ToString(),
                        Selected = (priceOption.Equals(price.ToString())) ? true : false
                    });
            }
            allDropdownlists.Add(priceList);

            return allDropdownlists;
        }

        [HttpPost]
        public void SetSessionCompareAjax(string compare1, string compare2)
        {
            if (String.IsNullOrWhiteSpace(compare1))
            {
                Session.Remove("compare1");
            }
            else
            {
                Session["compare1"] = compare1;
            }

            if (String.IsNullOrWhiteSpace(compare2))
            {
                Session.Remove("compare2");
            }
            else
            {
                Session["compare2"] = compare2;
            }
        }

        /*Function to create AverageViewModels: each criteria with a specific average asking price calculated
        * Calculate average for each district and property type
        * Each will be a AverageViewModel e.g. [Condominium, 1503.15]
        * Then display all AverageViewModels in a page with a table
        */
        private AverageViewModel getAveragePrices()
        {
            //TODO Logic
            //Declare empty list
            Stopwatch myStopWatch = Stopwatch.StartNew();
            List<Average> typeCriteria = new List<Average>();
            List<Average> districtCriteria = new List<Average>();
            IEnumerable<Property> propertiesByCriteria; //To later load list of properties for each different criteria

            //Get list of all properties first
            IEnumerable<Property> fullList = propertyMapper.SelectAll();
            //Get all property types
            IEnumerable<string> allPropertyTypes = propertyMapper.GetAllPropertyTypes();
            //Get all district names
            IEnumerable<string> allDistricts = propertyMapper.GetAllDistrictName();

            //filter the full list by each property type, and then find its average and add as new AverageViewModel
            foreach (string type in allPropertyTypes)
            {
                propertiesByCriteria = fullList.Where(p => p.PropertyType.Equals(type));
                //If there are properties for this criteria  
                if (propertiesByCriteria.Any())
                {
                    double totalSum = 0;
                    int count = 0;
                    //Nested For-loop to get total sum of all properties for this criteria
                    foreach (Property ppt in propertiesByCriteria)
                    {
                        totalSum = totalSum + ppt.AskingPrice;
                        count++;
                    }
                    //Add the criteria and its average as a new AverageViewModel
                    Average avg = new Average();
                    avg.criteriaAverage = totalSum / count;
                    avg.criteriaName = type;
                    typeCriteria.Add(avg);
                    //System.Diagnostics.Debug.WriteLine(avm.criteriaAverage + " - " + avm.criteriaName);
                }
            }
            //foreach district, find its average and add as new AverageViewModel
            foreach (string district in allDistricts)
            {
                propertiesByCriteria = fullList.Where(p => p.DistrictID.Equals(propertyMapper.GetDistrictIdByDistrictName(district).First()));
                if (propertiesByCriteria.Any())
                {
                    double totalSum = 0;
                    int count = 0;
                    //Nested For-loop to get total sum of all properties for this criteria
                    foreach (Property ppt in propertiesByCriteria)
                    {
                        totalSum = totalSum + ppt.AskingPrice;
                        count++;
                    }
                    //Add the criteria and its average as a new AverageViewModel
                    Average avg = new Average();
                    avg.criteriaAverage = totalSum / count;
                    avg.criteriaName = district;
                    districtCriteria.Add(avg);
                    //System.Diagnostics.Debug.Write(avm.criteriaAverage + " - " + avm.criteriaName);
                }
            }
            if (districtCriteria.Any() && typeCriteria.Any())
            {
                //Put the two loaded lists from above into a AverageViewModel for display
                AverageViewModel avm = new AverageViewModel();
                avm.districtAverages = districtCriteria;
                avm.propertyTypesAverages = typeCriteria;
                System.Diagnostics.Debug.WriteLine("Time taken (ms) : " + myStopWatch.ElapsedMilliseconds);
                return avm;
            }
            return null;
        }

    }
}
