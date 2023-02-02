using Banks.Tools;

namespace Banks.Models.TimeProvider;

public class TimeProvider : ITimeProvider
{
    public TimeProvider(DateTime currentTime, DateTime lastUpdate)
    {
        CurrentTime = currentTime;
        LastUpdate = lastUpdate;
    }

    public DateTime LastUpdate { get; private set; }
    public DateTime CurrentTime { get; private set; }

    public void SetLastUpdate(DateTime newTime)
    {
        LastUpdate = newTime;
    }

    public void ChangeTime(DateTime future)
    {
        if (future < CurrentTime)
            throw new BanksException("Incorrect Date Time to change");
        LastUpdate = CurrentTime;
        CurrentTime = future;
    }
}