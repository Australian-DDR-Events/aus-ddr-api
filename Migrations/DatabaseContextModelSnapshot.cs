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

                    b.Property<string>("ProfilePictureUrl")
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

                    b.Property<Guid?>("GradedDancerDishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GradedIngredientId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DancerId");

                    b.HasIndex("GradedDancerDishId");

                    b.HasIndex("GradedIngredientId");

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
                        .HasColumnType("text");

                    b.HasKey("Id");

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
                        .HasDefaultValue(new DateTime(2021, 2, 7, 7, 42, 17, 216, DateTimeKind.Utc).AddTicks(8180));

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

            modelBuilder.Entity("AusDdrApi.Entities.GradedDancerDish", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dancer", "Dancer")
                        .WithMany("GradedDishes")
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
                        .WithMany("GradedIngredients")
                        .HasForeignKey("DancerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.GradedDancerDish", "GradedDancerDish")
                        .WithMany()
                        .HasForeignKey("GradedDancerDishId");

                    b.HasOne("AusDdrApi.Entities.GradedIngredient", "GradedIngredient")
                        .WithMany()
                        .HasForeignKey("GradedIngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dancer");

                    b.Navigation("GradedDancerDish");

                    b.Navigation("GradedIngredient");
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

            modelBuilder.Entity("AusDdrApi.Entities.Score", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dancer", "Dancer")
                        .WithMany("Scores")
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

            modelBuilder.Entity("DishIngredient", b =>
                {
                    b.HasOne("AusDdrApi.Entities.Dish", null)
                        .WithMany()
                        .HasForeignKey("DishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AusDdrApi.Entities.Ingredient", null)
                        .WithMany()
                        .HasForeignKey("IngredientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AusDdrApi.Entities.Dancer", b =>
                {
                    b.Navigation("GradedDishes");

                    b.Navigation("GradedIngredients");

                    b.Navigation("Scores");
                });
#pragma warning restore 612, 618
        }
    }
}
