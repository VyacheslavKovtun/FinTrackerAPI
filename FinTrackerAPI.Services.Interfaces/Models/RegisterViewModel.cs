﻿namespace FinTrackerAPI.Services.Interfaces.Models
{
    public class RegisterViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PreferredCurrencyCode { get; set; }
    }
}
