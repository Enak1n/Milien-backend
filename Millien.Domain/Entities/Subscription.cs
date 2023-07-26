using Microsoft.EntityFrameworkCore;

namespace Millien.Domain.Entities
{
    [PrimaryKey("FollowerId", "FollowingId")]
    public class Subscription
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
    }
}
