﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PlantsIdentifierAPI.Data;

namespace PlantsIdentifierAPI.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20181005231806_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PlantsIdentifierAPI.Models.Plant", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommonName");

                    b.Property<string>("PhotoURL");

                    b.HasKey("ID");

                    b.ToTable("Plant");
                });
#pragma warning restore 612, 618
        }
    }
}
