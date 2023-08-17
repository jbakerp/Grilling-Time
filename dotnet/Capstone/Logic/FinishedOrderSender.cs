using Capstone.Controllers;
using Capstone.DAO;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Capstone.Logic
{
    public class FinishedOrderSender
    {
        public void SendFinishedOrderMail(string emailAddress)
        {
            string fromMail = "mcavinuejon@gmail.com";
            string fromPassword = "rxyamyuqaxshgtab";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Order finished";
            message.To.Add(new MailAddress(emailAddress));
            message.Body = "<html><body>Your order is now completed! Please see the chef to pick up your food</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail, fromPassword),
            };

            smtpClient.Send(message);
        }
    }
}
