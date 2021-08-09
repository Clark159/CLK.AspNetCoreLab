using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationPermissionLab
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
        [OperationPermissionFilter("Home.Add")]
        public ActionResult Add()
        {
            // Return
            return View();
        }

        [Authorize]
        [OperationPermissionFilter("Home.Remove")]
        public ActionResult Remove()
        {
            // Return
            return View();
        }

        [Authorize]
        [OperationPermissionFilter("Home.Update")]
        public ActionResult Update()
        {
            // Return
            return View();
        }
    }
}
