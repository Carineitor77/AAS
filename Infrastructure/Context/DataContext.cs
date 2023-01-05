using Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System;

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
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Name).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.PasswordSalt).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.RoleId).IsRequired();


            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, Name = "director" },
                new Role() { Id = 2, Name = "teacher" },
                new Role() { Id = 3, Name = "student" }
            );

            modelBuilder.Entity<Lesson>().HasData(
                new Lesson()
                {
                    Id = 1,
                    Name = "english",
                    Description = "English lesson",
                    StartDate = new DateTime(2023, 4, 1, 7, 0, 0),
                    EndDate = new DateTime(2023, 4, 1, 10, 0, 0),
                    IsTaken = false
                },
                new Lesson()
                {
                    Id = 2,
                    Name = "math",
                    Description = "Math lesson",
                    StartDate = new DateTime(2023, 6, 1, 4, 0, 0),
                    EndDate = new DateTime(2023, 6, 1, 6, 0, 0),
                    IsTaken = false
                }
            );

            using var hmac = new HMACSHA512();

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Name = "Tom",
                    Email = "tom@email.com",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Tom$1234")),
                    PasswordSalt = hmac.Key,
                    RoleId = 3
                },
                new User()
                {
                    Id = 2,
                    Name = "Anna",
                    Email = "anna@email.com",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Anna$1234")),
                    PasswordSalt = hmac.Key,
                    RoleId = 2
                },
                new User()
                {
                    Id = 3,
                    Name = "Bob",
                    Email = "bob@email.com",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Bob$1234")),
                    PasswordSalt = hmac.Key,
                    RoleId = 2
                },
                new User()
                {
                    Id = 4,
                    Name = "Katya",
                    Email = "katya@email.com",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Katya$1234")),
                    PasswordSalt = hmac.Key,
                    RoleId = 1
                }
            );
        }
    }
}
