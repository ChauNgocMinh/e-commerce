using E_Commerce.Models;
using E_Commerce.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System;
using E_Commerce.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using PayPal.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PayPal.Api.OpenIdConnect;

namespace E_Commerce.Controllers
{
    public class BillController : Controller
    {
        public readonly IBillRepository _BillRepo;
        public readonly ICartRepository _CartRepo;
        public readonly IBookRepository _BookRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private readonly Random _random = new Random();
        private UserManager<ApplicationUser> _userManager;

        public BillController(IHttpContextAccessor httpContextAccessor, IBillRepository _billRepo, ICartRepository _cartRepo, UserManager<ApplicationUser> userManager, IBookRepository bookRepo)
        {
            _BillRepo = _billRepo;
            _CartRepo = _cartRepo;
            _userManager = userManager;
            _BookRepo = bookRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        [NonAction]
        public string RandomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder idBuilder = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                idBuilder.Append(chars[_random.Next(chars.Length)]);
            }
            return idBuilder.ToString();
        }
 
        
        
        [HttpPost]
        public async Task<IActionResult> CreateBill(string name, int total, string address, string note, string phone, string paymentMethods, string IdCartItem)
        {

            string email = HttpContext.User.Identity.Name;
            int pay;
            string idBill = RandomId();
            List<string> idItemlist = new List<string>(IdCartItem.Split(','));
            foreach (var idItem in idItemlist)
            {
                var a = await _CartRepo.UpdateCartItemAsync(email, idItem, idBill);
                await _BookRepo.ChageNumberSaleAsync(a.IdBook, a.Number);
            }
            
            if (paymentMethods == "cash")
            {
                pay = total;
            }
            else
            {
                pay = 0;
            }
            BillModel model = new BillModel
            {
                Id = idBill,
                Name = name,
                Email = email,
                BuyingDate = DateTime.Now,
                Total = total,
                Pay = pay,
                Address = address,
                Status = false,
                Note = note,
                PaymentMethods = paymentMethods,
                Phone = phone,
            };
            await _BillRepo.CreateBillAsync(model);
            
            if (paymentMethods == "cash")
            {
                return View(model);
            }
            if (paymentMethods == "paypal")
            {
                return PaymentWithPaypal(model);
            }
            else
            {
                return PaymentWithPaypal(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllBill()
        {
            string email = HttpContext.User.Identity.Name;
            return View(await _BillRepo.GetAllBillByEmailAsync(email));
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatusBill(string id)
        {
            string email = HttpContext.User.Identity.Name;
            await _BillRepo.UpdateBillAsync(id);
            return RedirectToAction(nameof(ShowAllBill));
        }

        public ActionResult FailureView()
        {
            return View();
        }
        public ActionResult SuccessView()
        {
            return View();
        }
        public ActionResult PaymentWithPaypal(BillModel model)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = HttpContext.Request.Query["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}/Bill/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, model);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    ISession session = _httpContextAccessor.HttpContext.Session;
                    //string guid = Guid.NewGuid().ToString();

                    session.SetString(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = HttpContext.Request.Query["guid"];
                    ISession session = _httpContextAccessor.HttpContext.Session;
                    var executedPayment = ExecutePayment(apiContext, payerId, session.GetString(guid));
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, BillModel model)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()   
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Đơn hàng " + model.Id.ToString(),
                currency = "USD",
                price = (model.Total / 23820).ToString(),
                quantity = "1",
                sku = "",
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = (model.Total / 23820).ToString(),
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (model.Total / 23820).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

    }
}
