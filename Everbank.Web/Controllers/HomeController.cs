using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Everbank.Web.Models;
using Everbank.Service;
using Everbank.Service.Contracts;
using Everbank.Repositories.Contracts;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Everbank.Web.Helpers;
using System.Security.Claims;

namespace Everbank.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserService userService = new UserService();
        private TransactionService transactionService = new TransactionService();
        private DashboardHelper dashboardHelper = new DashboardHelper();
        private MessageHelper messageHelper = new MessageHelper();
        private SecurityHelper securityHelper = new SecurityHelper();
        private List<Message> messages = new List<Message>();

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login(LoginFormModel loginFormModel)
        {
            
            ServiceResponse response = userService.AuthenticateUser(loginFormModel.EmailAddress, loginFormModel.Password);
            messageHelper.AppendResponseMessages(messages, response);
            User user = response.ResponseObject as User;
            if (user != null)
            {
                // Ideally we would use async await here but the current behavior of this call does not behave as expected when awaited
                securityHelper.SignInAsync(HttpContext, user.Id, user.EmailAddress, user.FirstName).Wait();
                messageHelper.AddMessagesToSession(messages, HttpContext);
                return RedirectToAction("Dashboard");
            }
            else
            {
                loginFormModel.Messages = messages;
                return View("Index", loginFormModel);
            }
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(CreateAccountFormModel createAccountFormModel)
        {
            if (createAccountFormModel.Password != createAccountFormModel.PasswordConfirm)
            {
                Message errorMessage = new Message() {
                    Text = "Your password confirmation does not match. Please try again.",
                    Type = MessageType.ERROR,
                };
                createAccountFormModel.Messages = new List<Message> { errorMessage };
                return View(createAccountFormModel);
            }
            else
            {
                ServiceResponse userResponse = userService.CreateUser(createAccountFormModel.EmailAddress, createAccountFormModel.Password, createAccountFormModel.FirstName);
                User user = userResponse.ResponseObject as User;
                messageHelper.AppendResponseMessages(messages, userResponse);

                if (user != null)
                {
                    // Ideally we would use async await here but the current behavior of this call does not behave as expected when awaited
                    securityHelper.SignInAsync(HttpContext, user.Id, user.EmailAddress, user.FirstName).Wait();
                    messageHelper.AddMessagesToSession(messages, HttpContext);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    createAccountFormModel.Messages = messages;
                    return View(createAccountFormModel);
                }
            }
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            messageHelper.AppendMessagesFromSession(messages, HttpContext);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User user = securityHelper.GetUserFromIdentity(HttpContext.User.Identity as ClaimsIdentity);
                DashboardModel dashboardModel = dashboardHelper.BuildDashboardModel(user, messages);
                return View(dashboardModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult DepositFunds()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.IsDeposit = true;
                TransactionFormModel transactionFormModel = new TransactionFormModel() {
                    Messages = messages,
                };
                return View("Transaction", transactionFormModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DepositFunds(TransactionFormModel transactionFormModel)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (transactionFormModel.Amount <= 0)
                {
                    Message errorMessage = new Message() {
                        Text = "You cannot deposit zero or a negative amount. Please try again.",
                        Type = MessageType.ERROR,
                    };
                    transactionFormModel.Messages = new List<Message> { errorMessage };
                    ViewBag.IsDeposit = true;
                    return View("Transaction", transactionFormModel);
                }

                User user = securityHelper.GetUserFromIdentity(HttpContext.User.Identity as ClaimsIdentity);
                ServiceResponse response = transactionService.CreateTransaction(user.Id, transactionFormModel.Amount);
                messageHelper.AppendResponseMessages(messages, response);
                Transaction transaction = response.ResponseObject as Transaction;
                if (transaction != null)
                {
                    messageHelper.AddMessagesToSession(messages, HttpContext);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.IsDeposit = true;
                    transactionFormModel.Messages = messages;
                    return View("Transaction", transactionFormModel);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult WithdrawFunds()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.IsDeposit = false;
                TransactionFormModel transactionFormModel = new TransactionFormModel() {
                    Messages = messages,
                };
                return View("Transaction", transactionFormModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult WithdrawFunds(TransactionFormModel transactionFormModel)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (transactionFormModel.Amount <= 0)
                {
                    Message errorMessage = new Message() {
                        Text = "You cannot withdraw zero or a negative amount. Please try again.",
                        Type = MessageType.ERROR,
                    };
                    transactionFormModel.Messages = new List<Message> { errorMessage };
                    ViewBag.IsDeposit = false;
                    return View("Transaction", transactionFormModel);
                }

                User user = securityHelper.GetUserFromIdentity(HttpContext.User.Identity as ClaimsIdentity);
                ServiceResponse response = transactionService.CreateTransaction(user.Id, transactionFormModel.Amount * -1);
                messageHelper.AppendResponseMessages(messages, response);
                Transaction transaction = response.ResponseObject as Transaction;
                if (transaction != null)
                {
                    messageHelper.AddMessagesToSession(messages, HttpContext);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.IsDeposit = false;
                    transactionFormModel.Messages = messages;
                    return View("Transaction", transactionFormModel);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            securityHelper.SignOutAsync(HttpContext).Wait();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
