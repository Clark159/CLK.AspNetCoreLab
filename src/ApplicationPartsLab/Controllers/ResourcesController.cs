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

namespace ApplicationPartsLab
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
                
                // ModuleAssembly
                var moduleAssembly = Assembly.LoadFrom(Path.Combine(baseDirectory, $"{entryAssembly.GetName().Name}.Module.dll"));
                if (moduleAssembly == null) throw new InvalidOperationException($"{nameof(moduleAssembly)}=null");
                {
                    // ModuleResources
                    viewModel.ModuleResources = moduleAssembly.GetManifestResourceNames().ToList();
                    
                    // ModuleResourceFiles
                    var moduleResourceFiles = new List<string>();
                    this.FillResourceFiles(new ManifestEmbeddedFileProvider(moduleAssembly), @"\", ref moduleResourceFiles);
                    viewModel.ModuleResourceFiles = moduleResourceFiles;
                }
            }

            // Return
            return viewModel;
        }

        private void FillResourceFiles(IFileProvider fileProvider, string path, ref List<string> resourceFiles)
        {
            #region Contracts

            if (fileProvider == null) throw new ArgumentException(nameof(fileProvider));
            if (string.IsNullOrEmpty(path) == true) throw new ArgumentException(nameof(path));
            if (resourceFiles == null) throw new ArgumentException(nameof(resourceFiles));

            #endregion

            // GetDirectoryContents
            foreach (var content in fileProvider.GetDirectoryContents(path))
            {
                // ContentPath
                var contentPath = Path.Combine(path, content.Name);
                if (string.IsNullOrEmpty(contentPath) == true) throw new ArgumentException(nameof(contentPath));

                // Directory
                if (content.IsDirectory == true) this.FillResourceFiles(fileProvider, contentPath, ref resourceFiles);

                // File
                if (content.IsDirectory == false) resourceFiles.Add(contentPath);
            }
        }


        // Class
        public class ResourcesViewModel
        {
            // Properties
            public List<string> ModuleResources { get; set; }

            public List<string> ModuleResourceFiles { get; set; }
        }
    }
}