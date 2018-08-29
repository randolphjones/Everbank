using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Everbank.Repositories.Contracts;

namespace Everbank.Web.Helpers
{
    public class SecurityHelper
    {

        public SecurityHelper()
        {

        }
        
        ///<summary>
        /// Signs in the current user's cookie
        ///</summary>
        public async Task SignInAsync(HttpContext httpContext, int userId, string emailAddress, string firstName)
        {
            ClaimsPrincipal claimsPrincipal = BuildClaimsPrincipal(userId, emailAddress, firstName);
            // AuthenticationProperties authenticationProperties = BuildAuthenticationProperties();
            await httpContext.SignInAsync(claimsPrincipal);
            //await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
        }

        ///<summary>
        /// Builds a Claims Principal object to used with Sign in
        ///</summary>
        private ClaimsPrincipal BuildClaimsPrincipal(int userId, string emailAddress, string firstName)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Email, emailAddress),
                new Claim("FirstName", firstName),
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(claimsIdentity);
        }

        ///<summary>
        /// Creates a user object from the provided ClaimsIdentity
        ///</summary>
        public User GetUserFromIdentity (ClaimsIdentity identity)
        {          
            User user = new User();

            foreach(Claim claim in identity.Claims)
            {
                switch(claim.Type)
                {
                    case "FirstName":
                        user.FirstName = claim.Value;
                        break;
                    case "UserId":
                        user.Id = Int32.Parse(claim.Value);
                        break;
                    case ClaimTypes.Email:
                        user.EmailAddress = claim.Value;
                        break;
                }

            }
            return user;
        }

        ///<summary>
        /// Signs out the current user's cookie
        ///</summary>
        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }
    }
}