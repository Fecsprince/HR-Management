
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

    public class BranchController : Controller
    {
        IRepository<Branch> context;

        public BranchController(IRepository<Branch> Branchcontext)
        {
            context = Branchcontext;

        }


        // GET: Branches
        public ActionResult Index()
        {
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }

            var branches = context.Collection();

            if (branches != null)
            {
                return View(branches);
            }
            else
            {
                return View();
            }

        }

        #region ITF ADMIN (ITF BRANCH CRUD)



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult Index(Branch model)
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

                        var newBranch = context.Insert(model);

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


        public ActionResult AddEditITF(string ID)
        {
            Branch model = new Branch();
            try
            {
                if (ID != null)
                {                    
                    var obj = context.Find(ID);
                    if (obj != null)
                    {
                        model.Name = obj.Name;
                        model.Address = obj.Address;
                    }
                }
            }
            catch (Exception ex)
            {
                Session["grmsg"] = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                return RedirectToAction("Index");
            }
            return PartialView("AddEditITF", model);
        }


        [Authorize(Roles = "ITFAdmin, SuperAdmin")]
        public JsonResult DeleteITF(string ID)
        {

            string msg = "";
            if (ID != null)
            {
                var objDel = con.GetITFs.SingleOrDefault(o => o.SortCode == ID);
                var objUser = con.Users.SingleOrDefault(o => o.Email == objDel.ITAdmin);
                if (objDel != null && objUser != null)
                {
                    objDel.IsActive = false;
                    objUser.IsActive = false;
                    con.SaveChanges();
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(string id)
        {
            if (id != null)
            {
                try
                {
                    var service = context.Find(id);
                    if (service != null)
                    {

                        //GET USER
                        string shopName = "";
                        //var user = userContext.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                        var shop = shopContext.Collection().Where(x => x.UserID == service.ShopID).FirstOrDefault();

                        if (shop != null)
                        {
                            shopName = shop.Name;
                        }

                        Service serviceVm = new Service()
                        {
                            Id = service.Id,
                            Name = service.Name,
                            ShopID = shopName,
                            _Image1 = service.Image1,
                            _Image2 = service.Image2,
                            Price = service.Price,
                        };


                        return View(serviceVm);
                    }
                    else
                    {
                        ViewBag.Msg = "NO RECORD FOUND!";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message != null)
                    {
                        TempData["Msg"] = ex.InnerException.Message.ToString();
                    }
                    else
                    {
                        TempData["Msg"] = ex.Message.ToString();
                    }

                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
            }
            else
            {
                ViewBag.Msg = "Invalid request, please check the shop identification!";
                return View();
            }

        }

        public ActionResult Delete(string Id)
        {
            Service serviceToDelete = context.Find(Id);

            if (serviceToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(serviceToDelete);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult ConfrimDelete(string Id)
        {

            Service serviceToDelete = context.Find(Id);

            if (serviceToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                return RedirectToAction("Index");
            }
        }
    }
}