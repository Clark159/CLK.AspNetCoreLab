using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PathPermissionLab
{
    public class HomeController : Controller
    {
        // Methods
        [Authorize]
        public ActionResult Index()
        {
            // Return
            return View();
        }

        [Authorize]
        public ActionResult Add()
        {
            // Return
            return View();
        }

        [Authorize]
        public ActionResult Remove()
        {
            // Return
            return View();
        }

        [Authorize]
        public ActionResult Update()
        {
            // Return
            return View();
        }
    }
}
