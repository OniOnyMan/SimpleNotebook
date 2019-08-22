using SimpleNotebook.BLL.Abstract;
using SimpleNotebook.PL.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleNotebook.PL.Web.Controllers
{
    public class NoteController : Controller
    {
        public ActionResult Index()
        {
            return View(NoteVM.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteVM noteVM)
        {
            if (!ModelState.IsValid)
                return View(noteVM);

            if (NoteVM.Add(noteVM))
                return RedirectToAction("Index");
            else
            {
                ViewBag.Error = "Error occurred while processing your request";
                return View(noteVM);
            }
        }

        public JsonResult IsExpectedBirthYear(int birthYear)
        {
            var throwError = true;
            if (Request.UrlReferrer != null && Request.UrlReferrer.OriginalString.Contains("Create"))
                throwError = NoteVM.Check(birthYear);                
            return Json(throwError, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Delete(Guid id)
        {
            var model = NoteVM.Get(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            if(NoteVM.Remove(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Error occurred while processing your request";
                return View(NoteVM.Get(id));
            }
        }

        public ActionResult Search(string query)
        {
            ViewBag.SearchQuery = query;
            return View(NoteVM.Find(query));
        }
    }
}
