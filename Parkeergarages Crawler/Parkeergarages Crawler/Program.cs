using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using MySql.Data.MySqlClient;

namespace Parkeergarages_Crawler
{
    class Program
    {
        // constants
        public const string ALL_GARAGES_URL     = "http://opendata.technolution.nl/opendata/parkingdata/v1";
        public static string LOG_FOLDER         = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Parkeergarages Crawler Logfiles";
        public static string LOG_FILE           = LOG_FOLDER + "\\log.txt";
        public const string DATABASE_DATASOURCE = "localhost";
        public const string DATABASE_PORT       = "3306";
        public const string DATABASE_USERNAME   = "Test";
        public const string DATABASE_PASSWORD   = "Test123";

        // global variables
        public static dynamic garages = null;

        static void Main(string[] args)
        {
            Console.Title = "Parkeergarages Crawler";

            // create log folder
            Directory.CreateDirectory(LOG_FOLDER);

            log("Start Crawler", 's');

            // get garages
            garages = getJsonObjectFromUrl(ALL_GARAGES_URL);
            processGarages();

            // close program
            log("Exit Crawler", 'e');
            log("", '0');
        }

        static dynamic getJsonObjectFromUrl(String url)
        {
            log("retrieving json object from: " + url, '1');

            try
            {
                String json = new WebClient().DownloadString(url);
                dynamic json_object = JsonConvert.DeserializeObject(json);

                log("json object successfully retrieved", '1');
                return json_object;
            }
            catch (Exception e)
            {
                log("failed to get json object", '2');
                //log(e.ToString(), '3');
                return null;
            }
        }

        static void processGarages()
        {
            log("processing garages", '1');

            if (garages != null && garages.Count != 0)
            {
                foreach (var garage in garages["parkingFacilities"])
                {
                    log("processing garage: " + (string)garage["name"], '1');
                    dynamic dynamicData = null;
                    dynamic staticData = null;

                    try
                    {
                        Console.WriteLine((string)garage["name"] );

                        staticData = getJsonObjectFromUrl((string)garage["staticDataUrl"]);
                        dynamicData = getJsonObjectFromUrl( (string)garage["dynamicDataUrl"] );

                        saveStaticGarage(staticData);
                        saveDynamicGarage(dynamicData);
                        log("garage processed: " + (string)garage["name"], '1');
                    }
                    catch (Exception e)
                    {
                        log("failed processing garage: " + (string)garage["name"], '2');
                        log(e.ToString(), '3');
                    }            
                }
            }
            else
            {
                log("no garages in object", '2');
            }
        }

        static void saveStaticGarage(dynamic garage)
        {
            log("saving static garage data", '1');

            if (garage != null)
            {
                try
                {
                    // save static data? run once with insert query, after that update query
                    log("static garage data successfully saved", '1');
                }
                catch (Exception e)
                {
                    log("failed to save static garage data", '2');
                    log(e.ToString(), '3');
                    throw;
                }
            }
            else
            {
                log("no static garage data to save", '2');
            }
        }

        static void saveDynamicGarage(dynamic garage)
        {
            log("dynamic saving garage data", '1');

            if (garage != null)
            {
                try
                {
                    // get data values
                    string identifier = (string)garage["parkingFacilityDynamicInformation"]["identifier"];
                    string description = (string)garage["parkingFacilityDynamicInformation"]["description"];

                    int open = Convert.ToInt32(garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["open"]);
                    int full = Convert.ToInt32(garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["full"]);

                    int parking_capacity = (Int32)garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["parkingCapacity"];
                    int vacant_spaces = (Int32)garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["vacantSpaces"];

                    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
                    dtDateTime = dtDateTime.AddSeconds((double)garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["lastUpdated"]);
                    String lastUpdated = dtDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                    // create query
                    String query = "INSERT INTO `infman01_1`.`garagedata` (`identifier`, `description`, `open`, `full`, `parking capacity`, `vacant spaces`, `last updated`) VALUES ('" + identifier + "', '" + description + "', " + open + ", " + full + ", '" + parking_capacity + "', '" + vacant_spaces + "', '" + lastUpdated + "');";

                    // connect to db
                    String connection = "datasource=" + DATABASE_DATASOURCE + ";port=" + DATABASE_PORT + ";username=" + DATABASE_USERNAME + ";password=" + DATABASE_PASSWORD;
                    MySqlConnection myConnection = new MySqlConnection(connection);
                    MySqlCommand myCommand = new MySqlCommand(query, myConnection);
                    MySqlDataReader myReader;
                    myConnection.Open();
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                    }
                    myConnection.Close();

                    log("dynamic garage data successfully saved", '1');
                }
                catch (Exception e)
                {
                    log("failed to save dynamic garage data", '2');
                    log(e.ToString(), '3');
                    throw;
                }
            }
            else
            {
                log("no dynamic garage data to save", '2');
            }
        }

        /// <summary>
        /// logs message including message type and datetime timestamp
        /// </summary>
        /// <param name="log_message">message to log</param>
        /// <param name="type">type of log message, default null</param>
        static void log(string log_message, Char level)
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
