using System.Text.Json.Serialization;

namespace napper_be.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string HashedPassword { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public byte[] Salt { get; set; }

    }
}