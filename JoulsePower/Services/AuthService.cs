using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Caching;
using JoulsePower.Models;
using JoulsePower.Services.Repositories;

namespace JoulsePower.Services
{
    public class AuthService
    {
        protected UserRepository UserDao;

        private const int Ttl = 60;

        public AuthService()
        {
            this.UserDao = new UserRepository();   
        }

        public IDictionary<string, object> Login(string username, string password)
        {
            var user = this.UserDao.GetByUsernameAndPassword(username, password);

            if (user != null)
            {
                HttpContext.Current.User = new GenericPrincipal(
                    new GenericIdentity(user.Username),
                    user.Roles
                );

                var token = GenerateToken(user);
                RefreshUserCache(user, token);

                return new Dictionary<string, object>
                {
                    {"token", token},
                    {"expireAt", DateTime.Now.AddMinutes(Ttl).ToString(new DateTimeFormatInfo())},
                    {"user",
                        new Dictionary<string, object>
                        {
                            {"name", user.Username},
                            {"roles", user.Roles}
                        }
                    }
                };
            }

            return null;
        }

        private static string GenerateToken(User user)
        {
            var sha1Factory = new SHA1CryptoServiceProvider();
            var timestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var unhashed = Encoding.Default.GetBytes(string.Format("{0}{1}{2}", user.Id, user.Username, timestamp));
            var token = BitConverter.ToString(sha1Factory.ComputeHash(unhashed)).Replace("-", "");

            return token.ToLower(CultureInfo.CurrentCulture);
        }

        private static void RefreshUserCache(User user, string token)
        {
            HttpRuntime.Cache.Add(token, user, null, DateTime.Now.AddMinutes(Ttl), TimeSpan.Zero,
                CacheItemPriority.High, null);
        }
    }
}