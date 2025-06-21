using Microsoft.EntityFrameworkCore;
using RecordStore.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordStore.Database.Context
{
    public class RecordStoreDatabaseContext : DbContext
    {
        public RecordStoreDatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public RecordStoreDatabaseContext()
        {
        }

        public DbSet<Record> Records { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistRecord> ArtistRecords { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRecord> OrderRecords { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ArtistRecord>()
                .HasIndex(ar => new { ar.RecordId, ar.ArtistId })
                .IsUnique();

            modelBuilder.Entity<Inventory>()
                .HasIndex(i => i.RecordId)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<OrderRecord>()
                .HasIndex(or => new { or.OrderId, or.RecordId })
                .IsUnique();

            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.UserId, r.RecordId })
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("data source=localhost;Initial Catalog=RecordStore;Persist Security Info=True;User ID=recordstoreDAWM;Password=1q2w3e;Connection Timeout=60;TrustServerCertificate=True");
            }
        }
    }
}
