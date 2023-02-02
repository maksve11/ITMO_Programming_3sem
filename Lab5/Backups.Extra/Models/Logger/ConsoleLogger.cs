using System.Text;
using Backups.Extra.Tools;

namespace Backups.Extra.Models.Logger;

public class ConsoleLogger : ILogger
{
    public ConsoleLogger(bool willTimeToLog)
    {
        TimeToLog = willTimeToLog;
    }

    public bool TimeToLog { get; }

    public void LogInformation(string? context, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new BackupsExtraException("Can't be this message in log");

        var builder = new StringBuilder();
        if (TimeToLog)
            builder.Append(TimeToLog);

        builder.Append(':').Append(message);
        Console.WriteLine(builder.ToString());
    }
}