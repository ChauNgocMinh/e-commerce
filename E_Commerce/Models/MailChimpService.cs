using MailChimp;
using MailChimp.Helper;
using MailChimp.Lists;
using System;

namespace E_Commerce.Models
{
    public class MailChimpService
    {
        private readonly string _apiKey = "38f23a629fbceb10b00273511b7cc8ba-us21";

        public void SendEmail(string emailAddress)
        {
            var listId = "YOUR_MAILCHIMP_LIST_ID"; // Thay bằng List ID của bạn
            var mailChimpManager = new MailChimpManager(_apiKey);

            //var mergeFields = new { FNAME = "John", LNAME = "Doe" }; // Thay bằng các trường cần thêm

            var emailParameter = new EmailParameter
            {
                Email = emailAddress,
                // EUId và LEId có thể được thiết lập theo yêu cầu
            };

            mailChimpManager.Subscribe(listId, emailParameter);
        }
    }
}
