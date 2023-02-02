namespace Backups.Extra.Models.Logger;

public interface ILogger
{
    void LogInformation(string? context, string message);
}