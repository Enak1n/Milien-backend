using Microsoft.EntityFrameworkCore;
using MilienAPI.Models;
using Npgsql;
using System.Data;

namespace MilienAPI.DataBase
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) :
            base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public DbSet<Ad> Ads { get; set; }
    }
}
