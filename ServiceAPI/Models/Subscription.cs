﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ServiceAPI.Models
{
    [PrimaryKey("FollowerId", "FollowingId")]
    public class Subscription
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public List<string> Notifications { get; set; } = new();

        public Subscription(int followerId, int followingId)
        {
            FollowerId = followerId;
            FollowingId = followingId;
        }
    }
}