﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AreaLab.Admin
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        // Methods
        public IActionResult Index()
        {
            return View();
        }
    }
}
