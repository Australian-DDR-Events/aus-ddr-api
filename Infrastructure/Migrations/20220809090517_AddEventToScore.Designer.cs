﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(EFDatabaseContext))]
    [Migration("20220809090517_AddEventToScore")]
    partial class AddEventToScore
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Application.Core.Entities.Badge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Threshold")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Badges");
                });

            modelBuilder.Entity("Application.Core.Entities.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Application.Core.Entities.Dancer", b =>
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

                    b.Property<DateTime?>("ProfilePictureTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthenticationId")
                        .IsUnique();

                    b.ToTable("Dancers");
                });

            modelBuilder.Entity("Application.Core.Entities.Dish", b =>
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

            modelBuilder.Entity("Application.Core.Entities.DishSong", b =>
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

                    b.Property<Guid>("SongDifficultyId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("SongDifficultyId");

                    b.ToTable("DishSongs");
                });

            modelBuilder.Entity("Application.Core.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Application.Core.Entities.GradedDancerDish", b =>
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

            modelBuilder.Entity("Application.Core.Entities.GradedDancerIngredient", b =>
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

            modelBuilder.Entity("Application.Core.Entities.GradedDish", b =>
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

            modelBuilder.Entity("Application.Core.Entities.GradedIngredient", b =>
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

            modelBuilder.Entity("Application.Core.Entities.Ingredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SongDifficultyId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SongDifficultyId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Application.Core.Entities.Reward", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TriggerData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("Application.Core.Entities.RewardQuality", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RewardId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RewardId");

                    b.ToTable("RewardQualities");
                });

            modelBuilder.Entity("Application.Core.Entities.RewardTrigger", b =>
                {
                    b.Property<string>("Trigger")
                        .HasColumnType("text");

                    b.HasKey("Trigger");

                    b.ToTable("RewardTriggers");
                });

            modelBuilder.Entity("Application.Core.Entities.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("uuid");

                    b.Property<int>("ExScore")
                        .HasColumnType("integer");

                    b.Property<Guid?>("GradedDancerDishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SongDifficultyId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SubmissionTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2022, 8, 9, 9, 5, 17, 763, DateTimeKind.Utc).AddTicks(7320));

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DancerId");

                    b.HasIndex("EventId");

                    b.HasIndex("GradedDancerDishId");

                    b.HasIndex("SongDifficultyId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("Application.Core.Entities.Song", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KonamiId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Application.Core.Entities.SongDifficulty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<int>("MaxScore")
                        .HasColumnType("integer");

                    b.Property<string>("PlayMode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("SongDifficulties");
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

            modelBuilder.Entity("CourseEvent", b =>
                {
                    b.Property<Guid>("CoursesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EventsId")
                        .HasColumnType("uuid");

                    b.HasKey("CoursesId", "EventsId");

                    b.HasIndex("EventsId");

                    b.ToTable("CourseEvent");
                });

            modelBuilder.Entity("CourseSongDifficulty", b =>
                {
                    b.Property<Guid>("CoursesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SongDifficultiesId")
                        .HasColumnType("uuid");

                    b.HasKey("CoursesId", "SongDifficultiesId");

                    b.HasIndex("SongDifficultiesId");

                    b.ToTable("CourseSongDifficulty");
                });

            modelBuilder.Entity("DancerRewardQuality", b =>
                {
                    b.Property<Guid>("DancersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RewardQualitiesId")
                        .HasColumnType("uuid");

                    b.HasKey("DancersId", "RewardQualitiesId");

                    b.HasIndex("RewardQualitiesId");

                    b.ToTable("DancerRewardQuality");
                });

            modelBuilder.Entity("DishIngredient", b =>
                {
                    b.Property<Guid>("DishesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IngredientsId")
                        .HasColumnType("uuid");

                    b.HasKey("DishesId", "IngredientsId");

                    b.HasIndex("IngredientsId");

                    b.ToTable("DishIngredient");
                });

            modelBuilder.Entity("EventSongDifficulty", b =>
                {
                    b.Property<Guid>("EventsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SongDifficultiesId")
                        .HasColumnType("uuid");

                    b.HasKey("EventsId", "SongDifficultiesId");

                    b.HasIndex("SongDifficultiesId");

                    b.ToTable("EventSongDifficulty");
                });

            modelBuilder.Entity("RewardRewardTrigger", b =>
                {
                    b.Property<Guid>("RewardsId")
                        .HasColumnType("uuid");

                    b.Property<string>("TriggersTrigger")
                        .HasColumnType("text");

                    b.HasKey("RewardsId", "TriggersTrigger");

                    b.HasIndex("TriggersTrigger");

                    b.ToTable("RewardRewardTrigger");
                });

            modelBuilder.Entity("Application.Core.Entities.Badge", b =>
                {
                    b.HasOne("Application.Core.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Application.Core.Entities.DishSong", b =>
                {
                    b.HasOne("Application.Core.Entities.Dish", "Dish")
                        .WithMany("DishSongs")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.SongDifficulty", "SongDifficulty")
                        .WithMany()
                        .HasForeignKey("SongDifficultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("SongDifficulty");
                });

            modelBuilder.Entity("Application.Core.Entities.GradedDancerDish", b =>
                {
                    b.HasOne("Application.Core.Entities.Dancer", "Dancer")
                        .WithMany()
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.GradedDish", "GradedDish")
                        .WithMany()
                        .HasForeignKey("GradedDishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");

                    b.Navigation("GradedDish");
                });

            modelBuilder.Entity("Application.Core.Entities.GradedDancerIngredient", b =>
                {
                    b.HasOne("Application.Core.Entities.Dancer", "Dancer")
                        .WithMany()
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.GradedIngredient", "GradedIngredient")
                        .WithMany()
                        .HasForeignKey("GradedIngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Score", "Score")
                        .WithMany()
                        .HasForeignKey("ScoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");

                    b.Navigation("GradedIngredient");

                    b.Navigation("Score");
                });

            modelBuilder.Entity("Application.Core.Entities.GradedDish", b =>
                {
                    b.HasOne("Application.Core.Entities.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("Application.Core.Entities.GradedIngredient", b =>
                {
                    b.HasOne("Application.Core.Entities.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");
                });

            modelBuilder.Entity("Application.Core.Entities.Ingredient", b =>
                {
                    b.HasOne("Application.Core.Entities.SongDifficulty", "SongDifficulty")
                        .WithMany()
                        .HasForeignKey("SongDifficultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SongDifficulty");
                });

            modelBuilder.Entity("Application.Core.Entities.RewardQuality", b =>
                {
                    b.HasOne("Application.Core.Entities.Reward", "Reward")
                        .WithMany()
                        .HasForeignKey("RewardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reward");
                });

            modelBuilder.Entity("Application.Core.Entities.Score", b =>
                {
                    b.HasOne("Application.Core.Entities.Dancer", "Dancer")
                        .WithMany("Scores")
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.HasOne("Application.Core.Entities.GradedDancerDish", null)
                        .WithMany("Scores")
                        .HasForeignKey("GradedDancerDishId");

                    b.HasOne("Application.Core.Entities.SongDifficulty", "SongDifficulty")
                        .WithMany("Scores")
                        .HasForeignKey("SongDifficultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");

                    b.Navigation("Event");

                    b.Navigation("SongDifficulty");
                });

            modelBuilder.Entity("Application.Core.Entities.SongDifficulty", b =>
                {
                    b.HasOne("Application.Core.Entities.Song", "Song")
                        .WithMany("SongDifficulties")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Song");
                });

            modelBuilder.Entity("BadgeDancer", b =>
                {
                    b.HasOne("Application.Core.Entities.Badge", null)
                        .WithMany()
                        .HasForeignKey("BadgesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Dancer", null)
                        .WithMany()
                        .HasForeignKey("DancersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseEvent", b =>
                {
                    b.HasOne("Application.Core.Entities.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseSongDifficulty", b =>
                {
                    b.HasOne("Application.Core.Entities.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.SongDifficulty", null)
                        .WithMany()
                        .HasForeignKey("SongDifficultiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DancerRewardQuality", b =>
                {
                    b.HasOne("Application.Core.Entities.Dancer", null)
                        .WithMany()
                        .HasForeignKey("DancersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.RewardQuality", null)
                        .WithMany()
                        .HasForeignKey("RewardQualitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DishIngredient", b =>
                {
                    b.HasOne("Application.Core.Entities.Dish", null)
                        .WithMany()
                        .HasForeignKey("DishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Ingredient", null)
                        .WithMany()
                        .HasForeignKey("IngredientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventSongDifficulty", b =>
                {
                    b.HasOne("Application.Core.Entities.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.SongDifficulty", null)
                        .WithMany()
                        .HasForeignKey("SongDifficultiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewardRewardTrigger", b =>
                {
                    b.HasOne("Application.Core.Entities.Reward", null)
                        .WithMany()
                        .HasForeignKey("RewardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.RewardTrigger", null)
                        .WithMany()
                        .HasForeignKey("TriggersTrigger")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Application.Core.Entities.Dancer", b =>
                {
                    b.Navigation("Scores");
                });

            modelBuilder.Entity("Application.Core.Entities.Dish", b =>
                {
                    b.Navigation("DishSongs");
                });

            modelBuilder.Entity("Application.Core.Entities.GradedDancerDish", b =>
                {
                    b.Navigation("Scores");
                });

            modelBuilder.Entity("Application.Core.Entities.Song", b =>
                {
                    b.Navigation("SongDifficulties");
                });

            modelBuilder.Entity("Application.Core.Entities.SongDifficulty", b =>
                {
                    b.Navigation("Scores");
                });
#pragma warning restore 612, 618
        }
    }
}
