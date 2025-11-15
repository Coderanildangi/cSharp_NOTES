using Microsoft.EntityFrameworkCore;
using Note_Server.Models;
using System;

namespace Note_Server.Data
{
    public class NotesDbContext : DbContext
    {
        // DbSet for Note entities.
        public DbSet<Note> Notes { get; set; }

        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define Id as the primary key
            modelBuilder.Entity<Note>()
                .HasKey(n => n.Id);

            // Seed initial data for a quick start
            modelBuilder.Entity<Note>().HasData(
                new Note
                {
                    Id = "a2a9d8c3-4e6f-4c5b-9d0a-1b3c4f5e6d7a", // Explicit ID for seeding
                    Title = "Project Alpha",
                    Content = "Initial meeting notes for the new Q4 feature.",
                    Tag = "idea",
                    // Using a fixed DateTime ensures consistent seed data across runs
                    CreatedAt = DateTime.Parse("2025-01-01T10:00:00Z").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-01-01T10:00:00Z").ToUniversalTime()
                },
                new Note
                {
                    Id = "b9b8c7a6-1d2e-3f4g-5h6i-7j8k9l0m1n2o",
                    Title = "Hush Hush Details",
                    Content = "My secret plan to take over the world. Only access with PIN.",
                    Tag = "confidential",
                    CreatedAt = DateTime.Parse("2025-01-01T11:00:00Z").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-01-01T11:00:00Z").ToUniversalTime()
                },
                new Note
                {
                    Id = "c1c2d3e4-5f6g-7h8i-9j0k-1l2m3n4o5p6q",
                    Title = "Grocery List",
                    Content = "Milk, Eggs, Bread.",
                    Tag = "todo",
                    CreatedAt = DateTime.Parse("2025-01-01T12:00:00Z").ToUniversalTime(),
                    UpdatedAt = DateTime.Parse("2025-01-01T12:00:00Z").ToUniversalTime()
                }
            );
        }
    }
}
