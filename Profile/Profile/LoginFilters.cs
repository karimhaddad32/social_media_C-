using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Profile.App_Start
{
    public class LoginFilters : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["user_id"] == null)
            {
                var routevalues = new System.Web.Routing.RouteValueDictionary();
                routevalues.Add("controller", "Login");
                routevalues.Add("action", "Index");

                filterContext.Result = new RedirectToRouteResult("Default", routevalues);
            }
        }

    }
}