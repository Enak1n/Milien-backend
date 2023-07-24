using Microsoft.EntityFrameworkCore;
using Npgsql;
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
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>();
        }

        public DbSet<PaidAd> PaidAds { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}