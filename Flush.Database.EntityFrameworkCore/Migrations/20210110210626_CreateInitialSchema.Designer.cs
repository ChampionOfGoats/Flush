﻿// <auto-generated />
using System;
using Flush.Database.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Flush.Database.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210110210626_CreateInitialSchema")]
    internal partial class CreateInitialSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Owner")
                        .HasColumnType("TEXT");

                    b.HasKey("RoomId");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("EndDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("SessionId");

                    b.HasIndex("RoomId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Trace", b =>
                {
                    b.Property<int>("TraceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Event")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RaisedBy")
                        .HasColumnType("TEXT");

                    b.Property<int>("SessionId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("TraceId");

                    b.HasIndex("SessionId");

                    b.ToTable("Traces");
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

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Trace", b =>
                {
                    b.HasOne("Flush.Database.EntityFrameworkCore.Session", "Session")
                        .WithMany("Traces")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Room", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Flush.Database.EntityFrameworkCore.Session", b =>
                {
                    b.Navigation("Traces");
                });
#pragma warning restore 612, 618
        }
    }
}
