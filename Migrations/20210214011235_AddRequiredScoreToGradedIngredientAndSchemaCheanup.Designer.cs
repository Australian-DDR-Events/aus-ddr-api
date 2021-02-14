﻿// <auto-generated />
using System;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AusDdrApi.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210214011235_AddRequiredScoreToGradedIngredientAndSchemaCheanup")]
    partial class AddRequiredScoreToGradedIngredientAndSchemaCheanup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("AusDdrApi.Entities.Dancer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthenticationId")
                        .HasColumnType("text");

                    b.Property<string>("DdrCode")
                        .HasColumnType("text");

                    b.Property<string>("DdrName")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryMachineLocation")
                        .HasColumnType("text");

                    b.Property<string>("State")
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

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
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
                        .HasColumnType("text");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.ToTable("GradedDish");
                });

            modelBuilder.Entity("AusDdrApi.Entities.GradedIngredient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
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

                    b.Property<Guid?>("DishId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("SongId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

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

                    b.Property<Guid>("SongId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SubmissionTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValue(new DateTime(2021, 2, 14, 1, 12, 35, 149, DateTimeKind.Utc).AddTicks(440));

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DancerId");

                    b.HasIndex("SongId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Song", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Artist")
                        .HasColumnType("text");

                    b.Property<string>("Difficulty")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Songs");
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

                    b.HasOne("AusDdrApi.Entities.Song", "Score")
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
                    b.HasOne("AusDdrApi.Entities.Dish", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("DishId");

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

                    b.HasOne("AusDdrApi.Entities.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("AusDdrApi.Entities.Dish", b =>
                {
                    b.Navigation("Ingredients");
                });
#pragma warning restore 612, 618
        }
    }
}
