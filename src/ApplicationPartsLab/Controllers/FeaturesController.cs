using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApplicationPartsLab
{
    public class FeaturesController : Controller
    {
        // Fields
        private readonly ApplicationPartManager _applicationPartManager = null;


        // Constructors
        public FeaturesController(ApplicationPartManager applicationPartManager)
        {
            #region Contracts

            if (applicationPartManager == null) throw new ArgumentException(nameof(applicationPartManager));

            #endregion

            // Default
            _applicationPartManager = applicationPartManager;
        }


        // Methods
        public ActionResult<FeaturesViewModel> Index()
        {
            // FeaturesViewModel
            var viewModel = new FeaturesViewModel();
            {
                // ControllerFeature
                var controllerFeature = new ControllerFeature();
                _applicationPartManager.PopulateFeature(controllerFeature);
                viewModel.Controllers = controllerFeature.Controllers.Select(x => x.FullName).ToList();

                // TagHelperFeature
                var tagHelperFeature = new TagHelperFeature();
                _applicationPartManager.PopulateFeature(tagHelperFeature);
                viewModel.TagHelpers = tagHelperFeature.TagHelpers.Select(x => x.FullName).ToList();

                // ViewComponentFeature
                var viewComponentFeature = new ViewComponentFeature();
                _applicationPartManager.PopulateFeature(viewComponentFeature);
                viewModel.ViewComponents = viewComponentFeature.ViewComponents.Select(x=>x.FullName).ToList();
            }

            // Return
            return viewModel;
        }


        // Class
        public class FeaturesViewModel
        {
            // Properties
            public List<string> Controllers { get; set; }

            public List<string> TagHelpers { get; set; }

            public List<string> ViewComponents { get; set; }
        }
    }
}