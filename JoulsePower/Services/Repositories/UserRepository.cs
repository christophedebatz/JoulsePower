using System;
using JoulsePower.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace JoulsePower.Services.Repositories
{
    public class UserRepository
    {
        private const string CacheKey = "UserStore";

        protected Cache Cache;

        protected IEnumerable<User> Users = new [] 
        {
            new User(1, "magikorion", "attractive", new [] { "user" }),
            new User(2, "fafnir", "attractive", new [] { "user", "admin" }),
            new User(3, "pushreset", "attractive", new [] { "user" }), 
            new User(4, "joulse", "attractive", new [] { "user" }),
            new User(5, "dkay", "attractive", new [] { "user" }),
        };

        /// <summary>
        /// Ctor
        /// </summary>
        public UserRepository()
        {
            this.Cache = HttpRuntime.Cache;

            if (this.Cache == null) return;

            if (this.Cache[CacheKey] == null) {
                this.Cache[CacheKey] = this.Users;
            }
        }

        /// <summary>
        /// Get user from username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetByUsernameAndPassword(string username, string password)
        {
            var sample = (IEnumerable<User>) this.Cache[CacheKey];

            try
            {
                return sample.First(user => user.Username.Equals(username) && user.Password.Equals(password));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}