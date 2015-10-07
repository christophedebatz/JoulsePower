using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace JoulsePower.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        private string _lastName;

        [Required]
        public string LastName
        {
            get { return this._lastName; }
            set { this._lastName = value.ToUpper(CultureInfo.CurrentCulture); }
        }

        [Required]
        public string EmailAddress { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="firstName">First name of the contact</param>
        /// <param name="lastName">Last name of the contact</param>
        /// <param name="emailAddress">Email address of the contact</param>
        /// <param name="company">Company if the contact</param>
        public Contact(int id, string firstName, string lastName, string emailAddress, Company company)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.Company = company;
        }
    }
}