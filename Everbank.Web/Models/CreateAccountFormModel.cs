using System;

namespace Everbank.Web.Models
{
    public class CreateAccountFormModel
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}