using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using JoulsePower.Services;

namespace JoulsePower.Controllers
{
    public class AuthController : ApiController
    {
        protected AuthService AuthService;

        public AuthController()
        {
            this.AuthService = new AuthService();
        }

        [ActionName("Login")]
        public HttpResponseMessage Login(HttpRequestMessage requestMsg)
        {
            var request = new HttpContextWrapper(HttpContext.Current).Request;

            if (request.Form["username"] == null || request.Form["password"] == null)
            {
                return requestMsg.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    new ArgumentException("Unable to find username or password.")
                );
            }

            var loginDictionnary = this.AuthService.Login(
                request.Form["username"], 
                request.Form["password"]
            );

            if (loginDictionnary != null)
            {
                return requestMsg.CreateResponse(
                    HttpStatusCode.OK,
                    loginDictionnary
                );
            }

            return requestMsg.CreateErrorResponse(
                HttpStatusCode.Forbidden,
                "Wrong credentials"
            );
        }
    }
}