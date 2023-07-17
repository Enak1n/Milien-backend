using Microsoft.EntityFrameworkCore;
using ServiceAPI.Models;
using System.Collections.Generic;

namespace ServiceAPI.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public DbSet<PaidAd> PaidAds { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}