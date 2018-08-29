using System;

namespace Everbank.Web.Models
{
    public class LoginFormModel : PageModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}