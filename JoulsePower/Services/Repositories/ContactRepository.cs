using JoulsePower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace JoulsePower.Services.Repositories
{
    /// http://weblogs.asp.net/pjohnson/httpruntime-cache-vs-httpcontext-current-cache
    
    public class ContactRepository
    {
        private const string CacheKey = "ContactStore";

        protected Cache Cache;

        protected IEnumerable<Contact> Contacts = new [] 
        {
            new Contact(1, "Christophe", "DE BATZ", "christophe.debatz@thalesgroup.com", new Company(1, "thales", "Elancourt")),
            new Contact(2, "Julien", "DUCROT", "julien.ducrot@live.fr", new Company(2, "attractive world", "Paris")),
            new Contact(3, "Marc", "GHORAYEB", "marc.ghorayeb@outlook.com", new Company(2, "attractive world", "Paris")),
            new Contact(4, "Patrick", "HARRIS", "harris.patrick@yahoo.fr", new Company(3, "Hachette", "Montreuil"))
        };

        /// <summary>
        /// Ctor
        /// </summary>
        public ContactRepository()
        {
            this.Cache = HttpRuntime.Cache;

            if (this.Cache == null) return;

            if (this.Cache[CacheKey] == null) {
                this.Cache[CacheKey] = this.Contacts;
            }
        }

        /// <summary>
        /// Get all contacts from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Contact> GetAll()
        {
            return (IEnumerable<Contact>)this.Cache[CacheKey];
        }

        /// <summary>
        /// Get the contact by its Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Contact GetById(int Id)
        {
            var sample = this.GetAll();

            try
            {
                return sample.First(contact => contact.Id == Id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Save contact into request Cache.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public bool Insert(Contact contact)
        {
            if (this.Cache == null) return false;

            try
            {
                var currentContacts = (IEnumerable<Contact>) this.Cache[CacheKey];
                this.Cache[CacheKey] = currentContacts.Concat(new[] { contact });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to insert new contact. " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Remove a contact from the request Cache.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            if (this.Cache == null) return false;

            try
            {
                var currentContacts = ((IEnumerable<Contact>) this.Cache[CacheKey]).ToList();
                var finalContacts = currentContacts.Where(contact => contact.Id != id);
                this.Cache[CacheKey] = finalContacts.ToArray();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove contact." + ex.Message);
                return false;
            }
        }
        
        /// <summary>
        /// Update a contact from the request Cache.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public bool Update(Contact contact)
        {
            return this.Delete(contact.Id) && this.Insert(contact);
        }
    }
}