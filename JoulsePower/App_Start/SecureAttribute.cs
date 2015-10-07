using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using JoulsePower.Models;

namespace JoulsePower
{
    public class SecureAttribute : AuthorizationFilterAttribute 
    {
        public User CurrentUser { get; protected set; }

        public string Roles { get; set; }

        public const string CacheKey = "UserTokenStore";

        public const string HeaderTokenName = "X-AUTH";

        protected Cache Cache;


        public SecureAttribute()
        {
            this.Cache = HttpRuntime.Cache;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (!this.Authorize(actionContext.Request))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Forbidden, 
                    new UnauthorizedAccessException("Access denied.")
                );
            }
        }

        private bool Authorize(HttpRequestMessage request)
        {
            string givenToken = null;

            try
            {
                givenToken = request.Headers.GetValues(HeaderTokenName).First();
            }
            catch (InvalidOperationException ex) {
                System.Diagnostics.Debug.WriteLine("Given token was not in cache.");
            }

            if (givenToken != null && this.Cache.Get(givenToken) != null)
            {
                this.CurrentUser = (User) this.Cache.Get(givenToken);
                var rolesArray = Regex.Replace(this.Roles, @"\s+", "").Split(',');

                if (this.CurrentUser.Roles.Intersect(rolesArray).Count() == rolesArray.Length)
                {
                    this.Cache.Remove(givenToken);
                    this.Cache.Add(givenToken, this.CurrentUser, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero,
                        CacheItemPriority.High, null);

                    return true;
                }
            }
            
            return false;
        }
    }
}