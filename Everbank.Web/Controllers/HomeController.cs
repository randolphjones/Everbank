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
using Everbank.Web.Helpers;

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
            // TODO: Check security and redirect to Dashboard if logged in
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginFormModel loginFormModel)
        {
            
            ServiceResponse response = userService.AuthenticateUser(loginFormModel.EmailAddress, loginFormModel.Password);
            MessageHelper.AppendResponseMessages(messages, response);
            User user = response.ResponseObject as User;
            if (user != null)
            {
                // TODO: Replace this with a redirect
                DashboardModel dashboardModel = DashboardHelper.BuildDashboardModel(user, messages);
                ViewBag.Messages = messages;
                return View("Dashboard", dashboardModel);
            }
            else
            {
                return RedirectToAction("Index");
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
                    // TODO: Replace this with a redirect
                    DashboardModel dashboardModel = DashboardHelper.BuildDashboardModel(user, messages);
                    ViewBag.Messages = messages;
                    return View("Dashboard", dashboardModel);
                }
                else
                {
                    ViewBag.Messages = messages;
                    return View();
                }
            }
        }

        public IActionResult Dashboard()
        {
            //TODO: Enforce Security
            return View();
        }

        [HttpGet]
        public IActionResult DepositFunds()
        {
            ViewBag.IsDeposit = true;
            return View("Transaction");
        }

        [HttpPost]
        public IActionResult DepositFunds(TransactionFormModel transactionFormModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult WithdrawFunds()
        {
            ViewBag.IsDeposit = false;
            return View("Transaction");
        }

        [HttpPost]
        public IActionResult WithdrawFunds(TransactionFormModel transactionFormModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // TODO: Clear session info and redirect to index
            throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
