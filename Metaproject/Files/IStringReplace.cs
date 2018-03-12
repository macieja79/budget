namespace Metaproject.Files
{
    public interface IStringReplace
    {
        string GetNewString(string oldString);
        bool IsSimulation { get; }
    }
}