
using HRManagement.Data.Models;
using HRManagement.Data.Supports;
using HRManagement.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SylistStore.WebUI.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class BranchController : Controller
    {
        IRepository<Branch> context;

        public BranchController(IRepository<Branch> Branchcontext)
        {
            context = Branchcontext;

        }



        #region BRANCH CRUD
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var branches = context.Collection().ToList();

                if (branches != null && branches.Count() > 0)
                {
                    ViewBag.AllBranches = branches;
                    ViewBag.BranchCount = branches.Count();
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
        public JsonResult Index(BranchViewModel model)
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
                        dbObj.Address = model.Address;
                        var updatedBranch = context.Update(dbObj);
                        if (updatedBranch != null)
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
                        var branch = new Branch()
                        {
                            Name = model.Name,
                            Address = model.Address
                        };
                        var newBranch = context.Insert(branch);

                        if (newBranch != null)
                        {
                            msg = newBranch.Name + " added Successfully!";
                        }
                        else
                        {
                            msg = newBranch.Name + " was not added successfully!";
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


        public ActionResult AddEditBranch(string ID)
        {

            try
            {

                if (ID != "")
                {
                    var obj = context.Find(ID);
                    if (obj != null)
                    {
                        var model = new BranchViewModel() { Id = obj.Id, Name = obj.Name, Address = obj.Address };
                        return PartialView("AddEditBranch", model);
                    }
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                else
                {
                    var model = new BranchViewModel();
                    return PartialView("AddEditBranch", model);
                }
            }
            catch (Exception ex)
            {
                Session["grmsg"] = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
            }
            return RedirectToAction("Index");
        }


        public JsonResult DeleteBranch(string ID)
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