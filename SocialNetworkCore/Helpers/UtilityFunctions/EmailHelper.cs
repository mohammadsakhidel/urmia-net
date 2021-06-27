using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetApp.Models;
using System.Net;
using System.Net.Mail;

namespace CoreHelpers
{
    public class EmailHelper
    {
        public static void SendEmail(string address, string subject, string message)
        {
            //MailMessage:
            MailAddress from = new MailAddress(EmailAccount.Email, Configs.SocialNetworkName, System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(address);
            MailMessage myMessage = new MailMessage(from, to);
            myMessage.Subject = subject;
            myMessage.IsBodyHtml = true;
            myMessage.Body = message;

            //SmtpClient
            SmtpClient mySmtpClient = new SmtpClient();
            System.Net.NetworkCredential myCredential = new System.Net.NetworkCredential(EmailAccount.Email, EmailAccount.Password);
            mySmtpClient.Host = EmailAccount.ProviderIP;
            mySmtpClient.UseDefaultCredentials = false;
            mySmtpClient.Credentials = myCredential;
            mySmtpClient.ServicePoint.MaxIdleTime = 1;

            //*************** logo image:
            AlternateView htmlview = default(AlternateView);
            htmlview = AlternateView.CreateAlternateViewFromString(message, null, "text/html");
            LinkedResource imageResources = new LinkedResource(HttpContext.Current.Server.MapPath(Urls.LogoNameIcon));
            imageResources.ContentId = "logo_name";
            imageResources.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            htmlview.LinkedResources.Add(imageResources);
            myMessage.AlternateViews.Add(htmlview);
            //***************

            //Send Async:
            object state = myMessage;
            mySmtpClient.SendCompleted += (s, e) => { 
                myMessage.Dispose();
                mySmtpClient.Dispose(); 
            };
            mySmtpClient.SendAsync(myMessage, state);
        }
        public static string GetRegisterationCodeEmailHtml(Member member)
        {
            string html = "";
            var content =
                Resources.Emails.RegisterationCodePhrase1 +
                "<b>" + member.RegistrationCode + "</b>" +
                Resources.Emails.RegisterationCodePhrase2 +
                "<a style=\"color: #0083d1; text-decoration: none;\" href=\"" + MyHelper.ResolveUrl(string.Format("/account/activation?email={0}&code={1}", member.Email, member.RegistrationCode), false) + "\">" +
                    Resources.Emails.RegisterationCodeLink +
                "</a>";
            html = GetEmailHtml(content);
            return html;
        }
        public static string GetPasswordRecoveryEmailHtml(string password)
        {
            string html = "";
            var content = Resources.Emails.PasswordRecoveryEmail +
                        "<div style=\"font-size: 10pt; font-weight: bold; padding: 5px 0 5px 0;\">" + password + "</div>";
            html = GetEmailHtml(content);
            return html;
        }
        public static string GetAlternativeEmaiActivationCodeEmailHtml(AccountSetting accSetting)
        {
            string html = "";
            var member = accSetting.Member;
            var content =
                Resources.Emails.AltEmailActivationCodePhrase1 +
                " " + member.FullName + " " +
                Resources.Emails.AltEmailActivationCodePhrase2 + 
                "<br />" +
                "<a style=\"color: #0083d1; text-decoration: none;\" href=\"" + MyHelper.ResolveUrl(string.Format("/account/AlternativeEmailActivation?email={0}&code={1}", member.Email, accSetting.AlternativeEmailActivationCode), false) + "\">" +
                    Resources.Emails.AltEmailActivationCodeLink +
                "</a>";
            html = GetEmailHtml(content);
            return html;
        }
        public static string GetGeneratedPasswordEmailHtml(string userName, string password)
        {
            string html = "";
            var content = Resources.Emails.GeneratedPasswordEmailPart1 +
                        "<div style=\"font-size: 10pt; font-weight: bold; margin: 10px 0; padding: 10px; background-color: #f1f2f2;\">" +
                            "Email: " + userName + "<br />" +
                            "Password: " + password +
                        "</div>" +
                        Resources.Emails.GeneratedPasswordEmailPart2 + "<br />" +
                        "<a href=\"" + MyHelper.ResolveUrl(string.Format("/account/login?u={1}", userName), false) + "\">" +
                            MyHelper.ResolveUrl(string.Format("/account/login?u={1}", userName), false) +
                        "</a>";
            html = GetEmailHtml(content);
            return html;
        }
        public static string GetGeneralEmailNotificationText(Member member)
        {
            string html = "";
            string content =
                Resources.Emails.ENotificationContentPart1 +
                "<a style=\"color: #0083d1; text-decoration: none;\" href=\"" + MyHelper.ResolveUrl(string.Format("/account/login?u={0}", member.Email), false) + "\">" +
                    Resources.Emails.LoginToYourAccount +
                "</a>";
            html = GetEmailHtml(content);
            return html;
        }
        #region Private Methods:
        private static string GetEmailHtml(string content)
        {
            string html = "";
            html =
                "<div style=\"direction: " + Resources.Words.Direction + "; margin: 20px; font-family: Tahoma; font-size: 9pt;\">" +
                    "<div style=\"background-color: #414042; padding: 10px 15px;\">" +
                        "<a href=\"" + MyHelper.ResolveUrl("", false) + "\">" +
                            "<img src=\"cid:logo_name\" alt=\"" + Configs.SocialNetworkName + "\" />" +
                        "</a>" +
                    "</div>" +
                    "<div style=\"padding: 20px; line-height: 25px; color: #414042; background-color: #ffffff;\">" +
                        "<div>" + 
                            content +
                        "</div>" + 
                        "<div style=\"text-align: center; width: 250px; padding-top: 20px;\">" +
                            Resources.Emails.EmailsFooter +
                        "</div>" + 
                    "</div>" +
                "</div>";
            return html;
        }
        #endregion
    }
}