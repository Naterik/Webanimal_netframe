using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetFramwork_WildNature.Db;
using PayPal.Api;

namespace NetFramwork_WildNature.Controllers
{
    public class DonateController : Controller
    {
        private readonly WildNature _context = new WildNature();

        // GET: Donate
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login", new { area = "Admin" });
            }
            return View(new Donate());
        }

        public ActionResult Checkout()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(Donate model)
        {
            if (ModelState.IsValid)
            {
                var user = Session["user"] as Account;
                model.AccountID = user.ID; 
                Session["Donate"] = new List<Donate> { model }; 
                return RedirectToAction("PaymentWithPaypal");
            }
            return View("Index", model);
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {

            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Donate/PaymentWithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() == "approved")
                    {
                        SaveDonationData();
                        return View("SuccessView");
                    }
                    else
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception)
            {
                return View("FailureView");
            }
        }

        private Payment payment;

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

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listDonate = Session["Donate"] as List<Donate>;
            if (listDonate == null)
            {
                throw new Exception("No donations found in session.");
            }

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var donate in listDonate)
            {
                var account = _context.Accounts.Find(donate.AccountID);
                if (account != null)
                {
                    itemList.items.Add(new Item()
                    {
                        name = account.Name,
                        currency = "USD",
                        price = donate.Amount?.ToString("F2") ?? "0.00",
                        quantity = "1",
                        sku = donate.ID.ToString()
                    });
                }
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var totalAmount = listDonate.Sum(d => d.Amount) ?? 0.0;
            var amount = new Amount()
            {
                currency = "USD",
                total = totalAmount.ToString("F2")
            };

            var transactionList = new List<Transaction>
            {
                new Transaction()
                {
                    description = $"Invoice #{DateTime.Now.Ticks}",
                    invoice_number = DateTime.Now.Ticks.ToString(),
                    amount = amount,
                    item_list = itemList
                }
            };

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return this.payment.Create(apiContext);
        }

        private void SaveDonationData()
        {
            var listDonate = Session["Donate"] as List<Donate>;
            if (listDonate != null)
            {
                foreach (var donate in listDonate)
                {
                    donate.Date = DateTime.Now;
                    donate.State = true; // Đánh dấu trạng thái là đã hoàn tất
                    _context.Donates.Add(donate);
                }
                _context.SaveChanges();
            }
        }

        public ActionResult FailureView()
        {
            return View();
        }

        public ActionResult SuccessView()
        {
            return View();
        }
    }
}
