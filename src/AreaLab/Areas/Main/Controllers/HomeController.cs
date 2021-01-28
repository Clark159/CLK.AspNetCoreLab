using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaLab.Main
{
    [Area("Main")]
    public class HomeController : Controller
    {
        // Methods
        public IActionResult Index()
        {            
            return View();
        }
    }
}
