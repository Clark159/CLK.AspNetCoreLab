﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationLab
{
    [Module("CLK-Lab-Module01")]
    public class HomeController : Controller
    {
        // Methods
        public ActionResult<string> Index()
        {
            return View();
        }
    }
}