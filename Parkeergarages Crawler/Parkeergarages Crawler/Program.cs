using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace Parkeergarages_Crawler
{
    class Program
    {
        // constants
        public const string ALL_GARAGES_URL     = "http://opendata.technolution.nl/opendata/parkingdata/v1";
        public static string LOG_FOLDER         = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Parkeergarages Crawler Logfiles";
        public static string LOG_FILE           = LOG_FOLDER + "\\Log.txt";
        public const string DATABASE_DATASOURCE = "localhost";
        public const string DATABASE_PORT       = "3306";
        public const string DATABASE_NAME       = "garages";
        public const string DATABASE_USERNAME   = "Inserter";
        public const string DATABASE_PASSWORD   = "Inserter123";

        // global variables
        public static dynamic garages = null;
        public static MySqlConnection connection = new MySqlConnection("SERVER=" + DATABASE_DATASOURCE + ";" + "DATABASE=" + DATABASE_NAME + ";" + "UID=" + DATABASE_USERNAME + ";" + "PASSWORD=" + DATABASE_PASSWORD + ";");

        static void Main(string[] args)
        {
            Console.Title = "Parkeergarages Crawler";

            // create Log folder
            Directory.CreateDirectory(LOG_FOLDER);

            Log("Start Crawler", 's');

            // open database connection
            if (OpenConnection() == true)
            {
                // get and process garages
                garages = GetJsonObjectFromUrl(ALL_GARAGES_URL);
                ProcessGarages();

                // close the database connection
                CloseConnection();
            }
            else
            {
                Log("Database Error: Failed to open the connection", '3');
            }

            // close program
            Log("Exit Crawler", 'e');
            Log("", '0');
        }

        /// <summary>
        /// gets json object from url
        /// </summary>
        /// <param name="url">source url</param>
        /// <returns>json object</returns>
        static dynamic GetJsonObjectFromUrl(String url)
        {
            Log("retrieving json object from: " + url, '1');

            try
            {
                String json = new WebClient().DownloadString(url);
                dynamic json_object = JsonConvert.DeserializeObject(json);

                Log("json object successfully retrieved", '1');
                return json_object;
            }
            catch (Exception e)
            {
                Log("failed to get json object", '2');
                //Log(e.ToString(), '3');
                return null;
            }
        }

        /// <summary>
        /// loops through all garages and calls ProcessGarage function for each garage
        /// </summary>
        static void ProcessGarages()
        {
            Log("processing garages", '1');

            if (garages != null && garages.Count != 0)
            {
                foreach (var garage in garages["parkingFacilities"])
                {
                    Log("processing garage: " + (string)garage["name"], '1');
                    try
                    {
                        Console.WriteLine((string)garage["name"] );

                        dynamic static_data = GetJsonObjectFromUrl((string)garage["staticDataUrl"]);
                        dynamic dynamic_data = GetJsonObjectFromUrl( (string)garage["dynamicDataUrl"] );

                        ProcessGarage(static_data, dynamic_data);

                        Log("garage processed: " + (string)garage["name"], '1');
                    }
                    catch (Exception e)
                    {
                        Log("failed processing garage: " + (string)garage["name"], '2');
                        Log(e.ToString(), '3');
                    }            
                }
            }
            else
            {
                Log("no garages in object", '2');
            }
        }

        /// <summary>
        /// Gets and saves values from a single garage object
        /// </summary>
        /// <param name="static_data">static garage data</param>
        /// <param name="dynamic_data">dynamic garage data</param>
        static void ProcessGarage(dynamic static_data, dynamic dynamic_data)
        {
            if (static_data != null && dynamic_data != null)
            {
                try
                {
                    // garage info
                    String name             = static_data["parkingFacilityStaticInformation"]["name"];
                    String info_id          = static_data["parkingFacilityStaticInformation"]["identifier"];
                    String latitude         = static_data["parkingFacilityStaticInformation"]["locationForDisplay"]["latitude"];
                    String longitude        = static_data["parkingFacilityStaticInformation"]["locationForDisplay"]["longitude"];
                    String aantal_plekken   = dynamic_data["parkingFacilityDynamicInformation"]["facilityActualStatus"]["parkingCapacity"];

                    // garage datum
                    DateTime unix_start     = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
                    DateTime garage_date    = unix_start.AddSeconds((double)dynamic_data["parkingFacilityDynamicInformation"]["facilityActualStatus"]["lastUpdated"]);            
                    String datum            = garage_date.ToString("yyyy-MM-dd HH:mm");
                    String dag              = garage_date.DayOfWeek.ToString();
                    String maand            = garage_date.ToString("MMMM", CultureInfo.InvariantCulture);
                    String jaar             = garage_date.Year.ToString();
                    String uur              = garage_date.Hour.ToString();
                    String minuut           = garage_date.Minute.ToString();

                    // garage feiten
                    bool open             = dynamic_data["parkingFacilityDynamicInformation"]["facilityActualStatus"]["open"];
                    bool full             = dynamic_data["parkingFacilityDynamicInformation"]["facilityActualStatus"]["full"];
                    String vrije_plekken    = dynamic_data["parkingFacilityDynamicInformation"]["facilityActualStatus"]["vacantSpaces"];

                    // insert into database
                    //info
                    String query = "INSERT INTO `garages`.`info` (`garage_id`, `name`, `latitude`, `longitude`, `aantal_plekken`) VALUES ('" + info_id + "', '" + name + "', '" + latitude + "', '" + longitude + "', '" + aantal_plekken + "');";
                    Insert(query);

                    // datum
                    query = "INSERT INTO `garages`.`datum` (`datum`, `dag`, `maand`, `jaar`, `uur`, `minuut`) VALUES ('" + datum + "', '" + dag + "', '" + maand + "', '" + jaar + "', '" + uur + "', '" + minuut + "');";
                    Insert(query);

                    // feit
                    query = "INSERT INTO `garages`.`feit` (`garage_id`, `datum`, `open`, `full`, `vrije_plekken`) VALUES ('" + info_id + "', '" + datum + "', '" + Convert.ToInt32(open) + "', '" + Convert.ToInt32(full) + "', '" + vrije_plekken + "');";
                    Insert(query);
                }
                catch (Exception e)
                {
                    Log("failed to save properties from garage object to database", '2');
                    Log(e.ToString(), '3');                   
                }
            }
            else
            {
                Log("no static and/or dynamic garage data to save", '2');
            }

        }

        /// <summary>
        /// Opens connection with the database
        /// </summary>
        /// <returns>Boolean succesful connection</returns>
        static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        Log("Database Error: Cannot connect to server", '3');
                        break;

                    case 1045:
                        Log("Database Error: Invalid username/password", '3');
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// Closes the connection with the database
        /// </summary>
        /// <returns></returns>
        static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Log("Database Error: Failed to close the connection", '3');
                Log(e.ToString(), '3');
                return false;
            }
        }

        /// <summary>
        /// performs insert query into database
        /// </summary>
        /// <param name="query"></param>
        static void Insert(String query)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log("Database Error: Failed to perform the INSERT query", '3');
                Log(e.ToString(), '3');
            }
        }

        /// <summary>
        /// logs message including message type and datetime timestamp
        /// </summary>
        /// <param name="log_message">message to log</param>
        /// <param name="type">type of log message, default null</param>
        static void Log(string log_message, Char level)
        {
            String type = "MESSAGE";

            switch (level)
            {
                case '0':
                    File.AppendAllText(LOG_FILE, Environment.NewLine);
                    return;
                case '1':
                    type = "MESSAGE";
                    break;
                case '2':
                    type = "FAILED ";
                    break;
                case '3':
                    type = "ERROR  ";
                    break;
                case 's':
                    type = "START  ";
                    break;
                case 'e':
                    type = "EXIT   ";
                    break;

            }

            String now = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
            String message = now + " - " + type + " - " + log_message + Environment.NewLine;
            File.AppendAllText(LOG_FILE, message);
        }
    }
}
