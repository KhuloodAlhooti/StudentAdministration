using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Text;

namespace StudentAdministration.Controllers
{
    public class ChartsController : Controller
    {
        StudentEntities dbEntity = new StudentEntities();


        public ChartsController()
        {
            dbEntity = new StudentEntities();
            dbEntity.Configuration.LazyLoadingEnabled = false;
        }

        // GET: Charts
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult loadDataSummary()
        {
            try
            {
              
                var totalStudent = dbEntity.Students.ToList().Count();
                var totalClass = dbEntity.Classes.ToList().Count();
                var totalCountries = dbEntity.Countries.ToList().Count();

                var list = new { tStudent = totalStudent, tClass = totalClass, tCountries = totalCountries };

                var response = new { Status = 1, ResponseText = list, Message = "loaded Successfully" };
                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                var response = new { Status = 5, ResponseText = string.Empty, Message = "Error Happened" };

                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CountStudentPerClass()
        {
            try
            {

                var StudentList = dbEntity.Students.Include(s => s.Class).ToList();
                var statistic = StudentList.GroupBy(e => e.Class.ClassName)
              .Select(x => new { ClassName = x.Key, Count = x.Count() }).ToList();

                var response = new { Status = 1, ResponseText = statistic, Message = "loaded Successfully" };
                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new { Status = 5, ResponseText = string.Empty, Message = "Error Happened" };

                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CountStudentPerCountry()
        {
            try
            {

                var StudentList = dbEntity.Students.Include(s => s.Country).ToList();
                var statistic = StudentList.GroupBy(e => e.Country.CounntryName)
              .Select(x => new { CounntryName = x.Key, Count = x.Count() }).ToList();

                var response = new { Status = 1, ResponseText = statistic, Message = "loaded Successfully" };
                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new { Status = 5, ResponseText = string.Empty, Message = "Error Happened" };

                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult StudentAverageAge()
        {
            try
            {
                int CurrentYear = DateTime.Now.Year;
                int theAge;
                DateTime CurrentYearBirthday;

                List<int> Age = new List<int>();
                List<object> result = new List<object>();
                List<studentAges> _studentAges = new List<studentAges>();
                var StudentList = dbEntity.Students.ToList();

                foreach (var item in StudentList)
                {
                    //get difference to know if current year birthday is passed or not;;

                    theAge = CurrentYear - item.DOB.Value.Year;
                    CurrentYearBirthday = item.DOB.Value.AddYears(theAge);

                    Age.Add(CurrentYearBirthday > DateTime.Now ? theAge - 1 : theAge);
                }

                var groupAge = Age.GroupBy(i => i);

                foreach (var item in groupAge)
                {

                    _studentAges.Add( new studentAges {age= item.Key, count= item.Count() } );
                   
                     

                }

                var response = new { Status = 1, ResponseText = _studentAges, Message = "loaded Successfully" };
                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new { Status = 5, ResponseText = string.Empty, Message = "Error Happened" };

                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RegistrationYears()
        {
            try
            {

                var registration = dbEntity.Students
              
              .ToList();

                var statistic = registration.GroupBy(e => e.RegistrationYear)
             
                    .Select(x => new { Year = x.Key, Count = x.Count() }).ToList();


                var response = new { Status = 1, ResponseText = statistic.OrderByDescending(x=>x.Year), Message = "loaded Successfully" };
                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new { Status = 5, ResponseText = string.Empty, Message = "Error Happened" };

                return this.Json(response, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }
        }




        public class studentAges
        {
            public int age { get; set; }
            public int count { get; set; }
           
        }
    }
}