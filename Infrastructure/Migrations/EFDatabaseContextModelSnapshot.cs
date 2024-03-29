﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(EFDatabaseContext))]
    partial class EFDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Application.Core.Entities.Chart", b =>
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

                    b.ToTable("Charts");
                });

            modelBuilder.Entity("Application.Core.Entities.Connection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConnectionData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("DancerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DancerId");

                    b.ToTable("Connections");
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

                    b.ToTable("Dish");
                });

            modelBuilder.Entity("Application.Core.Entities.DishSong", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChartId")
                        .HasColumnType("uuid");

                    b.Property<string>("CookingMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CookingOrder")
                        .HasColumnType("integer");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChartId");

                    b.HasIndex("DishId");

                    b.ToTable("DishSong");
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

                    b.ToTable("GradedDish");
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

                    b.ToTable("GradedIngredient");
                });

            modelBuilder.Entity("Application.Core.Entities.Ingredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChartId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChartId");

                    b.ToTable("Ingredient");
                });

            modelBuilder.Entity("Application.Core.Entities.Internal.Session", b =>
                {
                    b.Property<string>("Cookie")
                        .HasColumnType("text");

                    b.Property<Guid>("DancerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Expiry")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Cookie");

                    b.HasIndex("DancerId");

                    b.ToTable("Sessions");
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

                    b.Property<Guid>("ChartId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DancerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("uuid");

                    b.Property<int>("ExScore")
                        .HasColumnType("integer");

                    b.Property<DateTime>("SubmissionTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValue(new DateTime(2023, 5, 29, 6, 3, 1, 460, DateTimeKind.Utc).AddTicks(9560));

                    b.Property<bool>("Validated")
                        .HasColumnType("boolean");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ChartId");

                    b.HasIndex("DancerId");

                    b.HasIndex("EventId");

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

            modelBuilder.Entity("ChartCourse", b =>
                {
                    b.Property<Guid>("ChartsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CoursesId")
                        .HasColumnType("uuid");

                    b.HasKey("ChartsId", "CoursesId");

                    b.HasIndex("CoursesId");

                    b.ToTable("ChartCourse");
                });

            modelBuilder.Entity("ChartEvent", b =>
                {
                    b.Property<Guid>("ChartsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EventsId")
                        .HasColumnType("uuid");

                    b.HasKey("ChartsId", "EventsId");

                    b.HasIndex("EventsId");

                    b.ToTable("ChartEvent");
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

            modelBuilder.Entity("Application.Core.Entities.Chart", b =>
                {
                    b.HasOne("Application.Core.Entities.Song", "Song")
                        .WithMany("Charts")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Song");
                });

            modelBuilder.Entity("Application.Core.Entities.Connection", b =>
                {
                    b.HasOne("Application.Core.Entities.Dancer", "Dancer")
                        .WithMany()
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");
                });

            modelBuilder.Entity("Application.Core.Entities.DishSong", b =>
                {
                    b.HasOne("Application.Core.Entities.Chart", "Chart")
                        .WithMany()
                        .HasForeignKey("ChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Dish", "Dish")
                        .WithMany("DishSongs")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chart");

                    b.Navigation("Dish");
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
                    b.HasOne("Application.Core.Entities.Chart", "Chart")
                        .WithMany()
                        .HasForeignKey("ChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chart");
                });

            modelBuilder.Entity("Application.Core.Entities.Internal.Session", b =>
                {
                    b.HasOne("Application.Core.Entities.Dancer", "Dancer")
                        .WithMany()
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");
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
                    b.HasOne("Application.Core.Entities.Chart", "Chart")
                        .WithMany("Scores")
                        .HasForeignKey("ChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Dancer", "Dancer")
                        .WithMany("Scores")
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.Navigation("Chart");

                    b.Navigation("Dancer");

                    b.Navigation("Event");
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

            modelBuilder.Entity("ChartCourse", b =>
                {
                    b.HasOne("Application.Core.Entities.Chart", null)
                        .WithMany()
                        .HasForeignKey("ChartsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChartEvent", b =>
                {
                    b.HasOne("Application.Core.Entities.Chart", null)
                        .WithMany()
                        .HasForeignKey("ChartsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Core.Entities.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
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

            modelBuilder.Entity("Application.Core.Entities.Chart", b =>
                {
                    b.Navigation("Scores");
                });

            modelBuilder.Entity("Application.Core.Entities.Dancer", b =>
                {
                    b.Navigation("Scores");
                });

            modelBuilder.Entity("Application.Core.Entities.Dish", b =>
                {
                    b.Navigation("DishSongs");
                });

            modelBuilder.Entity("Application.Core.Entities.Song", b =>
                {
                    b.Navigation("Charts");
                });
#pragma warning restore 612, 618
        }
    }
}
