using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RazorClassLibraryLab
{
    public class ResourcesController : Controller
    {
        // Methods
        public ActionResult<ResourcesViewModel> Index()
        {
            // ResourcesViewModel
            var viewModel = new ResourcesViewModel();
            {
                // BaseDirectory
                var baseDirectory = AppContext.BaseDirectory;
                if (Directory.Exists(baseDirectory) == false) throw new InvalidOperationException("baseDirectory=null");

                // EntryAssembly
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
                {
                    // EntryResources
                    viewModel.EntryResources = entryAssembly.GetManifestResourceNames().ToList();
                }

                // ModuleAssembly
                var moduleAssembly = Assembly.LoadFile(Path.Combine(baseDirectory, $"{entryAssembly.GetName().Name}.Module.dll"));
                if (moduleAssembly == null) throw new InvalidOperationException($"{nameof(moduleAssembly)}=null");
                {
                    // ModuleResources
                    viewModel.ModuleResources = moduleAssembly.GetManifestResourceNames().ToList();
                }
            }

            // Return
            return viewModel;
        }


        // Class
        public class ResourcesViewModel
        {
            // Properties
            public List<string> EntryResources { get; set; }

            public List<string> ModuleResources { get; set; }
        }
    }
}