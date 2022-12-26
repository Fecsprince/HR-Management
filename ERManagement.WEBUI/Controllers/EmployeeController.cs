using ClosedXML.Excel;
using HRManagement.WEBUI;
using HRManagement.WEBUI.Models;
using HRManagement.Data.Models;
using HRManagement.Data.Supports;
using HRManagement.Services.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Globalization;
using ERManagement.WEBUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HRManagement.WebUI.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class EmployeeController : Controller
    {
        readonly IRepository<Employee> empContext;
        readonly IRepository<Designation> desContext;
        readonly IRepository<Branch> braContext;
        readonly IRepository<EmployeeDefaultPassword> empDefPassContext; 
        readonly private ApplicationDbContext con = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public EmployeeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public EmployeeController(IRepository<Employee> employeeContext,
                                    IRepository<Designation> designationContext,
                                        IRepository<Branch> branchContext,
                                        IRepository<EmployeeDefaultPassword> _empDefPassContext)
        {
            empContext = employeeContext;
            desContext = designationContext;
            braContext = branchContext;
            empDefPassContext = _empDefPassContext;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        #region EMPLOYEE CRUD

        [HttpGet]
        public ActionResult Index(string SCBranch = "", string SCDesignation = "")
        {
            try
            {
                if (Session["grmsg"] != null)
                {
                    ViewBag.msg = Session["grmsg"].ToString();
                }

                IEnumerable<Employee> employees = empContext.Collection();

                if (employees.Count() > 0)
                {
                    if (SCBranch != "" && SCDesignation != "")
                        employees.Where(x => x.Branch.Name == SCBranch &&
                                  x.Designation.Name == SCDesignation).
                                  OrderBy(x => x.Name);

                    else if (SCBranch != "")
                        employees.Where(x => x.Branch.Name == SCBranch).
                                   OrderBy(x => x.Name);
                    else if (SCDesignation != "")
                        employees.Where(x => x.Designation.Name == SCDesignation).
                                  OrderBy(x => x.Name);
                    else
                        employees = employees.
                            OrderBy(x => x.Name);


                    ViewBag.CurrencyFmt = CultureInfo.CreateSpecificCulture("NG-NG");
                    ViewBag.AllEmployees = employees.ToList();
                    ViewBag.EmployeesCount = employees.Count();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    ViewBag.msg = ex.Message.ToString();
                else
                    ViewBag.msg = ex.InnerException.Message.ToString();
            }

            List<SearchBranchViewModel> searchBranches = new List<SearchBranchViewModel>();
            List<SearchDesignationViewModel> searchDesignations = new List<SearchDesignationViewModel>();
            var empBranches = braContext.Collection().Select(x => x.Name).ToList();
            foreach (var br in empBranches)
            {
                searchBranches.Add(new SearchBranchViewModel()
                {
                    SBranch = br
                });
            }

            var empDesignations = desContext.Collection().Select(x => x.Name).ToList();
            foreach (var de in empDesignations)
            {
                searchDesignations.Add(new SearchDesignationViewModel()
                {
                    SDesignation = de
                });
            }

            ViewBag.EmpBranches = searchBranches;
            ViewBag.EmpDesignations = searchDesignations;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Index(EmployeeViewModel model)
        {
            string msg = "";


            try
            {
                var dbObj = empContext.Find(model.Id);

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
                    dbObj.Pension = model.Pension;

                    dbObj.GrossSalary = model.BasicSalary +
                                        model.HousingAllowance +
                                        model.TransportAllowance +
                                        model.UtilityAllowance +
                                        model.Pension;

                    double tax = (model.Tax / 100) * Convert.ToDouble(model.BasicSalary);
                    dbObj.Tax = model.Tax;

                    dbObj.NetSalary = dbObj.GrossSalary - Convert.ToDecimal(tax);


                    var updatedEmployee = empContext.Update(dbObj);
                    if (updatedEmployee != null)
                    {
                        msg = model.Name.ToString() + " record is updated successfully!";
                    }
                    else
                    {
                        msg = model.Name.ToString() + " record update FAILED!";
                    }
                }
                else
                {
                    var emp = new Employee()
                    {
                        Name = model.Name,
                        DOB = model.DOB,
                        DOE = model.DOE,
                        Email = model.Email,
                        Gender = model.Gender,
                        User_ID = model.User_ID,
                        Desgination_ID = model.Desgination_ID,
                        JobUnit_Branch_ID = model.JobUnit_Branch_ID,
                        BasicSalary = model.BasicSalary,
                        HousingAllowance = model.HousingAllowance,
                        TransportAllowance = model.TransportAllowance,
                        UtilityAllowance = model.UtilityAllowance,
                        Pension = model.Pension,
                        Tax = model.Tax
                    };

                    emp.GrossSalary = model.BasicSalary +
                                      model.HousingAllowance +
                                      model.TransportAllowance +
                                      model.UtilityAllowance +
                                      model.Pension;

                    double tax = (model.Tax / 100) * Convert.ToDouble(model.BasicSalary);
                   

                    emp.NetSalary = emp.GrossSalary - Convert.ToDecimal(tax);

                    //CREATE USER ACCOUNT FIRST
                    UploadResponseViewModel response = new UploadResponseViewModel();
                    //CALL METHOD



                    var userAccountResponse = await RegisterUser(emp);
                    if (userAccountResponse != null &&
                                           userAccountResponse.UserId != "" &&
                                           userAccountResponse.UserId != null)
                    {
                        emp.User_ID = userAccountResponse.UserId;

                        response.Message = userAccountResponse.Message;
                    }
                    else
                        response.Message = userAccountResponse.Message;

                    if (ModelState.IsValid)
                    {
                        var newEmployee = empContext.Insert(emp);

                        if (newEmployee != null)
                        {
                            msg = newEmployee.Name + " record added Successfully!";
                        }
                        else
                        {
                            msg = newEmployee.Name + " record was not added successfully!";
                        }
                    }
                    else
                    {
                        msg = "PLEASE MAKE SURE YOUR ENTRIES ARE IN CORRECT FORMAT!\n" +
                               response.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message == null)
                {
                    msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString();

                }
                msg = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN! \n" + ex.Message.ToString() + "\n" +
                        ex.InnerException.Message.ToString();
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddEditEmployee(string ID)
        {

            try
            {

                List<SearchBranchViewModel> searchBranches = new List<SearchBranchViewModel>();
                List<SearchDesignationViewModel> searchDesignations = new List<SearchDesignationViewModel>();
                var empBranches = braContext.Collection().ToList();
                foreach (var br in empBranches)
                {
                    searchBranches.Add(new SearchBranchViewModel()
                    {
                        SBranch = br.Name,
                        SBranchID = br.Id
                    });
                }

                var empDesignations = desContext.Collection().ToList();
                foreach (var de in empDesignations)
                {
                    searchDesignations.Add(new SearchDesignationViewModel()
                    {
                        SDesignation = de.Name,
                        SDesignationID = de.Id
                    });
                }

                var genderList = new GenderTools();

                ViewBag.EmpBranches = searchBranches;
                ViewBag.EmpDesignations = searchDesignations;
                ViewBag.GenderList = genderList.GenderList().ToList();


                if (ID != "")
                {
                    var obj = empContext.Find(ID);
                    if (obj != null)
                    {
                        var model = new EmployeeViewModel()
                        {
                            Name = obj.Name,
                            DOB = obj.DOB,
                            DOE = obj.DOE,
                            Id = obj.Id,
                            Email = obj.Email,
                            Gender = obj.Gender,
                            User_ID = obj.User_ID,
                            Desgination_ID = obj.Desgination_ID,
                            JobUnit_Branch_ID = obj.JobUnit_Branch_ID,
                            BasicSalary = obj.BasicSalary,
                            HousingAllowance = obj.HousingAllowance,
                            TransportAllowance = obj.TransportAllowance,
                            UtilityAllowance = obj.UtilityAllowance,
                            GrossSalary = obj.GrossSalary,
                            NetSalary = obj.NetSalary,
                            Tax = obj.Tax,
                            Pension = obj.Pension
                        };
                        return PartialView("AddEditEmployee", model);
                    }
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                else
                {
                    return PartialView("AddEditEmployee");
                }
            }
            catch (Exception ex)
            {
                Session["grmsg"] = "TRY AGAIN, IF PERSISTED, CONTACT ADMIN!";
                return RedirectToAction("Index");
            }
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

        public ActionResult EmployeeDefaultPassword()
        {
            var empDepPass = empDefPassContext.Collection();
            ViewBag.AllDefaultPasswords = empDepPass.ToList();
            ViewBag.DefaultPasswordCount = empDepPass.Count();
            return View();
        }

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
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else if (excelfile != null && excelfile.ContentLength > 0 && excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    //UPLOAD NOT EMPTY
                    //CHANGE THE FILE NAME

                    string path = Server.MapPath("~/Uploads/EmployeeDocument/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);

                    var uploadResponses = new List<UploadResponseViewModel>();

                    List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
                    List<String> emailList = new List<String>();

                    //using (var xLWorkbook = new XLWorkbook(path))
                    //{
                    XLWorkbook xLWorkbook = new XLWorkbook(path);

                    int startRow = 3;
                    while (xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 3).GetString() != "")
                    {
                        EmployeeViewModel emp = new EmployeeViewModel(); //EMPLOYEE HOLDER

                        UploadResponseViewModel upRes = new UploadResponseViewModel(); // RESPONSE TO ADMIN
                        string branchName = null;
                        string designationName = null;

                        emp.Name = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 3).GetString();
                        emp.Email = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 4).GetString();
                        emp.Gender = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 5).GetString();
                        emp.DOB = Convert.ToDateTime(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 6).GetString());
                        designationName = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 7).GetString();
                        emp.DOE = Convert.ToDateTime(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 8).GetString());
                        branchName = xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 9).GetString();
                        emp.BasicSalary = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 10).GetString());
                        emp.HousingAllowance = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 11).GetString());
                        emp.TransportAllowance = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 12).GetString());
                        emp.UtilityAllowance = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 13).GetString());
                        emp.Pension = Convert.ToDecimal(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 14).GetString());
                        emp.Tax = Convert.ToDouble(xLWorkbook.Worksheets.Worksheet(1).Cell(startRow, 15).GetString());

                        emp.GrossSalary = emp.BasicSalary +
                                     emp.HousingAllowance +
                                     emp.TransportAllowance +
                                     emp.UtilityAllowance +
                                     emp.Pension;
                        var tax = (emp.Tax / 100) * (double)emp.BasicSalary;

                        emp.NetSalary = emp.GrossSalary - Convert.ToDecimal(tax);


                        //  - ITERATE DESIGNATION AND BRANCH AND MATCH


                        IEnumerable<Branch> branches = braContext.Collection();
                        IEnumerable<Designation> designations = desContext.Collection();

                        //  - BRANCH

                        var branch = branches.Where(x => x.Name.Contains(branchName)).FirstOrDefault();
                        if (branch != null)
                            emp.JobUnit_Branch_ID = branch.Id;
                        else
                        {
                            Branch svBranch = new Branch()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = branchName,
                                Address = branchName
                            };

                            var dbBranch = braContext.Insert(svBranch);
                            emp.JobUnit_Branch_ID = dbBranch != null ? dbBranch.Id : branches.FirstOrDefault(x => x.Name.Contains("TEMPORARY")).Id;
                        }

                        //  - DESIGNATION
                        var designation = designations.Where(x => x.Name.Contains(designationName)).FirstOrDefault();
                        if (designation != null)
                            emp.Desgination_ID = designation.Id;
                        else
                        {
                            Designation svDesignation = new Designation()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = designationName,
                                Description = designationName
                            };

                            var dbDesignation = desContext.Insert(svDesignation);
                            emp.Desgination_ID = dbDesignation != null ? dbDesignation.Id : designations.FirstOrDefault(x => x.Name.Contains("TEMPORARY")).Id;
                        }

                        //  - GENERATE EMAIL, TO BE REMOVED AFTER TESTING

                        //var formattedEmail = emp.Name.Replace(" ", "") + "@gmail.com";
                        //emp.Email = formattedEmail;
                        emailList.Add(emp.Email);
                        //  NOW ADD EMP TO LIST OF EMPLOYEES CONTAINER

                        employees.Add(emp);

                        startRow++;

                    }
                    //}

                    //  -- BELLOW WILL CREATE ROW FOR EACH EMPLOYEE

                    foreach (var em in employees)
                    {
                        if (em != null)
                        {
                            var emp = new Employee()
                            {
                                HousingAllowance = em.HousingAllowance,
                                TransportAllowance = em.TransportAllowance,
                                UtilityAllowance = em.UtilityAllowance,
                                BasicSalary = em.BasicSalary,
                                Pension = em.Pension,
                                GrossSalary = em.GrossSalary,
                                NetSalary = em.NetSalary,
                                Tax = em.Tax,
                                Email = em.Email,
                                Name = em.Name,
                                DOB = em.DOB,
                                DOE = em.DOE,
                                Gender = em.Gender,
                                Desgination_ID = em.Desgination_ID,
                                JobUnit_Branch_ID = em.JobUnit_Branch_ID
                            };

                            //  -- CREATE USER ACCOUNT FIRST
                            //CALL METHOD

                            var userAccountResponse = await RegisterUser(emp);

                            if (userAccountResponse != null &&
                                                   userAccountResponse.UserId != "" &&
                                                   userAccountResponse.UserId != null)
                            {
                                emp.User_ID = userAccountResponse.UserId;
                                uploadResponses.Add(new UploadResponseViewModel()
                                {
                                    Message = userAccountResponse.Message
                                });
                              
                            }
                            else
                                uploadResponses.Add(new UploadResponseViewModel()
                                {
                                    Message = userAccountResponse.Message
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

        [NonAction]
        public async Task<UserAccountViewModel> RegisterUser(Employee emp)
        {
            string msg = "";
            string defaultPassword = Membership.GeneratePassword(10, 1) + "12";

            //  -- CREATE USER ACCOUNT FIRST
            var user = new ApplicationUser { UserName = emp.Email, Email = emp.Email };
            var result = await UserManager.CreateAsync(user, defaultPassword);

            if (result.Succeeded)
            {
                string id = emp.Id;

                //  -- ADD EMPLOYEE TO DB
                emp.Id = id;

                emp.User_ID = user.Id;


                var savEmp = empContext.Insert(emp);
                if (savEmp != null)
                {
                    string roleMsg = "";

                    //CREATE EMPLOYEE_DEFAULT_PASSWORD
                    var empDefPass = new EmployeeDefaultPassword()
                    {
                        Email = emp.Email,
                        Password = defaultPassword
                    };

                    empDefPassContext.Insert(empDefPass);

                    var roles = con.Roles.Where(x => x.Name == "Employee").FirstOrDefault();
                    if (roles != null)
                    {
                        var roleassign = RoleAssignment(user.Id, roles.Name);
                        if (roleassign)
                            roleMsg = "Role Assigned successfully!";
                        else
                            roleMsg = "Role Assigned failed!";
                    }else
                        roleMsg = "Role Assigned failed because role does not exist!";

                    msg = emp.Email + " employee record has been created successfully, " +
                                             "your defualt password is: " + defaultPassword + ".\n" + roleMsg;
                }
                else
                {
                    msg = emp.Email + " employee record could not be created successfully!";
                }


            }
            else
                msg = emp.Email + " user account could not be created successfully!\n" +
                                  result.Errors != null ? result.Errors.FirstOrDefault().ToString() : "";

            return new UserAccountViewModel()
            {
                Message = msg,
                UserId = emp.User_ID
            };
        }



        #region METHOD TO ADD EMPLOYEE INTO ROLE
        [NonAction]
        public bool RoleAssignment(string userId, string role)
        {
            if (userId == null || role == null)
            {
                return false;
            }
            else
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(con));

                var result = UserManager.AddToRole(userId, role);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion
    }



}
