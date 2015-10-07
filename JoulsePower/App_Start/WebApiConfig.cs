using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using JoulsePower.Controllers.Formatter;
using Newtonsoft.Json.Serialization;

namespace JoulsePower
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var jsonFormatter = new JsonFormatter
            {
                SerializerSettings = {ContractResolver = new CamelCasePropertyNamesContractResolver()}
            };

            config.Formatters.Add(jsonFormatter);

            config.Routes.MapHttpRoute(
                name: "ContactCompany",
                routeTemplate: "api/contacts/{id}/company",
                defaults: new { controller = "Contact", action = "GetContactCompany" },
                constraints: new { id = "\\d+", httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                name: "DeleteContact",
                routeTemplate: "api/contacts/{id}",
                defaults: new { controller = "Contact", action = "Delete" },
                constraints: new { id = "\\d+", httpMethod = new HttpMethodConstraint(HttpMethod.Delete) }
            );

            config.Routes.MapHttpRoute(
                name: "GetContact",
                routeTemplate: "api/contacts/{id}",
                defaults: new { controller = "Contact", action = "GetSingle" },
                constraints: new { id = "\\d+", httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                name: "GetAllContacts",
                routeTemplate: "api/contacts",
                defaults: new { controller = "Contact", action = "GetAll" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                name: "PostContact",
                routeTemplate: "api/contacts",
                defaults: new { controller = "Contact", action = "Post" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            config.Routes.MapHttpRoute(
                name: "Login",
                routeTemplate: "api/token",
                defaults: new { controller = "Auth", action = "Login" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            config.Routes.MapHttpRoute(
                name: "PutContact",
                routeTemplate: "api/contacts",
                defaults: new { controller = "Contact", action = "Put" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) }
            );
        }
    }
}
