namespace Frontliners.Identity.InfraStructure.Interfaces;

public interface INotificationService
{
    Task SendMessagesAsync(string message,string To,CancellationToken cancellationToken);
}