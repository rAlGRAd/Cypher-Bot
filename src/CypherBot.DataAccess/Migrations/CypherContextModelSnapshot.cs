﻿// <auto-generated />
using System;
using CypherBot.DataAccess.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CypherBot.DataAccess.Migrations
{
    [DbContext(typeof(CypherContext))]
    partial class CypherContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity("CypherBot.Models.Character", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IntPool");

                    b.Property<int>("MightPool");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.Property<string>("Player")
                        .HasMaxLength(30);

                    b.Property<int>("RecoveryDie");

                    b.Property<int>("RecoveryMod");

                    b.Property<int>("SpeedPool");

                    b.Property<int>("Tier");

                    b.Property<int>("XP");

                    b.HasKey("CharacterId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("CypherBot.Models.CharacterCypher", b =>
                {
                    b.Property<int>("CypherId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CharacterId");

                    b.Property<string>("Effect")
                        .HasColumnType("varchar(2000)");

                    b.Property<int>("LevelBonus");

                    b.Property<int>("LevelDie");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.Property<string>("Source")
                        .HasMaxLength(20);

                    b.Property<string>("Type")
                        .HasMaxLength(15);

                    b.HasKey("CypherId");

                    b.HasIndex("CharacterId");

                    b.ToTable("CharacterCyphers");
                });

            modelBuilder.Entity("CypherBot.Models.CharacterInventory", b =>
                {
                    b.Property<int>("InventoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterId");

                    b.Property<string>("ItemName")
                        .HasMaxLength(50);

                    b.Property<int>("Qty");

                    b.HasKey("InventoryId");

                    b.HasIndex("CharacterId");

                    b.ToTable("CharacterInventories");
                });

            modelBuilder.Entity("CypherBot.Models.CharacterRecoveryRoll", b =>
                {
                    b.Property<int>("RecoveryRollId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CharacterId");

                    b.Property<bool>("IsUsed");

                    b.Property<string>("RollName")
                        .HasMaxLength(25);

                    b.HasKey("RecoveryRollId");

                    b.HasIndex("CharacterId");

                    b.ToTable("CharacterRecoveryRolls");
                });

            modelBuilder.Entity("CypherBot.Models.Cypher", b =>
                {
                    b.Property<int>("CypherId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Effect")
                        .HasColumnType("varchar(2000)");

                    b.Property<int>("LevelBonus");

                    b.Property<int>("LevelDie");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.Property<string>("Source")
                        .HasMaxLength(20);

                    b.Property<string>("Type")
                        .HasMaxLength(15);

                    b.HasKey("CypherId");

                    b.ToTable("Cyphers");
                });

            modelBuilder.Entity("CypherBot.Models.CharacterCypher", b =>
                {
                    b.HasOne("CypherBot.Models.Character", "Character")
                        .WithMany("Cyphers")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CypherBot.Models.CharacterInventory", b =>
                {
                    b.HasOne("CypherBot.Models.Character")
                        .WithMany("Inventory")
                        .HasForeignKey("CharacterId");
                });

            modelBuilder.Entity("CypherBot.Models.CharacterRecoveryRoll", b =>
                {
                    b.HasOne("CypherBot.Models.Character")
                        .WithMany("RecoveryRolls")
                        .HasForeignKey("CharacterId");
                });
#pragma warning restore 612, 618
        }
    }
}
