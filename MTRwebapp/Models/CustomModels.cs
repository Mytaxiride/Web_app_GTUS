using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTRwebapp.Models
{
    public class CustomModels
    {

        public class Data
        {
            public string id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public DateTime createdon { get; set; }
        }

        public class DriverData
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class JobDetail
        {

            public string jobid { get; set; }
            public DateTime dtstarttime { get; set; }
            public DateTime dtendtime { get; set; }
            public string starttime { get; set; }
            public string endtime { get; set; }
            public string driverid { get; set; }
            public string did { get; set; }
            public string pickuplat { get; set; }
            public string pickuplng { get; set; }
            public string droplat { get; set; }
            public string droplng { get; set; }
            public string timetaken { get; set; }

        }

        public class DriverLocation
        {
            public string lat { get; set; }
            public string lng { get; set; }
            public string createdon { get; set; }
        }
    }
}