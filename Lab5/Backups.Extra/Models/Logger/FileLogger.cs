using System.Text;
using Backups.Extra.Tools;

namespace Backups.Extra.Models.Logger;

public class FileLogger : ILogger
{
    public FileLogger(string file, bool willTimeToLog)
    {
        if (string.IsNullOrWhiteSpace(file))
            throw new BackupsExtraException("Can't do logging with this file");
        FileName = file;
        TimeToLog = willTimeToLog;
    }

    public string FileName { get; }
    public bool TimeToLog { get; }

    public void LogInformation(string? context, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new BackupsExtraException("Can't be this message in log");

        using var writer = new StreamWriter(FileName, true);

        var builder = new StringBuilder();
        if (TimeToLog)
            builder.Append(TimeToLog);

        builder.Append(':').Append(message);
        writer.WriteLine(builder.ToString());
    }
}