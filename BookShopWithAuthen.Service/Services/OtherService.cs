using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace BookShopWithAuthen.Service.Services
{
    public class OtherService
    {
        public OtherService()
        {
           
        }
        public static void SendMail(string emailTo, string subject, string message)
        {
            MailMessage mail = new MailMessage("dtbookcompany@gmail.com", emailTo)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = message
            };
            string emailAddress = ConfigurationManager.AppSettings["emailAddress"];
            string password = ConfigurationManager.AppSettings["passwordEmailAddress"];
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(emailAddress, password)
            };

            client.Send(mail);
        }
    }
}
