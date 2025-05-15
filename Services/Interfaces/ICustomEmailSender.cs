
namespace Services.Interfaces
{
    public interface ICustomEmailSender : IEmailSender
    {
        Task SendPasswordChangedNotificationAsync(string email);
    }
}
