using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

//using ClassOfMagic.Helpers.Custom;

namespace Sunburst.Helpers.Email
{
    public class Email
    {
        public static void SendMail(string to, string subject, string msg, string cc)
        {
            try
            {
                string SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmailAddress"];
                string FromEmailAddress= System.Configuration.ConfigurationManager.AppSettings["FromEmailAddress"];
                string SenderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["SenderEmailPassword"];
                string SenderSMTPServer = System.Configuration.ConfigurationManager.AppSettings["SenderSMTPServer"];
                int Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);
                bool IsSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsSsl"]);
                string DisplayName = System.Configuration.ConfigurationManager.AppSettings["DisplayName"];
                bool IsLive = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsLive"]);

                MailMessage message = new MailMessage();
                string[] addresses = to.Split(';');
                foreach (string address in addresses)
                {
                    message.To.Add(new MailAddress(address));
                }

                if (string.IsNullOrEmpty(cc) == false)
                    message.CC.Add(new MailAddress(cc));

                if (IsLive == false)
                {
                    //message.Bcc.Add("abcd@gmail.com");
                    //  message.Bcc.Add("wxyz@gmail.com");
                }

                message.From = new MailAddress(FromEmailAddress, DisplayName);
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = SenderSMTPServer;
                if (Port > 0)
                    client.Port = Port;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(FromEmailAddress, SenderEmailPassword);
                client.EnableSsl = IsSsl;
                client.Credentials = nc;
                client.Send(message);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, ex.TargetSite.Name + " - " + ex.Message);
            }
        }

        public static void SendMail(string to, string subject, string message)
        {
            string from = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"];
            string password = System.Configuration.ConfigurationManager.AppSettings["PasswordFrom"];
            string smtp = System.Configuration.ConfigurationManager.AppSettings["Smtp"];
            int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);
            bool isSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsSsl"]);
            try
            {
                MailMessage oMsg = new MailMessage();
                oMsg.From = new MailAddress(from, "Yes Let's");
                oMsg.To.Add(new MailAddress(to));
                oMsg.ReplyToList.Add(new MailAddress(from));
                oMsg.Subject = subject;
                oMsg.Body = message;
                oMsg.SubjectEncoding = System.Text.Encoding.UTF8;
                oMsg.BodyEncoding = System.Text.Encoding.UTF8;
                oMsg.IsBodyHtml = true;
                oMsg.Priority = MailPriority.High;
                SmtpClient oSmtp = new SmtpClient();
                oSmtp.Host = smtp;
                if (port > 0)
                    oSmtp.Port = port;
                oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                oSmtp.EnableSsl = isSsl;
                NetworkCredential oCredential = new NetworkCredential(from, password);
                oSmtp.UseDefaultCredentials = false;
                oSmtp.Credentials = oCredential;
                oSmtp.Send(oMsg);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, ex.ToString());
            }
        }

        public static void SendBulkMail(List<string> toBCC, string subject, string msg, string cc)
        {
            try
            {
                string SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmailAddress"];
                string SenderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["SenderEmailPassword"];
                string SenderSMTPServer = System.Configuration.ConfigurationManager.AppSettings["SenderSMTPServer"];
                int Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);
                bool IsSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsSsl"]);
                string DisplayName = System.Configuration.ConfigurationManager.AppSettings["DisplayName"];

                MailMessage message = new MailMessage();

                foreach (string address in toBCC)
                {
                    message.Bcc.Add(new MailAddress(address));
                }

                if (string.IsNullOrEmpty(cc) == false)
                    message.CC.Add(new MailAddress(cc));

                message.From = new MailAddress(SenderEmailAddress, DisplayName);
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = SenderSMTPServer;
                if (Port > 0)
                    client.Port = Port;
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(SenderEmailAddress, SenderEmailPassword);
                client.EnableSsl = IsSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = nc;
                client.Send(message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void SendMailWithAttachment(string to, string subject, string msg, string cc, string filepath)
        {
            try
            {
                string SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmailAddress"];
                string SenderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["SenderEmailPassword"];
                string SenderSMTPServer = System.Configuration.ConfigurationManager.AppSettings["SenderSMTPServer"];
                int Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);
                bool IsSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsSsl"]);
                string DisplayName = System.Configuration.ConfigurationManager.AppSettings["DisplayName"];
                bool IsLive = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsLive"]);

                MailMessage message = new MailMessage();
                string[] addresses = to.Split(';');
                foreach (string address in addresses)
                {
                    message.To.Add(new MailAddress(address));
                }

                if (string.IsNullOrEmpty(cc) == false)
                    message.CC.Add(new MailAddress(cc));

                if (IsLive == false)
                {
                    //message.Bcc.Add("example@gmail.com");
                    //message.Bcc.Add("example@gmail.com");
                }

                message.From = new MailAddress(SenderEmailAddress, DisplayName);
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;

                message.Attachments.Add(new Attachment(filepath));
                SmtpClient client = new SmtpClient();
                client.Host = SenderSMTPServer;
                if (Port > 0)
                    client.Port = Port;
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(SenderEmailAddress, SenderEmailPassword);
                client.EnableSsl = IsSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = nc;
                client.Send(message);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, ex.TargetSite.Name + " - " + ex.Message);
            }
        }

        public static void SendVerificationEmail(string FullName, string EmailAddress, string VerificationLink)
        {
            try
            {
                string smsg = GetTemplateString((int)Common.EmailTemplates.VerifyEmail);
                smsg = smsg.Replace("{User_Name}", FullName);
                smsg = smsg.Replace("{Verification_Link}", VerificationLink);
                SendMail(EmailAddress, "Account Verification Email", smsg, "");
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, ex.TargetSite.Name + " - " + ex.Message);
            }
        }

        public static string GetTemplateString(int templateCode)
        {
            StreamReader objStreamReader;
            string path = "";
            if (templateCode == (int)Common.EmailTemplates.ForgotPassword)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\forgotpassword.htm";
            }
            else if (templateCode == (int)Common.EmailTemplates.VerifyEmail)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\verifyemail.htm";
            }
            else if (templateCode == (int)Common.EmailTemplates.Activation)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\activation.htm";
            }
            else if (templateCode == (int)Common.EmailTemplates.ResetPassword)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\resetpassword.htm";
            }
            else if (templateCode == (int)Common.EmailTemplates.WelcomeEmail)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\WelcomeEmail.htm";
            }
            else if (templateCode == (int)Common.EmailTemplates.ResendPassword)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\resend_pw.html";
            }
            else if (templateCode == (int)Common.EmailTemplates.MassEmail)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\massemail.htm";
            }
            else if (templateCode == (int)Common.EmailTemplates.NewStudentAssignEmail)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\NewStudentAssignEmail.html";
            }
            else if (templateCode == (int)Common.EmailTemplates.WebinarEmail)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\CourseConfirmationWebinar.html";
            }
            else if (templateCode == (int)Common.EmailTemplates.EnrollmentNotificationEmail)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\EnrollmentNotificationOnline.html";
            }

            else if (templateCode == (int)Common.EmailTemplates.contracttemplate)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\ContractTemplate.html";
            }
            else if (templateCode == (int)Common.EmailTemplates.NewSingupRequest)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\NewUserRegister.htm";
            }
            else if (templateCode == (int)Common.EmailTemplates.Error)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"\Templates\Error.htm";
            }

            if (!string.IsNullOrEmpty(path))
            {
                objStreamReader = File.OpenText(path);
                string emailText = objStreamReader.ReadToEnd();
                objStreamReader.Close();
                objStreamReader = null;
                objStreamReader = null;
                return emailText;
            }
            else
            {
                objStreamReader = null;
                return string.Empty;
            }
        }

        public static void mainget()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + @"\UploadReport\Report.csv";
            String[] values = File.ReadAllLines(path);
        }

        public void SendReportMailWithAttachment(List<string> toBCC, string subject, string msg, string cc, string filepath)
        {
            try
            {
                string SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmailAddress"];
                string SenderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["SenderEmailPassword"];
                string SenderSMTPServer = System.Configuration.ConfigurationManager.AppSettings["SenderSMTPServer"];
                int Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);
                bool IsSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsSsl"]);
                string DisplayName = System.Configuration.ConfigurationManager.AppSettings["DisplayName"];
                bool IsLive = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsLive"]);

                MailMessage message = new MailMessage();
                foreach (string address in toBCC)
                {
                    message.Bcc.Add(new MailAddress(address));
                }

                if (string.IsNullOrEmpty(cc) == false)
                    message.CC.Add(new MailAddress(cc));

                message.From = new MailAddress(SenderEmailAddress, DisplayName);
                message.Subject = subject;
                message.Body = "<table align='left' width='100%' border='0' cellpadding='0' cellspacing='0' style='min-width:100%;font-family:Calibri;'><tr><td valign='top' style='padding:10px;text-align:center;'><img align='center' alt='' src='https://gallery.mailchimp.com/7d22cb976b6d0171c1702fad0/images/eaaa0f49-f9ae-411d-8f7c-7b6e129f9be6.png' width='300' style='max-width:300px; padding-bottom: 0; display: inline !important; vertical-align: bottom;'></td></tr><tr><td valign='top' style='padding:10px;text-align:center;'><p style='padding-bottom: 30px;'>Dear Sunburst User,<br /><br />New CSV report has been uploaded by " + msg + ",<br />There is an attachment that they have uploaded.<br /></p></td></tr><tr style='background: #272727; color: white;'><td valign='top' style='text-align:center; padding: 20px;'>Thank you<br><br><span style='font-size:22px'>Sunburst&nbsp;Plant Disease Clinic Inc</span><br><br>This email is system generated. Do not reply to this email.</td></tr></table>";
                message.IsBodyHtml = true;

                message.Attachments.Add(new Attachment(filepath));
                SmtpClient client = new SmtpClient();
                client.Host = SenderSMTPServer;
                if (Port > 0)
                    client.Port = Port;
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(SenderEmailAddress, SenderEmailPassword);
                client.EnableSsl = IsSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = nc;
                client.Send(message);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, ex.TargetSite.Name + " - " + ex.Message);
            }
        }

        public void SendReportMail(List<string> toBCC, string subject, string msg, string cc)
        {
            try
            {
                string SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmailAddress"];
                string SenderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["SenderEmailPassword"];
                string SenderSMTPServer = System.Configuration.ConfigurationManager.AppSettings["SenderSMTPServer"];
                int Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);
                bool IsSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsSsl"]);
                string DisplayName = System.Configuration.ConfigurationManager.AppSettings["DisplayName"];

                MailMessage message = new MailMessage();

                foreach (string address in toBCC)
                {
                    message.Bcc.Add(new MailAddress(address));
                }

                if (string.IsNullOrEmpty(cc) == false)
                    message.CC.Add(new MailAddress(cc));

                message.From = new MailAddress(SenderEmailAddress, DisplayName);
                message.Subject = subject;
                message.Body = "<table align='left' width='100%' border='0' cellpadding='0' cellspacing='0' style='min-width:100%;font-family:Calibri;'><tr><td valign='top' style='padding:10px;text-align:center;'><img align='center' alt='' src='https://gallery.mailchimp.com/7d22cb976b6d0171c1702fad0/images/eaaa0f49-f9ae-411d-8f7c-7b6e129f9be6.png' width='300' style='max-width:300px; padding-bottom: 0; display: inline !important; vertical-align: bottom;'></td></tr><tr><td valign='top' style='padding:10px;text-align:center;'><p style='padding-bottom: 30px;'>Dear Sunburst User,<br /><br />New report added manually by " + msg + "</p></td></tr><tr style='background: #272727; color: white;'><td valign='top' style='text-align:center; padding: 20px;'>Thank you<br><br><span style='font-size:22px'>Sunburst&nbsp;Plant Disease Clinic Inc</span><br><br>This email is system generated. Do not reply to this email.</td></tr></table>";
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Host = SenderSMTPServer;
                if (Port > 0)
                    client.Port = Port;
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(SenderEmailAddress, SenderEmailPassword);
                client.EnableSsl = IsSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = nc;
                client.Send(message);
            }
            catch (Exception)
            {
                return;
            }
        }
        
    }
}

