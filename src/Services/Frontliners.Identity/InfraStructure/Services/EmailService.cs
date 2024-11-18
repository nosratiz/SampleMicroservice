using Frontliners.Identity.InfraStructure.Interfaces;

namespace Frontliners.Identity.InfraStructure.Services;

public sealed class EmailService : INotificationService
{
    public Task SendMessagesAsync(string message, string to, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sending email to {to} with message: {message}");
        
        return Task.CompletedTask;
    }
}