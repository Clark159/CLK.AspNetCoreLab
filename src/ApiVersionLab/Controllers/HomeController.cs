using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVersionLab
{
    [ApiController]    
    [Route("[controller]/[action]")]    
    public class HomeController : Controller
    {
        // Methods
        [ApiVersion("1.0")]
        public ActionResult<string> GetData()
        {
            return "Hello World! 1.0";
        }

        [ApiVersion("2.0")]
        [ActionName("GetData")]
        public ActionResult<string> GetDataV2()
        {
            return "Hello World! 2.0";
        }
    }
}
