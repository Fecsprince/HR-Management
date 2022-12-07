
using HRManagement.Data.Models;
using HRManagement.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SylistStore.WebUI.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class DesignationController : Controller
    {
        IRepository<Designation> context;

        public DesignationController(IRepository<Designation> Designationcontext)
        {
            context = Designationcontext;

        }

        #region BRANCH CRUD

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var designations = context.Collection();

                if (designations != null && designations.Count() > 0)
                {
                    ViewBag.AllDesignations = designations;
                    ViewBag.DesignationsCount = designations.Count();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.msg = Session["csmsg"].ToString() + " " + ex.InnerException.Message.ToString();
                else
                    ViewBag.msg = Session["csmsg"].ToString() + " " + ex.Message.ToString();

                return View();
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Index(Designation model)
        {
            string msg = "";

            if (ModelState.IsValid)
            {

                var dbObj = context.Find(model.Id);
                try
                {
                    if (dbObj != null)
                    {
                        dbObj.Name = model.Name;
                        dbObj.Description = model.Description;
                        var updatedDesigation = context.Update(dbObj);
                        if (updatedDesigation != null)
                        {
                            msg = model.Name.ToString() + " is updated successfully!";
                        }
                        else
                        {
                            msg = model.Name.ToString() + " update FAILED!";
                        }
                    }
                    else
                    {

                        var newDesigation = context.Insert(model);

                        if (newDesigation != null)
                        {
                            msg = newDesigation.Name + " added Successfully!";
                        }
                        else
                        {
                            msg = newDesigation.Name + " was not added successfully!";
                        }

                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message != null)
                    {
                        msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString() + "\n" +
                            ex.InnerException.Message.ToString();
                    }
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();
                }
            }
            else
            {
                msg = "PLEASE MAKE SURE YOUR ENTRIES ARE IN CORRECT FORMAT!";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddEditDesigation(string ID)
        {
            Designation model = new Designation();
            try
            {
                if (ID != null)
                {
                    var obj = context.Find(ID);
                    if (obj != null)
                    {
                        model.Name = obj.Name;
                        model.Description = obj.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                Session["grmsg"] = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                return RedirectToAction("Index");
            }
            return PartialView("AddEditDesigation", model);
        }


        public JsonResult DeleteDesignation(string ID)
        {

            string msg = "";
            if (ID != null)
            {
                var objDel = context.Delete(ID);
                if (objDel == true)
                {
                    msg = "1";
                }
                else
                {
                    msg = "0";
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}