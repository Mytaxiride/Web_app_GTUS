using Amazon.DynamoDBv2;
using MTRwebapp.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using static MTRwebapp.Models.CustomModels;

namespace MTRwebapp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string pWord = Custom.EncMD5(password);
            List<Data> InList = Custom.FetchDataList(username, pWord);
            if (InList.Count != 0)
            {
                Session["Username"] = InList[0].name;
                TempData["Username"] = InList[0].name;
                return RedirectToAction("Dashboard");
            }
            else
                TempData["SnackMessage"] = "Invalid username or password";
            return RedirectToAction("Login");
        }

        public ActionResult Dashboard()
        {

            List<DriverLocation> Driver = new List<DriverLocation>();
            DriverLocation data = new DriverLocation();
            data.lat = "40.8446226";
            data.lng = "-73.9085876";
            data.createdon = "1561072664067";
            Driver.Add(data);

            data = new DriverLocation();
            data.lat = "40.8446167";
            data.lng = "-73.9085815";
            data.createdon = "1561072754066";
            Driver.Add(data);

            data = new DriverLocation();
            data.lat = "40.8425217";
            data.lng = "-73.9179651";
            data.createdon = "1561075462102";
            Driver.Add(data);

            TempData["Track"] = Driver;


            // AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIA5VC2S63ELJLWW77Y", "6YDUjsBE3Bnv+QfScwfDpnOpnpf3BHqs9denlx/b", Amazon.RegionEndpoint.USEast1);
            // Table driver_location_history = Table.LoadTable(client, "driver_location_history");
            // var track = Dynamo.FindDriverLocationBetween(driver_location_history, 13233, "1560786481335", "1560786851375");
            //TempData["Track"] = track;
            return View();
        }

        [HttpPost]
        public ActionResult JobDetail(string Jobid)
        {
            JobDetail InList = Custom.FetchJobDetail(Jobid);
            InList.starttime = InList.dtstarttime.ToString("MM/dd/yyyy h:mm tt");
            InList.endtime = InList.dtendtime.ToString("MM/dd/yyyy h:mm tt");
            InList.timetaken = Math.Round(InList.dtendtime.Subtract(InList.dtstarttime).TotalMinutes, 0) + " Mins";
            int driverid = Convert.ToInt32(InList.did);
            AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIA5VC2S63ELJLWW77Y", "6YDUjsBE3Bnv+QfScwfDpnOpnpf3BHqs9denlx/b", Amazon.RegionEndpoint.USEast1);
            //Table driver_location_history = Table.LoadTable(client, "driver_location_history");
            List<DriverLocation> track = Dynamo.FindDriverLocationBetween(driverid, ConvertToTimestamp(InList.dtstarttime), ConvertToTimestamp(InList.dtendtime));
            //List<DriverLocation> Driver = new List<DriverLocation>();
            //DriverLocation data = new DriverLocation();
            //data.lat = "40.8446226";
            //data.lng = "-73.9085876";
            //data.createdon = "1561072664067";
            //Driver.Add(data);

            //data = new DriverLocation();
            //data.lat = "40.8446167";
            //data.lng = "-73.9085815";
            //data.createdon = "1561072754066";
            //Driver.Add(data);

            //data = new DriverLocation();
            //data.lat = "40.8425217";
            //data.lng = "-73.9179651";
            //data.createdon = "1561075462102";
            //Driver.Add(data);

            TempData["Track"] = track;


            return Json(new { InList, Track = track, Status = "0" });
            //return Json(InList);
        }

        [HttpPost]
        public ActionResult TimeBasedDetail(string driverno, string timefrom, string timeto)
        {

            JobDetail InList = Custom.FetchTimeBasedDetail(driverno);
            //DateTime start = DateTime.Parse(timefrom);
            // DateTime start = DateTime.ParseExact(timefrom, "MM/dd/yy h:mm tt", CultureInfo.InvariantCulture);
            DateTime starttime = Convert.ToDateTime(timefrom);
            DateTime stoptime = Convert.ToDateTime(timeto);
            InList.dtstarttime = starttime;
            InList.dtendtime = stoptime;
            InList.starttime = starttime.ToString("MM/dd/yyyy h:mm tt");
            InList.endtime = stoptime.ToString("MM/dd/yyyy h:mm tt");
            InList.timetaken = Math.Round(InList.dtendtime.Subtract(InList.dtstarttime).TotalMinutes, 0) + " Mins";
            int driverid = Convert.ToInt32(InList.did);
            AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIA5VC2S63ELJLWW77Y", "6YDUjsBE3Bnv+QfScwfDpnOpnpf3BHqs9denlx/b", Amazon.RegionEndpoint.USEast1);
            // Table driver_location_history = Table.LoadTable(client, "driver_location_history");
            List<DriverLocation> track = Dynamo.FindDriverLocationBetween(driverid, ConvertToTimestamp(starttime), ConvertToTimestamp(stoptime));
            return Json(new { Track = track, Status = "1" });

        }

        //[HttpPost]
        public ActionResult Track(string Jobid, string driverno, string timefrom, string timeto, int driverid = 0)
        {
            if (driverno != null && timefrom != null && timeto != null && Jobid == null)
            {
                DateTime starttime = Convert.ToDateTime(timefrom);
                DateTime stoptime = Convert.ToDateTime(timeto);
                JobDetail InList = new JobDetail();
                if (driverid == 0)
                {
                    InList = Custom.FetchTimeBasedDetail(driverno);

                    //InList.dtstarttime = starttime;
                    //InList.dtendtime = stoptime;
                    //InList.starttime = starttime.ToString("MM/dd/yyyy h:mm tt");
                    //InList.endtime = stoptime.ToString("MM/dd/yyyy h:mm tt");
                    //InList.timetaken = Math.Round(InList.dtendtime.Subtract(InList.dtstarttime).TotalMinutes, 0) + " Mins";
                    driverid = Convert.ToInt32(InList.did);
                }
                AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIA5VC2S63ELJLWW77Y", "6YDUjsBE3Bnv+QfScwfDpnOpnpf3BHqs9denlx/b", Amazon.RegionEndpoint.USEast1);

                List<DriverLocation> track = Dynamo.FindDriverLocationBetween(driverid, ConvertToTimestamp(starttime), ConvertToTimestamp(stoptime));
                //return Json(new { Track = track, Status = "1" });
                TempData["Track"] = JsonConvert.SerializeObject(track); //Json(track);
                
                InList.driverid = driverno;
                InList.did = driverid.ToString();
                InList.dtendtime = stoptime;
                InList.dtstarttime = starttime;
                InList.starttime = InList.dtstarttime.ToString("MM/dd/yyyy h:mm tt");
                InList.endtime = InList.dtendtime.ToString("MM/dd/yyyy h:mm tt");
                InList.timetaken = (Math.Round(InList.dtendtime.Subtract(InList.dtstarttime).TotalMilliseconds, 0)).ToString();
                if (track != null && track.Count > 0)
                {
                    InList.pickuplat = track[0].lat;
                    InList.pickuplng = track[0].lng;

                    InList.droplat = track[track.Count-1].lat;
                    InList.droplng = track[track.Count - 1].lng;
                }
                return View(InList);
            }
            else if (Jobid != null && driverno == null && timefrom == null && timeto == null)
            {
                JobDetail InList = Custom.FetchJobDetail(Jobid);
                InList.starttime = InList.dtstarttime.ToString("MM/dd/yyyy h:mm tt");
                InList.endtime = InList.dtendtime.ToString("MM/dd/yyyy h:mm tt");
                //InList.timetaken = Math.Round(InList.dtendtime.Subtract(InList.dtstarttime).TotalMinutes, 0) + " Mins";
                InList.timetaken = (Math.Round(InList.dtendtime.Subtract(InList.dtstarttime).TotalMilliseconds, 0)).ToString();
                driverid = Convert.ToInt32(InList.did);
                AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIA5VC2S63ELJLWW77Y", "6YDUjsBE3Bnv+QfScwfDpnOpnpf3BHqs9denlx/b", Amazon.RegionEndpoint.USEast1);

                List<DriverLocation> track = Dynamo.FindDriverLocationBetween(driverid, ConvertToTimestamp(InList.dtstarttime), ConvertToTimestamp(InList.dtendtime));
                //string json = new JavaScriptSerializer().Serialize(track);

                //dynamic Djson = JsonConvert.DeserializeObject<dynamic>(json);
                TempData["Track"] = JsonConvert.SerializeObject(track); //Json(track);

                return View(InList);
            }
            else
            {
                TempData["SnackMessage"] = " Strings are null";
                return RedirectToAction("Login");
            }
        }

        //public ActionResult Track(string driverno, string timefrom, string timeto)
        //{
        //    if (driverno != null && timefrom != null && timeto != null)
        //    {
        //        JobDetail InList = Custom.FetchTimeBasedDetail(driverno);                
        //        DateTime starttime = Convert.ToDateTime(timefrom);
        //        DateTime stoptime = Convert.ToDateTime(timeto);
        //        //InList.dtstarttime = starttime;
        //        //InList.dtendtime = stoptime;
        //        //InList.starttime = starttime.ToString("MM/dd/yyyy h:mm tt");
        //        //InList.endtime = stoptime.ToString("MM/dd/yyyy h:mm tt");
        //        //InList.timetaken = Math.Round(InList.dtendtime.Subtract(InList.dtstarttime).TotalMinutes, 0) + " Mins";
        //        int driverid = Convert.ToInt32(InList.did);
        //        AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIA5VC2S63ELJLWW77Y", "6YDUjsBE3Bnv+QfScwfDpnOpnpf3BHqs9denlx/b", Amazon.RegionEndpoint.USEast1);

        //        List<DriverLocation> track = Dynamo.FindDriverLocationBetween(driverid, ConvertToTimestamp(starttime), ConvertToTimestamp(stoptime));
        //        //return Json(new { Track = track, Status = "1" });
        //        TempData["Track"] = JsonConvert.SerializeObject(track); //Json(track);
        //        return View();
        //    }
        //    else
        //    {
        //        TempData["SnackMessage"] = " Strings are null";
        //        return RedirectToAction("Login");
        //    }
        //}

        private string ConvertToTimestamp(DateTime value)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime UtcTime = TimeZoneInfo.ConvertTimeToUtc(value, easternZone);

            long epoch = (UtcTime.Ticks - 621355968000000000) / 10000;
            return epoch.ToString();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

    }
}