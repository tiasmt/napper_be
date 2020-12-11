using napper_be.Entities;

namespace napper_be.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.Name;
            Username = user.Username;
            Email = user.Email;
            Token = token;
        }
    }
}