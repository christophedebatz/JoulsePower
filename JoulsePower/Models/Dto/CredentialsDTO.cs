namespace JoulsePower.Models.Dto
{
    public class CredentialsDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public CredentialsDto(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}