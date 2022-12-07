
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

    public class EmployeeController : Controller
    {
        IRepository<Employee> context;

        public EmployeeController(IRepository<Employee> Employeecontext)
        {
            context = Employeecontext;

        }

        #region EMPLOYEE CRUD

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var employees = context.Collection();

                if (employees != null && employees.Count() > 0)
                {
                    ViewBag.AllEmployees = employees;
                    ViewBag.EmployeesCount = employees.Count();
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
        public JsonResult Index(Employee model)
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
                        dbObj.DOB = model.DOB;
                        dbObj.DOE = model.DOE;
                        dbObj.BasicSalary = model.BasicSalary;
                        dbObj.HousingAllowance = model.HousingAllowance;
                        dbObj.TransportAllowance = model.TransportAllowance;
                        dbObj.UtilityAllowance = model.UtilityAllowance;
                        dbObj.GrossSalary = model.GrossSalary;
                        dbObj.NetSalary = model.NetSalary;
                        dbObj.Tax = model.Tax;
                        dbObj.Pension = model.Pension;
                        
                        
                        
                        var updatedEmployee = context.Update(dbObj);
                        if (updatedEmployee != null)
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

                        var newEmployee = context.Insert(model);

                        if (newEmployee != null)
                        {
                            msg = newEmployee.Name + " added Successfully!";
                        }
                        else
                        {
                            msg = newEmployee.Name + " was not added successfully!";
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


        public ActionResult AddEditEmployee(string ID)
        {
            Employee model = new Employee();
            try
            {
                if (ID != null)
                {
                    var obj = context.Find(ID);
                    if (obj != null)
                    {
                        model.Name = obj.Name;
                        model.DOB = obj.DOB;
                        model.DOE = obj.DOE;
                        model.BasicSalary = obj.BasicSalary;
                        model.HousingAllowance = obj.HousingAllowance;
                        model.TransportAllowance = obj.TransportAllowance;
                        model.UtilityAllowance = obj.UtilityAllowance;
                        model.GrossSalary = obj.GrossSalary;
                        model.NetSalary = obj.NetSalary;
                        model.Tax = obj.Tax;
                        model.Pension = obj.Pension;
                    }
                }
            }
            catch (Exception ex)
            {
                Session["grmsg"] = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                return RedirectToAction("Index");
            }
            return PartialView("AddEditEmployee", model);
        }


        public JsonResult DeleteEmployee(string ID)
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

        #region --  EMPLOYEE EXCEL RECORD UPLOAD
        //public ActionResult UploadLogBooks()
        //{

        //    if (TempData["Msg"] != null)
        //    {
        //        ViewBag.Msg = TempData["Msg"];
        //    }

        //    if (User.IsInRole("SuperAdmin"))
        //    {
        //        ViewBag.Institution = con.GetInstitutions.Where(x => x.Active == true).ToList();
        //    }
        //    else
        //    {
        //        var inst = con.GetInstitutions.FirstOrDefault(x => x.ITFAdmin == User.Identity.Name);
        //        ViewBag.ITFAdmin = inst.ITFAdmin;

        //    }
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult UploadLogBooks(HttpPostedFileBase excelfile, string ITFAdmin)
        //{
        //    string msg = "";
        //    int countDone = 0;
        //    try
        //    {
        //        if (ITFAdmin != "")
        //        {
        //            //CHECK IF UPLOAD FILE IS EMPTY
        //            if (excelfile == null || excelfile.ContentLength < 0)
        //            {
        //                msg = "Please select a file, Try again!";
        //                //return View(model);
        //                return Json(msg, JsonRequestBehavior.AllowGet);
        //            }
        //            else if (excelfile != null && excelfile.ContentLength > 0 && excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
        //            {
        //                //UPLOAD NOT EMPTY
        //                //CHANGE THE FILE NAME
        //                string fileNewName = DateTime.Now.ToString("ddMMyyyyhhmmss") + Path.GetExtension(excelfile.FileName);

        //                string path = Server.MapPath("~/Uploads/LogBookUploads/" + fileNewName);
        //                if (System.IO.File.Exists(path))
        //                    System.IO.File.Delete(path);
        //                excelfile.SaveAs(path);



        //                //STUDENT EMAIL COLLECTIONS

        //                var uploadResponses = new List<UploadResponse>();

        //                //GET INSTITUTION ID FROM INSTITUTION ITF ADMIN UPLOADING


        //                XLWorkbook xLWorkbook = new XLWorkbook(path);

        //                int startRow = 3;
        //                while (xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 1).GetString() != "")
        //                {
        //                    StudentLogbook stdLog = new StudentLogbook(); //STUDENT HOLDER

        //                    UploadResponse upRes = new UploadResponse(); // RESPONSE TO ITF ADMIN



        //                    stdLog.StudentId = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 2).GetString();
        //                    stdLog.WeekNO = Convert.ToInt32(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 3).GetString());
        //                    stdLog.DayNo = Convert.ToInt32(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 4).GetString());
        //                    DateTime date = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 9).GetDateTime();

        //                    stdLog.Age = date.ToString("MM/dd/yyyy");

        //                    var student = con.GetStudents.FirstOrDefault(x => x.Id == stdLog.StudentId);



        //                    // DOES THE STUDENT APPLICANT EXIST?
        //                    var log = con.GetStudentLogbooks.Where(x => x.StudentId == stdLog.StudentId && x.DayNo == stdLog.DayNo && x.WeekNO == stdLog.WeekNO).FirstOrDefault();

        //                    if (log == null) // DOES NOT EXIST
        //                    {
        //                        //ADD STUDENT TO APPLICANT LIST
        //                        con.GetStudentLogbooks.Add(stdLog);

        //                        string ms = student.Regno.ToString() + " log was submitted succesfully!";

        //                        //ADD RESPONSE TO UPLOAD RESPONSE
        //                        upRes.Message = ms;
        //                        uploadResponses.Add(upRes);
        //                        countDone += 1;
        //                    }
        //                    else // YES STUDENT EXIST
        //                    {
        //                        string ms = student.Regno.ToString() + " log was Duplicated!";
        //                        //ADD RESPONSE TO UPLOAD RESPONSE
        //                        upRes.Message = ms;
        //                        uploadResponses.Add(upRes);
        //                        countDone += 1;
        //                    }
        //                    startRow++;

        //                }

        //                //SAVE
        //                int isSaved = con.SaveChanges();
        //                System.IO.File.Delete(path);

        //                if (isSaved > 0) // GREATER THAN 1 MEANS SAVED
        //                {
        //                    //INFORM ITF INSTITUTION ADMIN OF THE UPLOAD MADE
        //                    var _user = User.Identity.Name;

        //                    IdentityMessage ITidMes = new IdentityMessage()
        //                    {
        //                        Destination = _user,
        //                        Subject = "Upload response"
        //                    };

        //                    //GO THROUGH ALL THE RESPONSES AND SEND MAIL TO INSTITUTION ITF EMAIL
        //                    string mail = "";
        //                    foreach (var m in uploadResponses)
        //                    {
        //                        mail += "<hr />" + m.Message;
        //                    }

        //                    MailHelper sendMail1 = new MailHelper();
        //                    ConfirmEmailSend sendMsg1 = sendMail1.SendMail(ITidMes, mail);

        //                    msg = countDone.ToString() + " student(s) uploaded successfully, please check your email for more details!\n" + sendMsg1.Message;
        //                }
        //                else
        //                {
        //                    string mail = "";
        //                    foreach (var m in uploadResponses)
        //                    {
        //                        mail += "\n" + m.Message;
        //                    }
        //                    msg = mail;
        //                }

        //            }
        //            else
        //            {
        //                msg = "Please select a valid file, and Try again!";
        //            }
        //        }
        //        else
        //        {
        //            msg = "Sorry, Institution Admin cannot be empty!";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //            msg = "Fatal Error: If persisted, contact Admin with the Details: " + ex.InnerException.Message + ".";
        //        else
        //            msg = "Fatal Error: If persisted, contact Admin with the Details: " + ex.Message + ".";
        //    }

        //    TempData["Msg"] = msg;
        //    return RedirectToAction("UploadLogBooks");
        //}


        #endregion
    }
}