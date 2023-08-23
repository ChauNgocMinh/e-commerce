using E_Commerce.Models;
using MailChimp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MailChimp.Lists;
using MailChimp.Helper;

namespace E_Commerce.Controllers
{
    public class MailChimpController : Controller
    {
        private readonly MailChimpService _mailChimpService;

        public MailChimpController()
        {
            _mailChimpService = new MailChimpService();
        }

        public IActionResult SendEmailToUser()
        {
            string emailAddress = HttpContext.User.Identity.Name;
            _mailChimpService.SendEmail(emailAddress);
            return RedirectToAction("Books", "GetAll");
            // Redirect hoặc trả về thông báo thành công
        }
    }
}
