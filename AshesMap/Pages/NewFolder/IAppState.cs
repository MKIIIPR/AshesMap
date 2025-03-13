namespace AshesMap.Pages.NewFolder
{
    public interface IAppState
    {
        string Message { get; }
        bool Enabled { get; }
        int Counter { get; }

    }
}