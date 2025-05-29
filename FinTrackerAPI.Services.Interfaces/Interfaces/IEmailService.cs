using FinTrackerAPI.Infrastructure.Business.DTO;
using FinTrackerAPI.Services.Interfaces.Models;

namespace FinTrackerAPI.Services.Interfaces.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailConfirmationAsync(string toEmail, string confirmationLink);
    }
}
