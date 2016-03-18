using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parkeergarages_Website.Models
{
    public class Garage_info
    {
        public string garage_id { get; set; }
        public string name { get; set; }
        public Decimal latitude { get; set; }
        public Decimal longitude { get; set; }
        public int aantal_plekken { get; set; }
    }
}