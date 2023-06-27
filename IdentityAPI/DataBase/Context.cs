using Microsoft.EntityFrameworkCore;
using Milien_backend.Models;
using Npgsql;

namespace Milien_backend.DataBase
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) :
            base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        static Context() =>
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>();

        public DbSet<LoginDTO> Login { get; set; }
        public DbSet<UserDTO> Customers { get; set; }
    }
}
