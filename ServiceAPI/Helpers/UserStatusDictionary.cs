using System.Collections.Concurrent;

namespace ServiceAPI.Helpers
{
    public class UserStatusDictionary
    {
        private static readonly ConcurrentDictionary<int, bool> _userStatuses = new ConcurrentDictionary<int, bool>();

        public static void SetUserStatus(int userId, bool isOnline)
        {
            _userStatuses.AddOrUpdate(userId, isOnline, (_, value) => isOnline);
        }

        public static bool GetUserStatus(int userId)
        {
            return _userStatuses.TryGetValue(userId, out var isOnline) && isOnline;
        }

        public static void RemoveUser(int userId)
        {
            _userStatuses.TryRemove(userId, out _);
        }
    }
}
