﻿using Microsoft.EntityFrameworkCore;

namespace MilienAPI.Models
{
    [PrimaryKey("FollowerId", "FollowingId")]
    public class Subscription
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
    }
}
