using Microsoft.EntityFrameworkCore;

namespace DBTest;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=/home/slavafggh/SQLite/MyFreezer.db;");
    }
}