using System.ComponentModel.DataAnnotations;

namespace JoulsePower.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required] 
        public string Username;

        [Required] 
        public string Password;

        public string[] Roles;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public User(int id, string username, string password, string[] roles)
        {
            this.Username = username;
            this.Password = password;
            this.Id = id;
            this.Roles = roles;
        }
    }
}