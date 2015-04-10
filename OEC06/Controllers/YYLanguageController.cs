using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEC06.Controllers
{
    //this controller is for selecting the displaying language on UI.
    public class YYLanguageController : Controller
    {
        public ActionResult ChangeLanguage()
        {
            //new 3 listItem, en, fr and zh for English, French and Chinese. 
            SelectListItem en = new SelectListItem() { Text = "English", Value = "en"};
            SelectListItem fr = new SelectListItem() { Text = "French", Value = "fr" };
            SelectListItem zh = new SelectListItem() { Text = "Chinese", Value = "zh" };

            //define a preseletectItem to show the current language in the dropdownlist.
            SelectListItem preSelectedItem = null;

            //if there the request cookied has data in the with the key of 'language', then get the language 
            //and assign the value to preSelectedItem.
            if (Request.Cookies["language"] != null)
            {
                string language = Request.Cookies["language"].Value;

                switch(language)
                {
                    case "en":
                        en.Selected = true;
                        preSelectedItem = en;
                        break;
                    case "fr":
                        fr.Selected = true;
                        preSelectedItem = fr;
                        break;
                    case "zh":
                        zh.Selected = true;
                        preSelectedItem = zh;
                        break;
                    default:
                        en.Selected = true;
                        preSelectedItem = en;
                        break;
                }
            }
            //else make the preSeletedItem equals en.
            else
            {
                en.Selected = true;
                preSelectedItem = en;
            }

            //new a selectList for the dropdownlist in select language view.
            SelectList languages = new SelectList(new SelectListItem[] { en, fr, zh }, "Value", "Text", preSelectedItem.Value);

            //assign the selectlist to viewbad.
            ViewBag.languages = languages;

            //also add note the request url in response cookie
            Response.Cookies.Add(new HttpCookie("returnURL", Request.UrlReferrer.PathAndQuery));
            
            return PartialView();
        }

        //get the selected language and add it into the response cookie
        public void SelectLanguage(string languages)
        {
            Response.Cookies.Add(new HttpCookie("language", languages));
            if(Request.Cookies["returnURL"] != null)
            {
                Response.Redirect(Request.Cookies["returnURL"].Value);
            }
            else
            {
                Response.Redirect("/");
            }
        }
    }
}