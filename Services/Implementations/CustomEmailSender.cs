using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Services.Implementations
{
    public class CustomEmailSender : ICustomEmailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<CustomEmailSender> _logger;

        public CustomEmailSender(IEmailSender emailSender, ILogger<CustomEmailSender> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return _emailSender.SendEmailAsync(email, subject, htmlMessage);
        }

        public async Task SendPasswordChangedNotificationAsync(string email)
        {
            try
            {
                var subject = "BrainSprint : Password Changed Notification";
                var message = $"Your password has been successfully changed on {DateTime.UtcNow.ToString("f")}. " +
                             "If you didn't make this change, please contact support immediately.";

                await _emailSender.SendEmailAsync(email, subject, message);
                _logger.LogInformation($"Password change notification sent to {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending password change notification to {email}");
                throw;
            }
        }
    }
}
