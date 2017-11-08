﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SmartPlayerAPI.Persistance;
using System;

namespace SmartPlayerAPI.Migrations
{
    [DbContext(typeof(SmartPlayerContext))]
    partial class SmartPlayerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.AccelerometerAndGyroscopeResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Lat");

                    b.Property<double>("Lng");

                    b.Property<int>("PlayerInGameId");

                    b.Property<DateTimeOffset>("TimeOfOccur");

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.HasKey("Id");

                    b.HasIndex("PlayerInGameId");

                    b.ToTable("AccelerometerAndGyroscopeResult");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DateOfCreate");

                    b.Property<string>("Name");

                    b.Property<string>("PasswordHash");

                    b.HasKey("Id");

                    b.ToTable("Club");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClubId");

                    b.Property<string>("NameOfGame");

                    b.Property<DateTimeOffset>("TimeOfStart");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.GPSLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double?>("Lat");

                    b.Property<double?>("Lng");

                    b.Property<int>("PlayerInGameId");

                    b.Property<DateTimeOffset>("TimeOfOccur");

                    b.HasKey("Id");

                    b.HasIndex("PlayerInGameId");

                    b.ToTable("GPSLocation");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.Mock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<double>("Number");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Mock");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MACAddress");

                    b.HasKey("Id");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClubId");

                    b.Property<DateTimeOffset>("DateOfBirth");

                    b.Property<string>("FirstName");

                    b.Property<int>("HeighOfUser");

                    b.Property<string>("LastName");

                    b.Property<int>("WeightOfUser");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.PlayerInGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int?>("GameId");

                    b.Property<int?>("ModuleId");

                    b.Property<int>("Number");

                    b.Property<int?>("PlayerId");

                    b.Property<string>("Position");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("ModuleId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerInGame");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.PulseSensorResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PlayerInGameId");

                    b.Property<DateTimeOffset>("TimeOfOccur");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("PlayerInGameId");

                    b.ToTable("PulseSensorResult");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.AccelerometerAndGyroscopeResult", b =>
                {
                    b.HasOne("SmartPlayerAPI.Persistance.Models.PlayerInGame", "PlayerInGame")
                        .WithMany("AccelerometerAndGyroscopeResults")
                        .HasForeignKey("PlayerInGameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.Game", b =>
                {
                    b.HasOne("SmartPlayerAPI.Persistance.Models.Club", "Club")
                        .WithMany("Games")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.GPSLocation", b =>
                {
                    b.HasOne("SmartPlayerAPI.Persistance.Models.PlayerInGame", "PlayerInGame")
                        .WithMany("GPSLocations")
                        .HasForeignKey("PlayerInGameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.Player", b =>
                {
                    b.HasOne("SmartPlayerAPI.Persistance.Models.Club", "Club")
                        .WithMany("Players")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.PlayerInGame", b =>
                {
                    b.HasOne("SmartPlayerAPI.Persistance.Models.Game", "Game")
                        .WithMany("PlayerInGames")
                        .HasForeignKey("GameId");

                    b.HasOne("SmartPlayerAPI.Persistance.Models.Module", "Module")
                        .WithMany("PlayerInGames")
                        .HasForeignKey("ModuleId");

                    b.HasOne("SmartPlayerAPI.Persistance.Models.Player", "Player")
                        .WithMany("PlayerInGames")
                        .HasForeignKey("PlayerId");
                });

            modelBuilder.Entity("SmartPlayerAPI.Persistance.Models.PulseSensorResult", b =>
                {
                    b.HasOne("SmartPlayerAPI.Persistance.Models.PlayerInGame", "PlayerInGame")
                        .WithMany("PulseSensorResults")
                        .HasForeignKey("PlayerInGameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
