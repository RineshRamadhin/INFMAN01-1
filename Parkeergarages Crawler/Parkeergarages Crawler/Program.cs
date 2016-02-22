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
        public const string ALL_GARAGES_URL = "http://opendata.technolution.nl/opendata/parkingdata/v1";
        public static string LOG_FILE       = Directory.GetCurrentDirectory() + "\\log.txt";
        public const string DATABASE_NAME   = "";

        // global variables
        public static dynamic garages = null;

        static void Main(string[] args)
        {
            log("Start Crawler", "START  ");

            // get garages
            garages = getJsonObjectFromUrl(ALL_GARAGES_URL);
            processGarages();

            //Tijdelijke code
            //Console.WriteLine("Press a key to finish...");
            //Console.ReadLine();

            // close program
            log("Exit Crawler", "EXIT   ");
            log("", "NEW_LINE");
        }

        static dynamic getJsonObjectFromUrl(String url)
        {
            log("retrieving json object from: " + url, null);

            try
            {
                String json = new WebClient().DownloadString(url);
                dynamic json_object = JsonConvert.DeserializeObject(json);

                log("json object successfully retrieved", null);
                return json_object;
            }
            catch (Exception e)
            {
                log("failed to get json object", "FAILED ");
                //log(e.ToString(), "ERROR  ");
                return null;
            }
        }

        static void processGarages()
        {
            log("processing garages", null);

            if (garages != null && garages.Count != 0)
            {
                foreach (var garage in garages["parkingFacilities"])
                {
                    log("processing garage: " + (string)garage["name"], null);
                    dynamic dynamicData = null;
                    dynamic staticData = null;

                    try
                    {
                        Console.WriteLine((string)garage["name"] );
  
                        dynamicData = getJsonObjectFromUrl( (string)garage["dynamicDataUrl"] );
                        staticData = getJsonObjectFromUrl((string)garage["staticDataUrl"]);

                        saveGarage(dynamicData);
                        log("garage processed: " + (string)garage["name"], null);
                    }
                    catch (Exception e)
                    {
                        log("failed processing garage: " + (string)garage["name"], "FAILED ");
                        log(e.ToString(), "ERROR  ");
                    }            
                }
            }
            else
            {
                log("no garages in object", "FAILED ");
            }
        }

        static void saveGarage(dynamic garage)
        {
            log("saving garage data", null);

            if (garage != null)
            {          
                try
                {
                    // TODO: save garage data to db here
                    Console.WriteLine("identifier " + (string)garage["parkingFacilityDynamicInformation"]["identifier"]);
                    Console.WriteLine("description " + (string)garage["parkingFacilityDynamicInformation"]["description"]);
                    Console.WriteLine("open " + (string)garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["open"]);
                    Console.WriteLine("full " + (string)garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["full"]);
                    Console.WriteLine("parking capacity " + (string)garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["parkingCapacity"]);
                    Console.WriteLine("vacant spaces " + (string)garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["vacantSpaces"]);
                    Console.WriteLine("last updated " + (string)garage["parkingFacilityDynamicInformation"]["facilityActualStatus"]["lastUpdated"]);
                    Console.WriteLine("");

                    log("garage data successfully saved", null);
                }
                catch (Exception e)
                {
                    log("failed to save garage data", "FAILED ");
                    log(e.ToString(), "ERROR  ");
                    throw;
                }
            }
            else
            {
                log("no garage data to save", "FAILED ");
            }
        }


        /// <summary>
        /// logs message including message type and datetime timestamp
        /// </summary>
        /// <param name="log_message">message to log</param>
        /// <param name="type">type of log message, default null</param>
        static void log(string log_message, String type)
        {
            if (type == null || type.Length == 0)
            {
                type = "MESSAGE";
            }

            if (type == "NEW_LINE")
            {
                File.AppendAllText(LOG_FILE, Environment.NewLine);
            }
            else
            {
                String now = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
                String message = now + " - " + type + " - " + log_message + Environment.NewLine;
                File.AppendAllText(LOG_FILE, message);
            }
        }

    }
}
