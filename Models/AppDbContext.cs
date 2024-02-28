using Microsoft.EntityFrameworkCore;

namespace IntentAPI.Models;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(@"Host=postgres;Username=root;Password=root;Database=intent_db");

    public DbSet<User> Users { get; set; }
}
