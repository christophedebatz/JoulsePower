using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace JoulsePower.Models
{
    public class Company
    {
        [Required]
        public int Id { get; set; }

        private string _name;

        [Required]
        public string Name
        {
            get { return _name; }
            set { this._name = value.ToUpper(CultureInfo.CurrentCulture); }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { this._city = value.ToUpper(CultureInfo.CurrentCulture); }
        }

        public string Picture { get; set; }
    
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Company(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="city"></param>
        public Company(int id, string name, string city)
        {
            this.Id = id;
            this.Name = name;
            this.City = city;
        }
    }
}