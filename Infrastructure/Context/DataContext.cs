using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DataContext() {}

        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Visiting> Visitings => Set<Visiting>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Visiting>(u => u.Visitings)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
