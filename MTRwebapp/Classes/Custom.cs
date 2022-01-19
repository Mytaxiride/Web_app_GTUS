using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static MTRwebapp.Models.CustomModels;

namespace MTRwebapp.Classes
{
    public class Custom
    {

        public static List<Data> FetchDataList(string username, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connectionString);
            List<Data> returnval = new List<Data>();
            try
            {
                string searchsql = "SELECT us.usr_id , us.usr_name, us.usr_password, us.usr_email FROM taxi_user us WHERE us.usr_name = " + "'" + username + "'" + " AND us.usr_password = " + "'" + password + "'";
                MySqlCommand cmd = new MySqlCommand(searchsql, conn);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    returnval.Add(new Data { id = rdr["usr_id"].ToString(), name = rdr["usr_name"].ToString(), email = rdr["usr_email"].ToString(), password = rdr["usr_password"].ToString() });
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return returnval;
        }

        public static JobDetail FetchJobDetail(string jobid)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connectionString);
            JobDetail returnval = new JobDetail();
            try
            {
                string searchsql = "SELECT td.mtm_num, td.customerpickuptime, td.dropoftime, td.driver_number, td.pick_up_latitude,  td.dropoflat, td.dropoflang, td.pick_up_longitude,td.AssignedTo  FROM taxi_driverrequest td  WHERE td.mtm_num = " + "'" + jobid + "'";
                MySqlCommand cmd = new MySqlCommand(searchsql, conn);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    returnval = (new JobDetail
                    {
                        jobid = rdr["mtm_num"].ToString(),
                        dtstarttime = Convert.ToDateTime(rdr["customerpickuptime"]),
                        dtendtime = Convert.ToDateTime(rdr["dropoftime"]),
                        driverid = rdr["driver_number"].ToString(),
                        pickuplat = rdr["pick_up_latitude"].ToString(),
                        pickuplng = rdr["pick_up_longitude"].ToString(),
                        droplat = rdr["dropoflat"].ToString(),
                        droplng = rdr["dropoflang"].ToString(),
                        did = rdr["AssignedTo"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return returnval;
        }

        public static JobDetail FetchTimeBasedDetail(string driverno)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connectionString);
            //JobDetail returnval = new JobDetail();
            JobDetail returnval = new JobDetail();
            try
            {
                string searchsql = "SELECT td.taxi_driver_info_id,td.driver_number FROM taxi_driver_info td  WHERE td.domainid=33 and td.driver_number = " + "'" + driverno + "'";
                MySqlCommand cmd = new MySqlCommand(searchsql, conn);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    returnval = new JobDetail
                    {
                        //jobid = rdr["mtm_num"].ToString(),
                        //dtstarttime = Convert.ToDateTime(rdr["customerpickuptime"]),
                        //dtendtime = Convert.ToDateTime(rdr["dropoftime"]),
                        driverid = rdr["driver_number"].ToString(),
                        //pickuplat = rdr["pick_up_latitude"].ToString(),
                        //pickuplng = rdr["pick_up_longitude"].ToString(),
                        //droplat = rdr["dropoflat"].ToString(),
                        //droplng = rdr["dropoflang"].ToString(),
                        did = rdr["taxi_driver_info_id"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return returnval;
        }

        public static List<DriverData> DriverList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connectionString);
            List<DriverData> returnval = new List<DriverData>();
            try
            {
                string searchsql = "SELECT us.usr_id , us.usr_name FROM taxi_user us";
                MySqlCommand cmd = new MySqlCommand(searchsql, conn);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    returnval.Add(new DriverData { id = rdr["usr_id"].ToString(), name = rdr["usr_name"].ToString() });
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return returnval;
        }


        public static string EncMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(password);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            password = BitConverter.ToString(encodedBytes).Replace("-", "");
            var result = password.ToLower();
            return result;
        }


    }
}