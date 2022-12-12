using HRManagement.Data.Models;
using HRManagement.Data.Supports;
using HRManagement.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HRManagement.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UtilityController : Controller
    {
        private string v = "msg";
        IRepository<FeedBack> context;


        public UtilityController(IRepository<FeedBack> context)
        {
            this.context = context;
        }

        #region  FEED BACK

        [AllowAnonymous]
        [HttpPost]
        public JsonResult CreateTicket(string Email, string Subject, string Message)
        {
            string msg = "";

            try
            {
                if (Email != "" && Subject != "" && Message != "")
                {
                    try
                    {

                        FeedBack feedBack = new FeedBack()
                        {
                            Email = Email,
                            Subject = Subject,
                            Message = Message,
                            Status = false
                        };

                        var fd = context.Insert(feedBack);

                        if (fd != null)
                        {
                            msg = fd.Subject + ": has been sent successfully, we will get back to you shortly!";
                        }
                        else
                        {
                            msg = "NOT SENT, PLEASE TRY AGAIN!";
                        }
                        #region -- TO BE USED ANYTIME EMAIL SERVICE IS NEEDED
                        //if (feedBack != null)
                        //{

                        //    //GET SUPERADMIN EMAIL
                        //    var role = con.Roles.Where(x => x.Name == "SuperAdmin").FirstOrDefault();
                        //    if (role != null)
                        //    {
                        //        string superAdminEmailAddress = "";
                        //        var users = con.Users.ToList();
                        //        foreach (var user in users)
                        //        {
                        //            foreach (var rol in user.Roles)
                        //            {
                        //                if (rol.RoleId == role.Id)
                        //                {
                        //                    superAdminEmailAddress = user.Email;
                        //                }
                        //            }
                        //        }

                        //        if (superAdminEmailAddress != null && superAdminEmailAddress != "")
                        //        {
                        //            //PROCESS EMAIL
                        //            //SEND EMAIL TO SUPERADMIN

                        //            IdentityMessage custidentyMessage = new IdentityMessage()
                        //            {
                        //                Destination = superAdminEmailAddress,
                        //                Subject = "New Ticket"
                        //            };

                        //            var callbackUrl = Url.Action("Ticket", "Utility",
                        //        new { TicketNo = feedBack.TicketNo }, protocol: Request.Url.Scheme);


                        //            string html = String.Format("<h2>A ticket has been created successfully, " +
                        //                "to view the ticket kindly click " + callbackUrl +
                        //             " or copy the link and paste in Chrome browser or any other latest browser.</h2>");



                        //            MailHelper sendMail = new MailHelper();
                        //            ConfirmEmailSend sendMsg = sendMail.SendMail(custidentyMessage, html);


                        //            //PROCESS EMAIL
                        //            //SEND EMAIL TO USER

                        //            IdentityMessage useridentyMessage = new IdentityMessage()
                        //            {
                        //                Destination = feedBack.Email,
                        //                Subject = "Ticket Created"
                        //            };

                        //            string html2 = String.Format("<h2>Your ticket has been created successfully, " +
                        //                 "to view your ticket kindly click " + callbackUrl +
                        //              " or copy the link and paste in Chrome browser or any other latest browser.</h2>" +
                        //              "<hr/><h3 style='color:green;'>Thank you for message, we will respond to your request withn 24hours!</h3><hr/>");



                        //            MailHelper sendMaix = new MailHelper();
                        //            ConfirmEmailSend sendMsg2 = sendMaix.SendMail(useridentyMessage, html2);

                        //            msg = "Message was sent successfully to our technical support department, an agent will respond to your request within 24hours.";

                        //            con.FeedBacks.Add(feedBack);
                        //            con.SaveChanges();

                        //        }
                        //        else
                        //        {
                        //            msg = "NO SUPER ADMIN ACCOUNT AVAILABLE!";
                        //        }

                        //    }
                        //    else
                        //    {
                        //        msg = "NO SUPER ADMIN ROLE!";
                        //    }
                        //}
                        #endregion


                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                            msg = "FATAL ERROR LEVEL 1: IF THIS PERSIST, CONTACT ADMIN WITH ERROR DETAIL: " + ex.InnerException.Message + ".";
                        else
                            msg = "FATAL ERROR LEVEL 1: IF THIS PERSIST, CONTACT ADMIN WITH ERROR DETAIL: " + ex.Message + ".";
                    }
                }
                else
                {
                    msg = "PLEASE MAKE SURE YOU'RE NOT LEAVING ANY FIELD EMPTY, TRY AGAIN!";
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    msg = "FATAL ERROR LEVEL 2: IF THIS PERSIST, CONTACT ADMIN WITH ERROR DETAIL: " + ex.InnerException.Message + ".";
                else
                    msg = "FATAL ERROR LEVEL 2: IF THIS PERSIST, CONTACT ADMIN WITH ERROR DETAIL: " + ex.Message + ".";

            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Ticket(string TicketNo)
        {
            var ticketList = new List<FeedBackVM>();

            if (TicketNo != null)
            {
                var ticket = context.Find(TicketNo);
                if (ticket != null)
                {
                    var ticketVm = new FeedBackVM()
                    {
                        Email = ticket.Email,
                        Subject = ticket.Subject,
                        Message = ticket.Message,
                        CreatedAt = ticket.CreatedAt.ToString("hh:mm - dd/MM/yyyy"),
                        Id = ticket.Id,
                        TicketNo = ticket.TicketNo,
                        Status = ticket.Status
                    };
                    ticketList.Add(ticketVm);
                }
                else
                {
                    TempData[v] = "UNKNOWN TICKET NUMBER, NO RECORD FOUND!";
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (User.IsInRole("SuperAdmin"))
            {
                var listFeedbacks = context.Collection();

                foreach (var item in listFeedbacks)
                {
                    var ticketVm = new FeedBackVM()
                    {
                        Email = item.Email,
                        Subject = item.Subject,
                        Message = item.Message,
                        CreatedAt = item.CreatedAt.ToString("hh:mm - dd/MM/yyyy"),
                        Id = item.Id,
                        TicketNo = item.TicketNo,
                        Status = item.Status
                    };
                    ticketList.Add(ticketVm);
                }
            }
            else
            {
                TempData[v] = "EMPTY TICKET NUMBER!";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.FeedBacks = ticketList.ToList();
            return View();

        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult TicketStatus(string ID)
        {

            if (ID.Trim() != "")
            {
                var ticket = context.Find(ID);
                if (ticket != null)
                {
                    if (ticket.Status == true)
                    {
                        ticket.Status = false;
                        TempData[v] = "Ticket closed successfully!";
                    }
                    else
                    {
                        ticket.Status = true;
                        TempData[v] = "Ticket opened successfully!";
                    }
                    context.Commit();
                }
                else
                    TempData[v] = "NO RECORD FOUND!";
            }
            else
                TempData[v] = "ERROR: Operation terminated!";

            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}