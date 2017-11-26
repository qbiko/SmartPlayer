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
        public DbSet<Club> Club { get; set; }
        public DbSet<Game> Games { get;set; }
        public DbSet<Player> Players { get;set; }
        public DbSet<PulseSensorResult> PulseSensorResults { get; set; }
        public DbSet<AccelerometerAndGyroscopeResult> AccelerometerAndGyroscopeResults { get; set; }
        public DbSet<PlayerInGame> PlayerInGames { get; set; }
        public DbSet<Mock> Mock { get; set; }
        public DbSet<GPSLocation> GPSLocation { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<Pitch> Pitch { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Club>().ToTable("Club");
            modelBuilder.Entity<Club>().HasKey(o => o.Id);
            modelBuilder.Entity<Club>().HasMany(o => o.Modules).WithOne(o => o.Club);

            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<Game>().HasKey(o => o.Id);
            modelBuilder.Entity<Game>().HasOne(o => o.Club).WithMany(o => o.Games);
            modelBuilder.Entity<Game>().HasOne(o => o.Pitch);

            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Player>().HasKey(o => o.Id);
            modelBuilder.Entity<Player>().HasOne(o => o.Club).WithMany(o => o.Players);
            modelBuilder.Entity<Player>().HasMany(o => o.PlayerInGames).WithOne(o => o.Player);

            modelBuilder.Entity<PulseSensorResult>().ToTable("PulseSensorResult");
            modelBuilder.Entity<PulseSensorResult>().HasKey(o => o.Id);
            modelBuilder.Entity<PulseSensorResult>().HasOne(o => o.PlayerInGame).WithMany(o => o.PulseSensorResults).HasForeignKey(i => i.PlayerInGameId);

            modelBuilder.Entity<AccelerometerAndGyroscopeResult>().ToTable("AccelerometerAndGyroscopeResult");
            modelBuilder.Entity<AccelerometerAndGyroscopeResult>().HasKey(o => o.Id);
            modelBuilder.Entity<AccelerometerAndGyroscopeResult>().HasOne(o => o.PlayerInGame).WithMany(o => o.AccelerometerAndGyroscopeResults).HasForeignKey(i=>i.PlayerInGameId);

            modelBuilder.Entity<PlayerInGame>().ToTable("PlayerInGame");
            modelBuilder.Entity<PlayerInGame>().HasKey(o => o.Id);
            modelBuilder.Entity<PlayerInGame>().HasOne(o => o.Game).WithMany(o => o.PlayerInGames).IsRequired(false);
            modelBuilder.Entity<PlayerInGame>().HasOne(o => o.Player).WithMany(o => o.PlayerInGames).IsRequired(false);
            modelBuilder.Entity<PlayerInGame>().HasMany(o => o.PulseSensorResults).WithOne(o => o.PlayerInGame);
            modelBuilder.Entity<PlayerInGame>().HasMany(o => o.GPSLocations).WithOne(o => o.PlayerInGame);
            modelBuilder.Entity<PlayerInGame>().HasOne(o => o.Module).WithMany(o => o.PlayerInGames).IsRequired(false);

            modelBuilder.Entity<Mock>().ToTable("Mock");
            modelBuilder.Entity<Mock>().HasKey(o => o.Id);

            modelBuilder.Entity<GPSLocation>().ToTable("GPSLocation");
            modelBuilder.Entity<GPSLocation>().HasKey(o => o.Id);
            modelBuilder.Entity<GPSLocation>().HasOne(o => o.PlayerInGame).WithMany(o => o.GPSLocations).HasForeignKey(o => o.PlayerInGameId);

            modelBuilder.Entity<Module>().ToTable("Module");
            modelBuilder.Entity<Module>().HasKey(o => o.Id);
            modelBuilder.Entity<Module>().HasMany(o => o.PlayerInGames).WithOne(i => i.Module);
            modelBuilder.Entity<Module>().HasOne(o => o.Club).WithMany(o => o.Modules);

            modelBuilder.Entity<Pitch>().ToTable("Pitch");
            modelBuilder.Entity<Pitch>().HasKey(o => o.Id);
        }
    }
}
