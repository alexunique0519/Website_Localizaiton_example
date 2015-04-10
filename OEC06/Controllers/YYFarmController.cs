using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEC06.Models;
using OEC06.App_GlobalResources;

// this controller is to display and maintain all farms that have had or could have
// fertilizer test plots

namespace OEC06.Controllers
{
    public class YYFarmController : Controller
    {
        private OECContext db = new OECContext();

        // list all farms on file
        public ActionResult Index()
        {
            var farms = db.farms.Include(f => f.province).OrderBy(a=>a.province.name).ThenBy(a=>a.name);
            return View(farms.ToList());
        }

        // display the details of the selected farm
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            return View(farm);
        }

        // GET: Farm/Create
        public ActionResult Create()
        {
            //ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a=>a.name), "provinceCode", "name");
            return View();
        }

        // new farm record filled in ... save to database if it passes edits.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(farm farm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.farms.Add(farm);
                    db.SaveChanges();
                    //if the insert works, place a message and translate it to the current culture language, save it into tempdata
                    TempData["Message"] = string.Format(YYTranslations.recordInsertedForX, farm.name);
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                //if there is any exception thrown, translate the lead-in exception message to the current culture language
                ModelState.AddModelError("", YYTranslations.insertException + ex.GetBaseException().Message);
            }            

            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a => a.name), "provinceCode", "name", farm.provinceCode);
            return View(farm);
        }

        // present the selected farm for updates
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a => a.name), "provinceCode", "name", farm.provinceCode);
            return View(farm);
        }

        // farm record updated ... save to database if it passes edits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(farm farm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(farm).State = EntityState.Modified;
                    db.SaveChanges();
                    //if the update works, place a message and translate it to the current culture language, save it into tempdata
                    TempData["Message"] = string.Format(YYTranslations.recordUpdatedForX, farm.name);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //if there is any exception thrown, translate the lead-in exception message to the current culture language
                ModelState.AddModelError("", YYTranslations.updateException + ex.GetBaseException().Message);
            }
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a => a.name), "provinceCode", "name", farm.provinceCode);
            return View(farm);
        }

        // display farm record for confirmation that it is to be deleted
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            return View(farm);
        }

        // delete confirmed ... remove farm from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                farm farm = db.farms.Find(id);
                string name = farm.name;
                db.farms.Remove(farm);
                db.SaveChanges();
                //if the update works, place a message and translate it to the current culture language, save it into tempdata
                TempData["Message"] = string.Format(YYTranslations.recordDeletedForX, name);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //using translation res for the delete exception lead-in message
                TempData["Message"] = string.Format(YYTranslations.deleteException + "Deleting the Farm record of " + db.farms.Find(id).name + ex.GetBaseException().Message);
            }
            return RedirectToAction("Delete", new { id = id });
        }

        // release memory and connections associated with this page 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
