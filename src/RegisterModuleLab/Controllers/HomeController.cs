using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterModuleLab
{
    public class HomeController : Controller
    {
        // Fields
        private readonly SettingContext _settingContext = null;


        // Constructors
        public HomeController(SettingContext settingContext)
        {
            #region Contracts

            if (settingContext == null) throw new ArgumentException(nameof(settingContext));

            #endregion

            // Default
            _settingContext = settingContext;
        }


        // Methods
        public ActionResult<string> Index()
        {
            // Return
            return _settingContext.Execute();
        }
    }
}
