using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace IntegrationLab
{
    public class ModuleWebAssetsFileProvider : IFileProvider
    {
        // Constants
        private static readonly string _contentRoot = @"wwwroot";


        // Fields
        private readonly IFileProvider _innerProvider = null;


        // Constructors
        public ModuleWebAssetsFileProvider(Assembly assembly)
        {
            #region Contracts

            if (assembly == null) throw new ArgumentException(nameof(assembly));

            #endregion

            // Default
            _innerProvider = new ManifestEmbeddedFileProvider(assembly, _contentRoot);
        }


        // Methods
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(subpath) == true) throw new ArgumentException(nameof(subpath));

            #endregion

            // InnerProvider
            return _innerProvider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(subpath) == true) throw new ArgumentException(nameof(subpath));

            #endregion

            // InnerProvider
            return _innerProvider.GetFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            #region Contracts

            if (string.IsNullOrEmpty(filter) == true) throw new ArgumentException(nameof(filter));

            #endregion

            // InnerProvider
            return _innerProvider.Watch(filter);
        }
    }
}
