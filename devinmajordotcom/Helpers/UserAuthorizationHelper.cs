using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Helpers
{
    public class UserAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Write you authentication logic here . you can use Request headers,cookies ,..etc 
        }

    }

    public class AdminUserAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Write you authentication logic here . you can use Request headers,cookies ,..etc 
        }

    }

}