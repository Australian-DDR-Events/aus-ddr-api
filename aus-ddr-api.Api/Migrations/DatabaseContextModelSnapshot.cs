﻿// <auto-generated />
using System;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AusDdrApi.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("AusDdrApi.Entities.Badge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Badges");
                });

            modelBuilder.Entity("AusDdrApi.Entities.BadgeThreshold", b =>
                {
                    b.Property<Guid>("BadgeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BadgeId1")
                        .HasColumnType("uuid");

                    b.Property<int>("RequiredPoints")
                        .HasColumnType("integer");

                    b.HasKey("BadgeId");

                    b.HasIndex("BadgeId1");

                    b.ToTable("BadgeThresholds");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Dancer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthenticationId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DdrCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DdrName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrimaryMachineLocation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthenticationId")
                        .IsUnique();

                    b.ToTable("Dancers");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("MaxScore")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("AusDdrApi.Entities.DishIngredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IngredientId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("IngredientId");

                    b.ToTable("DishIngredients");
                });

            modelBuilder.Entity("AusDdrApi.Entities.DishSong", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CookingMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CookingOrder")
                        .HasColumnType("integer");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("SongId");

                    b.ToTable("DishSongs");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedDancerDish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GradedDishId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DancerId");

                    b.HasIndex("GradedDishId");

                    b.ToTable("GradedDancerDishes");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedDancerIngredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GradedIngredientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ScoreId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DancerId");

                    b.HasIndex("GradedIngredientId");

                    b.HasIndex("ScoreId");

                    b.ToTable("GradedDancerIngredients");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedDish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.ToTable("GradedDishes");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedIngredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("IngredientId")
                        .HasColumnType("uuid");

                    b.Property<int>("RequiredScore")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IngredientId");

                    b.ToTable("GradedIngredients");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Ingredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("GradedDancerDishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SubmissionTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValue(new DateTime(2021, 3, 3, 0, 50, 59, 788, DateTimeKind.Utc).AddTicks(9590));

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DancerId");

                    b.HasIndex("GradedDancerDishId");

                    b.HasIndex("SongId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Song", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<int>("MaxScore")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("BadgeDancer", b =>
                {
                    b.Property<Guid>("BadgesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DancersId")
                        .HasColumnType("uuid");

                    b.HasKey("BadgesId", "DancersId");

                    b.HasIndex("DancersId");

                    b.ToTable("BadgeDancer");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Badge", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("AusDdrApi.Entities.BadgeThreshold", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Badge", "Badge")
                        .WithMany()
                        .HasForeignKey("BadgeId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Badge");
                });

            modelBuilder.Entity("AusDdrApi.Entities.DishIngredient", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Ingredient");
                });

            modelBuilder.Entity("AusDdrApi.Entities.DishSong", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dish", "Dish")
                        .WithMany("DishSongs")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedDancerDish", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dancer", "Dancer")
                        .WithMany()
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.GradedDish", "GradedDish")
                        .WithMany()
                        .HasForeignKey("GradedDishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");

                    b.Navigation("GradedDish");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedDancerIngredient", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dancer", "Dancer")
                        .WithMany()
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.GradedIngredient", "GradedIngredient")
                        .WithMany()
                        .HasForeignKey("GradedIngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.Score", "Score")
                        .WithMany()
                        .HasForeignKey("ScoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");

                    b.Navigation("GradedIngredient");

                    b.Navigation("Score");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedDish", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedIngredient", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Ingredient", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Song");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Score", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dancer", "Dancer")
                        .WithMany()
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.GradedDancerDish", null)
                        .WithMany("Scores")
                        .HasForeignKey("GradedDancerDishId");

                    b.HasOne("AusDdrApi.Entities.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("BadgeDancer", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Badge", null)
                        .WithMany()
                        .HasForeignKey("BadgesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.Dancer", null)
                        .WithMany()
                        .HasForeignKey("DancersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AusDdrApi.Entities.Dish", b =>
                {
                    b.Navigation("DishSongs");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedDancerDish", b =>
                {
                    b.Navigation("Scores");
                });
#pragma warning restore 612, 618
        }
    }
}
