using Backups.Tools;

namespace Backups.Models;

public class Backup
{
    private readonly List<RestorePoint> _restorePoints;
    public Backup(List<RestorePoint> restorePoints)
    {
        if (restorePoints.Count == 0)
            throw new BackupsException("in Backup can't be 0 restore points");
        _restorePoints = restorePoints;
    }

    public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints.AsReadOnly();

    public void AddRestorePoint(RestorePoint newPoint)
    {
        if (_restorePoints.Contains(newPoint))
            throw new BackupsException("there's this restore point yet");
        _restorePoints.Add(newPoint);
    }

    public void RemoveRestorePoint(RestorePoint point)
    {
        if (!_restorePoints.Contains(point))
            throw new BackupsException("there's no this restore point in backup");
        _restorePoints.Remove(point);
    }
}