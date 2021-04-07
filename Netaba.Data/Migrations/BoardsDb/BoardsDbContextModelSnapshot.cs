﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Netaba.Data.Contexts;

namespace Netaba.Data.Migrations.BoardsDb
{
    [DbContext(typeof(BoardsDbContext))]
    partial class BoardsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Netaba.Data.Enteties.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("Netaba.Data.Enteties.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Format")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SizeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ViewHeight")
                        .HasColumnType("int");

                    b.Property<int>("ViewWidth")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Netaba.Data.Enteties.Post", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOp")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSage")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PassHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("PictureId")
                        .HasColumnType("int");

                    b.Property<string>("PosterName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TreadId")
                        .HasColumnType("int");

                    b.HasKey("BoardId", "Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("BoardId", "TreadId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Netaba.Data.Enteties.Tread", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("TimeOfLastPost")
                        .HasColumnType("datetime2");

                    b.HasKey("BoardId", "Id");

                    b.ToTable("Treads");
                });

            modelBuilder.Entity("Netaba.Data.Enteties.Post", b =>
                {
                    b.HasOne("Netaba.Data.Enteties.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("Netaba.Data.Enteties.Tread", "Tread")
                        .WithMany("Posts")
                        .HasForeignKey("BoardId", "TreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Tread");
                });

            modelBuilder.Entity("Netaba.Data.Enteties.Tread", b =>
                {
                    b.HasOne("Netaba.Data.Enteties.Board", "Board")
                        .WithMany("Treads")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("Netaba.Data.Enteties.Board", b =>
                {
                    b.Navigation("Treads");
                });

            modelBuilder.Entity("Netaba.Data.Enteties.Tread", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
