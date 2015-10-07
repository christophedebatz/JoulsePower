using JoulsePower.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JoulsePower.Services.Repositories;

namespace JoulsePower.Controllers
{
    [Secure(Roles="user")]
    [HandleException]
    public class ContactController : ApiController
    {
        protected ContactRepository ContactDao;

        public ContactController()
        {
            this.ContactDao = new ContactRepository();
        }

        [ActionName("GetAll")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            var result = this.ContactDao.GetAll();

            return result != null
                ? request.CreateResponse(HttpStatusCode.OK, result)
                : request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to find any contact");
        }

        [ActionName("GetSingle")]
        public HttpResponseMessage GetSingle(HttpRequestMessage request, int id)
        {
            var result = this.ContactDao.GetById(id);

            return result == null
                ? request.CreateErrorResponse(HttpStatusCode.NotFound, "Contact not found")
                : request.CreateResponse(HttpStatusCode.OK, result);
        }

        [ActionName("Post")]
        public HttpResponseMessage Post(HttpRequestMessage request, Contact contact)
        {
            return this.ContactDao.Insert(contact)
                ? request.CreateResponse(HttpStatusCode.NoContent)
                : request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to add contact");
        }

        [ActionName("Put")]
        [Secure(Roles = "admin")]
        public HttpResponseMessage Put(HttpRequestMessage request, Contact contact)
        {
            return this.ContactDao.Update(contact)
                ? request.CreateResponse(HttpStatusCode.NoContent)
                : request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to update contact");
        }

        [ActionName("Delete")]
        [Secure(Roles = "admin")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return this.ContactDao.Delete(id) 
                ? request.CreateResponse(HttpStatusCode.NoContent)
                : request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to add contact");
        }

        [ActionName("GetContactCompany")]
        public HttpResponseMessage GetContactCompany(HttpRequestMessage request, int id)
        {
            var result = this.ContactDao.GetById(id);

            if (result == null) {
                return request.CreateErrorResponse(HttpStatusCode.NotFound, "Contact not found.");
            }

            return result.Company != null 
                ? request.CreateResponse(HttpStatusCode.OK, result.Company) 
                : request.CreateErrorResponse(HttpStatusCode.NotFound, "This contact has no company");
        }
    }
}