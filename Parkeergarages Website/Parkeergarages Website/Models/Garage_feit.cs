using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parkeergarages_Website.Models
{
    public class Garage_feit
    {
        public string garage_id { get; set; }
        public DateTime datum { get; set; }
        public Boolean open { get; set; }
        public Boolean full { get; set; }
        public int vrije_plekken { get; set; }
    }
}