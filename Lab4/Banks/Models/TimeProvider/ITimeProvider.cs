namespace Banks.Models.TimeProvider;

public interface ITimeProvider
{
    public void SetLastUpdate(DateTime newTime);
    public void ChangeTime(DateTime future);
}