using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationLab
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ModuleAttribute : ApiControllerAttribute, IRouteValueProvider, IRouteTemplateProvider
    {
        // Fields
        private readonly AreaAttribute _areaAttribute = null;

        private readonly RouteAttribute _routeAttribute = null;


        // Constructors
        public ModuleAttribute(string moduleName, string routeTemplate = @"[area]/[controller]/[action]")
        {
            #region Contracts

            if (string.IsNullOrEmpty(moduleName) == true) throw new ArgumentException(nameof(moduleName));

            #endregion

            // Default
            _areaAttribute = new AreaAttribute(moduleName);
            _routeAttribute = new RouteAttribute(routeTemplate);
        }


        // AreaAttribute
        string IRouteValueProvider.RouteKey => _areaAttribute.RouteKey;

        string IRouteValueProvider.RouteValue => _areaAttribute.RouteValue;

        // RouteAttribute
        string IRouteTemplateProvider.Template => _routeAttribute.Template;

        int? IRouteTemplateProvider.Order => _routeAttribute.Order;

        string IRouteTemplateProvider.Name => _routeAttribute.Name;
    }
}
