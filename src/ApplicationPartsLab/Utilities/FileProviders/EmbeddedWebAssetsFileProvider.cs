using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ApplicationPartsLab
{
    public class EmbeddedWebAssetsFileProvider : IFileProvider
    {
        // Constants
        private static readonly string _contentRoot = @"wwwroot";

        private static readonly string _pathPrefixFormat = @"/_content/{0}/";


        // Fields
        private readonly string _pathPrefix = null;

        private readonly IFileProvider _innerProvider = null;


        // Constructors
        public EmbeddedWebAssetsFileProvider(Assembly assembly)
        {
            #region Contracts

            if (assembly == null) throw new ArgumentException(nameof(assembly));

            #endregion

            // Default
            _pathPrefix = string.Format(_pathPrefixFormat, assembly.GetName().Name);
            _innerProvider = new ManifestEmbeddedFileProvider(assembly, _contentRoot);
        }


        // Methods
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(subpath) == true) throw new ArgumentException(nameof(subpath));

            #endregion

            // ContentPath
            var contentPath = this.GetContentPath(subpath);
            if (string.IsNullOrEmpty(contentPath) == true) return NotFoundDirectoryContents.Singleton;

            // InnerProvider
            return _innerProvider.GetDirectoryContents(contentPath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(subpath) == true) throw new ArgumentException(nameof(subpath));

            #endregion

            // ContentPath
            var contentPath = this.GetContentPath(subpath);
            if (string.IsNullOrEmpty(contentPath) == true) return new NotFoundFileInfo(subpath);

            // InnerProvider
            return _innerProvider.GetFileInfo(contentPath);
        }

        public IChangeToken Watch(string filter)
        {
            #region Contracts

            if (string.IsNullOrEmpty(filter) == true) throw new ArgumentException(nameof(filter));

            #endregion

            // ContentPath
            var contentPath = this.GetContentPath(filter);
            if (string.IsNullOrEmpty(contentPath) == true) return NullChangeToken.Singleton;

            // InnerProvider
            return _innerProvider.Watch(contentPath);
        }


        private string GetContentPath(string subpath)
        {
            #region Contracts

            if (string.IsNullOrEmpty(subpath) == true) throw new ArgumentException(nameof(subpath));

            #endregion

            // Require
            if (subpath.StartsWith(_pathPrefix, StringComparison.OrdinalIgnoreCase) == false) return null;

            // ContentPath
            var contentPath = subpath.Substring(_pathPrefix.Length);
            if (string.IsNullOrEmpty(contentPath) == true) return null;

            // Return
            return contentPath;
        }
    }
}
