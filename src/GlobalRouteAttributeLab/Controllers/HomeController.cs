using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalRouteAttributeLab
{
    public class HomeController : Controller
    {
        // Methods
        [Route("[controller]/[action]")]
        public ActionResult<string> Index01()
        {
            return "Hello World! Index01";
        }

        [Route("test/[controller]/[action]")]
        public ActionResult<string> Index02()
        {
            return "Hello World! Index02";
        }
    }
}
