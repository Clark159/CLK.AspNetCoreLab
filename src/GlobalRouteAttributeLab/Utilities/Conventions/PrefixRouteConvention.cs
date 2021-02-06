using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalRouteAttributeLab
{
    public class PrefixRouteConvention : IApplicationModelConvention
    {
        // Fields
        private readonly string _prefixRoute = null;


        // Constructors
        public PrefixRouteConvention(string prefixRoute)
        {
            #region Contracts

            if (string.IsNullOrEmpty(prefixRoute) == true) throw new ArgumentException(nameof(prefixRoute));

            #endregion

            // Default
            _prefixRoute = prefixRoute;
        }


        // Methods
        public void Apply(ApplicationModel application)
        {
            #region Contracts

            if (application == null) throw new ArgumentException(nameof(application));

            #endregion

            // PrefixRouteModel
            var prefixRouteModel = new AttributeRouteModel(new RouteAttribute(_prefixRoute));

            // Controllers
            foreach (var controller in application.Controllers)
            {
                // Filter
                // ...

                // RouteSelector
                var routeSelector = controller.Selectors.FirstOrDefault(x => x.AttributeRouteModel != null);
                if (routeSelector == null)
                {
                    // Attach
                    controller.Selectors.Add(new SelectorModel() { AttributeRouteModel = prefixRouteModel });
                }
                else
                {
                    // Combine
                    routeSelector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(prefixRouteModel, routeSelector.AttributeRouteModel);
                }
            }
        }
    }
}