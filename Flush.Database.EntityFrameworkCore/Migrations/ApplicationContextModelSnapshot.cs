﻿// <auto-generated />
using System;
using Flush.Database.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Flush.Database.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Participant", b =>
                {
                    b.Property<int>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("IsModerator")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastSeenDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LastVote")
                        .HasColumnType("int");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("UniqueUserId")
                        .HasColumnType("int");

                    b.HasKey("ParticipantId");

                    b.HasIndex("SessionId");

                    b.HasIndex("UniqueUserId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerUniqueId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomUniqueId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Phase")
                        .HasColumnType("int");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("SessionId");

                    b.HasIndex("RoomId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.UniqueUser", b =>
                {
                    b.Property<int>("UniqueUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UniqueUserId");

                    b.ToTable("UniqueUsers");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Participant", b =>
                {
                    b.HasOne("Flush.Database.EntityFrameworkCore.Session", "Session")
                        .WithMany("Participants")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flush.Database.EntityFrameworkCore.UniqueUser", "UniqueUser")
                        .WithMany("Participants")
                        .HasForeignKey("UniqueUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("UniqueUser");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Session", b =>
                {
                    b.HasOne("Flush.Database.EntityFrameworkCore.Room", "Room")
                        .WithMany("Sessions")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Room", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Session", b =>
                {
                    b.Navigation("Participants");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.UniqueUser", b =>
                {
                    b.Navigation("Participants");
                });
#pragma warning restore 612, 618
        }
    }
}