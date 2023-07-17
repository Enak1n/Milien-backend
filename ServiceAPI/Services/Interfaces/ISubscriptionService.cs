namespace ServiceAPI.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task Subscribe(int followerId, int followingId);
        Task Unsubscribe(int followerId, int followingId);
    }
}
