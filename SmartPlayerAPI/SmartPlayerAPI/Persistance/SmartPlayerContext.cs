using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SmartPlayerAPI.Persistance.Models;

namespace SmartPlayerAPI.Persistance
{
    public class SmartPlayerContext : DbContext
    {
        public SmartPlayerContext(DbContextOptions<SmartPlayerContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(i => i.Id);
        }
    }
}
