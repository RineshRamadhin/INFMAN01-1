using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;
using Parkeergarages_Website.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace Parkeergarages_Website.Controllers
{
    public class GarageController : Controller
    {
        // constants
        public const string DATABASE_DATASOURCE = "localhost";
        public const string DATABASE_PORT       = "3306";
        public const string DATABASE_NAME       = "garages";
        public const string DATABASE_USERNAME   = "Dev";
        public const string DATABASE_PASSWORD   = "Dev123";

        // global variables
        public static MySqlConnection connection = new MySqlConnection("SERVER=" + DATABASE_DATASOURCE + ";" + "DATABASE=" + DATABASE_NAME + ";" + "UID=" + DATABASE_USERNAME + ";" + "PASSWORD=" + DATABASE_PASSWORD + ";");

        // GET: Garage/Overview
        public ActionResult Overview()
        {
            var garages = JsonConvert.SerializeObject( Garages() );
            ViewBag.garages = garages;

            return View();
        }

        // GET: Garage/Details?garage_id=
        public ActionResult Details(string garage_id)
        {
            Garage_info garage = Garage(garage_id);
            ViewBag.garage_name = garage.name;
            ViewBag.garage_info = JsonConvert.SerializeObject (garage);

            return View();
        }

        // GET: Garage/Test
        public String Test()
        {
            var garages = JsonConvert.SerializeObject( Garages() );

            return garages;
        }

        static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                connection.Close();
                return false;
            }
        }

        static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                return false;
            }
        }

        private static List<Garage_info> Garages()
        {
            string query = "SELECT * FROM garages.info";

            //Create a list to store the result
            List<Garage_info> garages = new List<Garage_info>();

            //Open connection
            if ( OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Garage_info garage      = new Garage_info();

                    garage.garage_id        = (string) dataReader["garage_id"];
                    garage.name             = (string) dataReader["name"];
                    garage.latitude         = decimal.Parse( dataReader.GetMySqlDecimal("longitude").ToString(), CultureInfo.InvariantCulture);
                    garage.longitude        = decimal.Parse( dataReader.GetMySqlDecimal("latitude").ToString(), CultureInfo.InvariantCulture);
                    garage.aantal_plekken   = (int) dataReader["aantal_plekken"];

                    garages.Add(garage);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return garages;
            }
            else
            {
                return garages;
            }
        }

        private static Garage_info Garage(string garage_id)
        {
            string query = "SELECT * FROM garages.info WHERE garages.info.garage_id = '" + garage_id  + "'";

            //Create a list to store the result
            Garage_info garage = new Garage_info();

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    garage.garage_id = (string)dataReader["garage_id"];
                    garage.name = (string)dataReader["name"];
                    garage.latitude = decimal.Parse(dataReader.GetMySqlDecimal("longitude").ToString(), CultureInfo.InvariantCulture);
                    garage.longitude = decimal.Parse(dataReader.GetMySqlDecimal("latitude").ToString(), CultureInfo.InvariantCulture);
                    garage.aantal_plekken = (int)dataReader["aantal_plekken"];
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return garage;
            }
            else
            {
                return garage;
            }
        }
    }
}