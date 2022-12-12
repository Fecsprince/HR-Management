using HRManagement.Data.Supports;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Services.Tools
{
    public class ServiceTool
    {

        //SEND MAIL TO EMPLOYEE
        public string SendEmployeeMail(List<UploadResponseViewModel> uploadResponse,
                                        List<String> emailList)
        {
            if (uploadResponse.Count > 0 && emailList.Count > 0)
            {
                int countDone = 0;

                string response = "::";

                foreach (var email in emailList)
                {
                    foreach (var m in uploadResponse)
                    {
                        if (m.Message.Contains(email))
                        {
                            IdentityMessage idMes = new IdentityMessage()
                            {
                                Destination = email,
                                Subject = "Employee Record"
                            };

                            MailHelper sendMail1 = new MailHelper();
                            ConfirmEmailSend sendMsg1 = sendMail1.SendMail(idMes, m.Message);
                            response += "\n::" + m.Message + ":- Mail Service Response: " + sendMsg1.Message;
                            countDone += 1;

                            break;
                        }
                    }
                }
                return response += "Out of " + emailList.Count() + " employee record " + countDone
                                + " were successfully submitted!";
            }
            return "Null ";
        }
    }
}
