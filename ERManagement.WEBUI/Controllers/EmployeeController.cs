using ClosedXML.Excel;
using ERManagement.WEBUI;
using ERManagement.WEBUI.Models;
using HRManagement.Data.Models;
using HRManagement.Data.Supports;
using HRManagement.Services.Interfaces;
using HRManagement.Services.Tools;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SylistStore.WebUI.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class EmployeeController : Controller
    {
        readonly IRepository<Employee> empContext;
        readonly IRepository<Designation> desContext;
        readonly IRepository<Branch> braContext;
        readonly private ApplicationDbContext con;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public EmployeeController(IRepository<Employee> employeeContext,
                                    IRepository<Designation> designationContext,
                                        IRepository<Branch> branchContext,
                                            ApplicationUserManager userManager,
                                                 ApplicationSignInManager signInManager)
        {
            empContext = employeeContext;
            desContext = designationContext;
            braContext = branchContext;
            con = new ApplicationDbContext();
            _userManager = userManager;
            _signInManager = signInManager;


        }

        #region EMPLOYEE CRUD

        [HttpGet]
        public ActionResult Index(string branch = null)
        {
            try
            {
                IEnumerable<Employee> employees = new List<Employee>();
                if (branch != null)
                {
                    employees = empContext.Collection().
                        Where(x => x.Branch.Name == branch).
                        OrderBy(x => x.Name).
                        ToList();
                }
                else
                    employees = empContext.Collection().
                        OrderBy(x => x.Name);

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

                var dbObj = empContext.Find(model.Name);
                try
                {
                    if (dbObj != null)
                    {
                        dbObj.Name = model.Name;
                        dbObj.DOB = model.DOB;

                        dbObj.Email = model.Email;
                        dbObj.Desgination_ID = model.Desgination_ID;
                        dbObj.JobUnit_Branch_ID = model.JobUnit_Branch_ID;

                        dbObj.DOE = model.DOE;

                        dbObj.BasicSalary = model.BasicSalary;
                        dbObj.HousingAllowance = model.HousingAllowance;
                        dbObj.TransportAllowance = model.TransportAllowance;
                        dbObj.UtilityAllowance = model.UtilityAllowance;
                        dbObj.GrossSalary = model.GrossSalary;
                        dbObj.NetSalary = model.NetSalary;
                        dbObj.Tax = model.Tax;
                        dbObj.Pension = model.Pension;



                        var updatedEmployee = empContext.Update(dbObj);
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

                        var newEmployee = empContext.Insert(model);

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
                    var obj = empContext.Find(ID);
                    if (obj != null)
                    {
                        model.Name = obj.Name;
                        model.DOB = obj.DOB;
                        model.DOE = obj.DOE;
                        model.Email = obj.Email;
                        model.Desgination_ID = obj.Desgination_ID;
                        model.JobUnit_Branch_ID = obj.JobUnit_Branch_ID;
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

        public ActionResult Employee(string empId = null)
        {
            if (empId == null)
            {
                 return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
               var returnedEmp = empContext.Find(empId);
                if (returnedEmp == null)
                {
                    HttpNotFound();
                }
                else
                {
                    return View(returnedEmp);
                }
            }
            return View();
        }

        public JsonResult DeleteEmployee(string ID)
        {

            string msg = "";
            if (ID != null)
            {
                var objDel = empContext.Delete(ID);
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
        public ActionResult UploadEmployee()
        {

            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> UploadEmployee(HttpPostedFileBase excelfile)
        {
            string msg = "";

            try
            {
                //CHECK IF UPLOAD FILE IS EMPTY
                if (excelfile == null || excelfile.ContentLength < 0)
                {
                    msg = "Please select a file, Try again!";
                    //return View(model);
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else if (excelfile != null && excelfile.ContentLength > 0 && excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    //UPLOAD NOT EMPTY
                    //CHANGE THE FILE NAME
                    string fileNewName = DateTime.Now.ToString("ddMMyyyyhhmmss") + Path.GetExtension(excelfile.FileName);

                    string path = Server.MapPath("~/Uploads/EmployeeDocument/" + fileNewName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);

                    var uploadResponses = new List<UploadResponse>();

                    //XLWorkbook xLWorkbook = new XLWorkbook(path);

                    List<Employee> employees = new List<Employee>();
                    List<String> emailList = new List<String>();

                    using (var xLWorkbook = new XLWorkbook(path))
                    {
                        int startRow = 3;
                        while (xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 1).GetString() != "")
                        {
                            Employee emp = new Employee(); //EMPLOYEE HOLDER

                            UploadResponse upRes = new UploadResponse(); // RESPONSE TO ADMIN
                            string branchName = null;
                            string designationName = null;

                            emp.Name = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 3).GetString();
                            emp.Gender = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 4).GetString();
                            emp.DOB = Convert.ToDateTime(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 5).GetString());
                            designationName = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 6).GetString();
                            emp.DOE = Convert.ToDateTime(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 7).GetString());
                            branchName = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 8).GetString();
                            emp.BasicSalary = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 9).GetString());
                            emp.HousingAllowance = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 10).GetString());
                            emp.TransportAllowance = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 11).GetString());
                            emp.UtilityAllowance = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 12).GetString());
                            emp.Pension = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 13).GetString());
                            emp.Tax = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 14).GetString());
                            emp.GrossSalary = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 15).GetString());
                            emp.NetSalary = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 16).GetString());


                            //  - ITERATE DESIGNATION AND BRANCH AND MATCH

                            //  - BRANCH
                            var branch = braContext.Collection().Where(x => x.Name.Contains(branchName)).FirstOrDefault();
                            if (branch != null)
                                emp.JobUnit_Branch_ID = branch.Id;
                            else
                            {
                                branch = braContext.Collection().Where(x => x.Name == "TEMPRORAY").FirstOrDefault();
                                emp.JobUnit_Branch_ID = branch.Id;
                            }

                            //  - DESIGNATION
                            var designation = desContext.Collection().Where(x => x.Name.Contains(branchName)).FirstOrDefault();
                            if (branch != null)
                                emp.Desgination_ID = designation.Id;
                            else
                            {
                                designation = desContext.Collection().Where(x => x.Name == "TEMPRORAY").FirstOrDefault();
                                emp.Desgination_ID = designation.Id;
                            }

                            //  - GENERATE EMAIL, TO BE REMOVED AFTER TESTING

                            var formattedEmail = emp.Name.Trim() + "@gmail.com";
                            emp.Email = formattedEmail;
                            emailList.Add(emp.Email);
                            //  NOW ADD EMP TO LIST OF EMPLOYEES CONTAINER

                            employees.Add(emp);

                            startRow++;

                        }
                    }

                    //  -- BELLOW WILL CREATE ROW FOR EACH EMPLOYEE

                    foreach (var em in employees)
                    {
                        if (em != null)
                        {
                            string defaultPassword = Membership.GeneratePassword(10, 1);

                            //  -- CREATE USER ACCOUNT FIRST
                            var user = new ApplicationUser { UserName = em.Email, Email = em.Email };
                            var result = await _userManager.CreateAsync(user, defaultPassword);

                            if (result.Succeeded)
                            {
                                //  -- ADD EMPLOYEE TO DB

                                var savEmp = empContext.Insert(em);
                                if (savEmp != null)
                                {
                                    uploadResponses.Add(new UploadResponse()
                                    {
                                        Message = em.Email + " employee record has been created successfully, " +
                                                             "your defualt password is: " + defaultPassword
                                    });

                                }
                                else
                                {
                                    uploadResponses.Add(new UploadResponse()
                                    {
                                        Message = em.Email + " employee record could not created successfully!"
                                    });
                                }
                            }
                            else
                                uploadResponses.Add(new UploadResponse()
                                {
                                    Message = em.Email + " user account could not created successfully!"
                                });
                        }
                    }


                    string response = "";
                    foreach (var m in uploadResponses)
                    {
                        response += "\n" + m.Message;
                    }
                    msg = response;

                    //  --  TO BE USE ONLY WHEN EMAIL SERVICE IS ACTIVE

                    //ServiceTool serviceTool = new ServiceTool();
                    //var returnedResponse = serviceTool.SendEmployeeMail(uploadResponses, emailList);

                    //msg = returnedResponse;
                }
                else
                {
                    msg = "Please select a valid file, and Try again!";
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "Fatal Error: If persisted, contact Admin with the Details: " + ex.InnerException.Message + ".";
                else
                    msg = "Fatal Error: If persisted, contact Admin with the Details: " + ex.Message + ".";
            }

            TempData["Msg"] = msg;
            return RedirectToAction("UploadEmployee");
        }


        #endregion
    }
}