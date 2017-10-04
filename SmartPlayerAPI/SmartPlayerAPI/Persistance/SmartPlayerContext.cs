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

        public DbSet<Club> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Club>().ToTable("User");
            modelBuilder.Entity<Club>().HasKey(i => i.Id);
        }
    }
}
