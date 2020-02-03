using MVC.CustomAuth;
using MVC.Models;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected FormsAuthenticationTicket GetAuthTicket()
        {
            HttpCookie authCookie = Request.Cookies["Cookie25"];
            if (authCookie == null) return null;
            try
            {
                return FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception exception)
            {
                //errorLog.Write("Can't decrypt cookie! {0}", exception.Message);
                return null;
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authTicket = GetAuthTicket();
            if (authTicket != null)
            {

                var serializeModel = JsonConvert.DeserializeObject<UserSerializeModel>(authTicket.UserData);

                CustomPrincipal principal = new CustomPrincipal(authTicket.Name)
                {
                    UserId = serializeModel.UserId,
                    Nickname = serializeModel.Nickname,
                };
                HttpContext.Current.User = principal;
            }

        }
    }
}
