using System;

namespace napper_be.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public int SessionTimeout { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SessionGUID { get; set; }
        
    }
}

