using Capstone.Controllers;
using Capstone.DAO;
using Capstone.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Capstone.Logic
{
    public class EmailSender
    {

        private ICookoutDao CookoutDao = new CookoutSqlDao("Server=.\\SQLEXPRESS;Database=final_capstone;Trusted_Connection=True;");
        private IInviteDao InviteDao = new InviteSqlDao("Server=.\\SQLEXPRESS;Database=final_capstone;Trusted_Connection=True;");
        private IUserDao UserDao = new UserSqlDao("Server=.\\SQLEXPRESS;Database=final_capstone;Trusted_Connection=True;");

        public void SendEmail(string emailAddress, int cookoutId, int inviteId)
        {
            Cookout thisCookout = CookoutDao.GetCookoutByCookoutId(cookoutId);
            string fromMail = "mcavinuejon@gmail.com";
            string fromPassword = "rxyamyuqaxshgtab";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "You're invited!";
            message.To.Add(new MailAddress(emailAddress));
            message.Body = $"<html><body> You're invited to the next upcoming cookout, {thisCookout.CookoutName}. Please RSVP by clicking the provided link: http://localhost:8080/cookout/{thisCookout.CookoutID}/rsvp/{inviteId}" +
                $"<br>{thisCookout.Description}</body></html>";
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
