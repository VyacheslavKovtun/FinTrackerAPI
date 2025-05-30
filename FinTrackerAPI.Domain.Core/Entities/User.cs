using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackerAPI.Domain.Core.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? APIToken { get; set; }
        public string PasswordHash { get; set; }
        public string? PreferredCurrencyCode { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public string? EmailConfirmationToken { get; set; }
    }
}
