using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace devinmajordotcom.Helpers
{
    public class RedirectionHelper
    {

        public static string GetUrl<T>(Expression<Action<T>> action, RequestContext requestContext, RouteValueDictionary values = null) where T : Controller
        {
            UrlHelper urlHelper = new UrlHelper(requestContext);
            RouteValueDictionary routeValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);

            if (values != null)
                foreach (var value in values)
                    routeValues.Add(value.Key, value.Value);

            return urlHelper.RouteUrl(routeValues);
        }

    }
}