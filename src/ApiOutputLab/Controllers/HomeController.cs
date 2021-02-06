using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOutputLab
{
    public class HomeController : Controller
    {
        // Methods
        public ActionResult<Employee> GetUser()
        {
            // Return
            return new Employee();
        }

        public ActionResult<Employee> GetNull()
        {
            // Return
            return null;
        }


        // Class
        public class Employee
        {
            // Properties
            public int Id { get; set; } = 100;

            public string Name { get; set; } = "Clark";
        }
    }
}
