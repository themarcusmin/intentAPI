using Microsoft.EntityFrameworkCore;

namespace IntentAPI.Models;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(@"Host=host.docker.internal;Username=root;Password=root;Database=intent_db;Port=5432")
                      .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);

    public DbSet<User> Users { get; set; }

    public DbSet<Event> Events { get; set; }

    public DbSet<Recurring> Recurrings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Recurring>()
            .Property(e => e.RecurringMode)
            .HasConversion(
                v => v.ToString(),
                v => (RecurringMode)Enum.Parse(typeof(RecurringMode), v));
    }
}
