using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using Emoda.PekiYaBen.Entity;

namespace Emoda.PekiYaBen.Web.App_Start
{
    public class ClaimsAuthorize : AuthorizeAttribute
    {
        public string[] claims { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var claimsIdentity = actionContext.Request.GetOwinContext().Authentication.User.Identity as ClaimsIdentity;

            if (!claims.All(x => claimsIdentity.Claims.Any(y => y.Type.Equals(x))))
            {
                throw new EmodaException("Bu işlem için yetkiniz bulunmamaktadır.");

                //return false;
            }

            //Continue with the regular Authorize check
            return base.IsAuthorized(actionContext);
        }
    }
}