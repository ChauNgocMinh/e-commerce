using System.Net.Mail;
using System.Net;
using System.Text;
using MailKit.Security;
using MimeKit;
using Org.BouncyCastle.Utilities.Net;
using System;

namespace E_Commerce.Helper
{
    public class SendEmail
    {
        private readonly Random _random = new Random();

    
        
        public void ActivateAccount(string toAddress)
        {
            string urlFriendlyString = WebUtility.UrlEncode(toAddress);
            string code = "https://localhost:7147/Controller/Confirm?Email=" + urlFriendlyString;

            // email và password người gửi  
            string fromMail = "chaungocminh202@gmail.com";
            string fromPassword = "clzabmghjapybojg";

            // Nội dung thư
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Validate registration";
            message.To.Add(new MailAddress(toAddress));

            message.Body = "<h1>Click on the link to activate your account: </h1>" +  "<a href = \" " + code +  "  \"> Chuyển đến trang chủ </a>  ";
 
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
        public void ChagePassWord(string toAddress)
        {
            string urlFriendlyString = WebUtility.UrlEncode(toAddress);
            string Link = "https://localhost:7147/Controller/ConfiPassword?Email=" + urlFriendlyString;

            // email và password người gửi  
            string fromMail = "chaungocminh202@gmail.com";
            string fromPassword = "clzabmghjapybojg";

            // Nội dung thư
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Change PassWord";
            message.To.Add(new MailAddress(toAddress));

            message.Body = "<h1>Click on the link to rest your password: </h1>" + "<a href = \" " + Link + "  \"> Chuyển đến trang chủ </a>  ";

            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}

