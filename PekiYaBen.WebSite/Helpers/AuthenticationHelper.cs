using PekiYaBen.WebSite.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PekiYaBen.WebSite.Helpers
{
    public static class AuthenticationHelper
    {
        public static AppUser GetAuthenticatedUser()
        {
            PekiYaBenDBEntities entities = new PekiYaBenDBEntities();

            if (HttpContext.Current == null ||
                HttpContext.Current.Request == null ||
                !HttpContext.Current.Request.IsAuthenticated ||
                !(HttpContext.Current.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
            if (formsIdentity.Ticket == null)
                throw new ArgumentNullException("ticket");

            var user = entities.AppUsers.FirstOrDefault(m => m.Email == formsIdentity.Ticket.UserData);

            return user;
        }
    }
}