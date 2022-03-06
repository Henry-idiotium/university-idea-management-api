using System;
using UIM.Core.Common;

namespace UIM.Core.Models.Entities
{
    public class RefreshToken : Entity<int>
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string ReplacedByToken { get; set; }
        public string ReasonRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiredDate;
        public bool IsRevoked => RevokedDate != null;
        public bool IsActive => !IsRevoked && !IsExpired;

        // Referential Integrity
        public AppUser User { get; set; }
    }
}