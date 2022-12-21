using HRManagement.Data.Models;
using HRManagement.Data.Supports;
using HRManagement.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERManagement.WEBUI.Controllers
{
    [Authorize(Roles = "SuperAdmin,Employee")]
    public class UserEmployeeController : Controller
    {
        readonly IRepository<Employee> empContext;
        public UserEmployeeController(IRepository<Employee> employeeContext)
        {
            empContext = employeeContext;
        }


        public ActionResult GetEmployee()
        {
            string Email = User.Identity.Name;
            if (Email != null)
            {
                ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");

                var objEmps = empContext.Collection();

                var emp = objEmps.FirstOrDefault(x => x.Email == Email);

                if (emp != null)
                {
                    if (emp.Passport != null)
                    {
                        var base64 = Convert.ToBase64String(emp.Passport);
                        var imgSrc = String.Format("data:image/png;base64,{0}", base64);
                        ViewBag.Cert = imgSrc;
                    }

                    return View(emp);
                }
                else
                {
                    HttpNotFound();
                }
            }
            TempData["Msg"] = "Sorry, no employee record found!";
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public ActionResult UploadPassport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPassport(UploadEmployeePassportViewModel model)
        {
            try
            {
                string msg = "";
                // CHECK IF IMAGE1 UPLOAD FILE IS EMPTY
                if (model.PassportUpload == null || model.PassportUpload.ContentLength < 0)
                {
                    msg = "Please select a file, Try again!";
                    ViewBag.Msg = msg;

                    return View();
                }
                else if (model.PassportUpload != null && model.PassportUpload.ContentLength > 0 &&
                    model.PassportUpload.FileName.ToLower().EndsWith("jpg") ||
                    model.PassportUpload.FileName.ToLower().EndsWith("png"))
                {
                    //UPLOAD NOT EMPTY
                    string email = User.Identity.Name;
                    string passName = email.Replace(".", "").Replace("@", "") + Path.GetExtension(model.PassportUpload.FileName);

                    string pathx = Server.MapPath("~/Uploads/Passports/" + passName);
                    model.PassportUpload.SaveAs(pathx);


                    if (pathx != null)
                    {
                        using (Image img = Image.FromFile(pathx))
                        {
                            var ms = new MemoryStream();

                            img.Save(ms, ImageFormat.Jpeg);

                            var bytes = ms.ToArray();

                            // NOW SAVE BYTE WITH STUDENT REGNO NUMBER
                            if (bytes.Length > 0)
                            {
                                var emps = empContext.Collection();
                                if (emps.Count() > 0)
                                {
                                    var emp = emps.FirstOrDefault(x => x.Email == email);
                                    if (emp != null)
                                    {
                                        emp.Passport = bytes;
                                        empContext.Commit();
                                        ViewBag.Msg = "Passport Upload Completed";
                                    }
                                }
                                else
                                    ViewBag.Msg = "No Employee Record Found!";
                            }
                            else
                            {
                                ViewBag.Msg = "Image conversion failed!";
                            }
                        }
                    }
                    else
                        ViewBag.Msg = "Employee Passport Upload counld not be completed successfully" +
                            " because, the image renaming failed!";

                    if (System.IO.File.Exists(pathx))
                        System.IO.File.Delete(pathx);
                    //model.PassportUpload.SaveAs(pathx);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == null)
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" + ex.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
                else
                {
                    TempData["Msg"] = "Please copy response and send to admin: \n" +
                                            ex.Message.ToString() + "\n" +
                                            ex.InnerException.Message.ToString();
                    ViewBag.Msg = TempData["Msg"].ToString();
                    return View();
                }
            }
            return View();
        }

    }
}