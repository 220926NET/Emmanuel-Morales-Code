using Microsoft.EntityFrameworkCore;
using projectOneApi_v1.Models;

namespace projectOneApi_v1.Data
{
    public class ApiDbContext : DbContext
    {

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        public DbSet<Logins> Logins { get; set; }

        public DbSet<Employees> Employees { get; set; }

        public DbSet<Tickets> Tickets { get; set; }
    }
}
