using Frontliners.Identity.InfraStructure.Interfaces;

namespace Frontliners.Identity.InfraStructure.Services;

public sealed class SmsService : INotificationService
{
    public Task SendMessagesAsync(string message, string to, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sending sms to {to} with message: {message}");
        
        return Task.CompletedTask;
    }
}