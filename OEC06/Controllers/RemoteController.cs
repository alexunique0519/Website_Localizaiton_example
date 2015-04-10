using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OEC06.Models;
using OEC06.App_GlobalResources;

namespace OEC06.Controllers
{
    //this controller is for validating the provinceCode
    public class RemoteController : Controller
    {
        private OECContext db = new OECContext();
        
        public JsonResult ValidateProvinceCode(string provinceCode)
       {
           string pCode;
           
           try 
           {
               if (provinceCode == null || provinceCode.ToString() == "")
               {
                   return Json(true, JsonRequestBehavior.AllowGet);
               }

               pCode = provinceCode.ToString().ToUpper();
               pCode.Trim();

               if (pCode.Length != 2)
               {
                   return Json(string.Format(YYTranslations.provinceCodeLength),
                   JsonRequestBehavior.AllowGet);

               }
               else if (pCode.ElementAt(0) < 'A' || pCode.ElementAt(0) > 'Z' || pCode.ElementAt(1) < 'A' || pCode.ElementAt(1) > 'Z')
               {
                   return Json(string.Format(YYTranslations.provinceCodeLetter),
                   JsonRequestBehavior.AllowGet);
               }
               else if (null == db.provinces.Find(pCode))
               {
                   return Json(string.Format(YYTranslations.provinceNotOnFile, pCode),
                   JsonRequestBehavior.AllowGet);
               }

               return Json(true, JsonRequestBehavior.AllowGet);

           }

            
           catch(Exception ex)
           {
               string sEx = "error validating province code " + ex.GetBaseException().Message; 
               return Json( sEx, JsonRequestBehavior.AllowGet);
           }
           
       }
    
    }
}