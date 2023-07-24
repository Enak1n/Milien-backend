using Microsoft.EntityFrameworkCore;
using MilienAPI.Models;
using Npgsql;

namespace MilienAPI.DataBase
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) :
            base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>();
        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PaidAd> PaidAds { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
