namespace Quik_BookingApp.Repos.Interface
{
    public interface IRefreshHandler
    {
        Task<string> GenerateToken(string username);
    }
}
