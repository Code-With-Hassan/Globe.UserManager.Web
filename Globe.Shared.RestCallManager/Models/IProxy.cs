namespace Globe.Shared.RestCallManager.Models
{
    public interface IProxy
    {
        string Url { get; }
        bool IsPublic { get; }
    }
}
