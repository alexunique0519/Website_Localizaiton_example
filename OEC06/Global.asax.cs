﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OEC06
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //set the resource file name for localization model validation string  
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "YYTranslations";
            DefaultModelBinder.ResourceClassKey = "YYTranslations";
        }


        // On each server request:
        // - Check the language cookie or Session variable 
        // - Set the culture & language if found, leave default if not
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (Request.Cookies["language"] != null)
            {
                string language = Request.Cookies["language"].Value;

                System.Threading.Thread.CurrentThread.CurrentUICulture = new
                    System.Globalization.CultureInfo(language);
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(language);
            }
        }

    }

}
