using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Parkeergarages.Controllers
{
    public class GarageController : Controller
    {
        //
        // GET: /Garage/Overview/ 
       
        public IActionResult Overview()
        {
            return View();
        }
    
        //
        // GET: /Garage/Details/ 
        
        public IActionResult Details(string garage_id)
        {
            ViewData["Message"] = "Details van garage " + garage_id + "komen hier.";

            return View();
        }
    }
}
