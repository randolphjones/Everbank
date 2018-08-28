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
            MessageHelper.AppendResponseMessages(messages, response);
            User user = response.ResponseObject as User;
            if (user != null)
            {
                // Ideally we would use async await here but the current behavior of this call is unpredictable
                SecurityHelper.SignInAsync(HttpContext, user.Id, user.EmailAddress, user.FirstName).Wait();
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Messages = messages;
                return View("Index");
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
                ViewBag.Messages = new List<Message> { errorMessage };
                return View();
            }
            else
            {
                ServiceResponse userResponse = userService.CreateUser(createAccountFormModel.EmailAddress, createAccountFormModel.Password, createAccountFormModel.FirstName);
                User user = userResponse.ResponseObject as User;
                MessageHelper.AppendResponseMessages(messages, userResponse);

                if (user != null)
                {
                    // Ideally we would use async await here but the current behavior of this call is unpredictable
                    SecurityHelper.SignInAsync(HttpContext, user.Id, user.EmailAddress, user.FirstName).Wait();
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.Messages = messages;
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User user = SecurityHelper.GetUserFromIdentity(HttpContext.User.Identity as ClaimsIdentity);
                DashboardModel dashboardModel = DashboardHelper.BuildDashboardModel(user, messages);
                ViewBag.Messages = messages;
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
                return View("Transaction");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DepositFunds(TransactionFormModel transactionFormModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult WithdrawFunds()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.IsDeposit = false;
                return View("Transaction");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult WithdrawFunds(TransactionFormModel transactionFormModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            SecurityHelper.SignOutAsync(HttpContext).Wait();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
