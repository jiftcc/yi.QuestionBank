﻿// <auto-generated />
using CC.Yi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CC.Yi.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210411075934_qb3")]
    partial class qb3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("CC.Yi.Model.question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<char>("answer")
                        .HasColumnType("TEXT");

                    b.Property<int>("difficulty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("question_a")
                        .HasColumnType("TEXT");

                    b.Property<string>("question_b")
                        .HasColumnType("TEXT");

                    b.Property<string>("question_c")
                        .HasColumnType("TEXT");

                    b.Property<string>("question_d")
                        .HasColumnType("TEXT");

                    b.Property<string>("subject")
                        .HasColumnType("TEXT");

                    b.Property<string>("type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("question");
                });

            modelBuilder.Entity("CC.Yi.Model.user", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("integral")
                        .HasColumnType("TEXT");

                    b.Property<string>("password")
                        .HasColumnType("TEXT");

                    b.Property<string>("user_name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("user");
                });
#pragma warning restore 612, 618
        }
    }
}
