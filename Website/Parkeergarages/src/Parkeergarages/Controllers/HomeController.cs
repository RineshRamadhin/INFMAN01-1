using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Parkeergarages.Controllers
{
    public class HomeController : Controller
    {
        // 
        // GET: /Home/Error

        public IActionResult Error()
        {
            return View();
        }
    }
}
