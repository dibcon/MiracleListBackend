﻿// <auto-generated />
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BO.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int>("UserID");

                    b.HasKey("CategoryID");

                    b.HasIndex("Name");

                    b.HasIndex("UserID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BO.Client", b =>
                {
                    b.Property<Guid>("ClientID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Company")
                        .HasMaxLength(50);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime?>("Deleted");

                    b.Property<string>("EMail")
                        .HasMaxLength(50);

                    b.Property<string>("Memo");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("Type")
                        .HasMaxLength(10);

                    b.HasKey("ClientID");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("BO.Log", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Client")
                        .HasMaxLength(15);

                    b.Property<string>("ClientDetails");

                    b.Property<DateTime>("DateTime");

                    b.Property<int>("Event");

                    b.Property<string>("Note");

                    b.Property<string>("Operation");

                    b.Property<int>("Severity");

                    b.Property<string>("Text");

                    b.Property<string>("Token");

                    b.Property<int?>("UserID");

                    b.HasKey("LogID");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("BO.SubTask", b =>
                {
                    b.Property<int>("SubTaskID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Done");

                    b.Property<int>("TaskID");

                    b.Property<string>("Title")
                        .HasMaxLength(250);

                    b.HasKey("SubTaskID");

                    b.HasIndex("TaskID");

                    b.ToTable("SubTask");
                });

            modelBuilder.Entity("BO.Task", b =>
                {
                    b.Property<int>("TaskID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("Done");

                    b.Property<DateTime?>("Due");

                    b.Property<int>("DueInDays")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("DATEDIFF(day, GETDATE(), [Due])");

                    b.Property<decimal?>("Effort");

                    b.Property<int?>("Importance");

                    b.Property<string>("Note");

                    b.Property<int>("Order");

                    b.Property<string>("Title")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("???")
                        .HasMaxLength(250);

                    b.HasKey("TaskID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("Done");

                    b.HasIndex("Due");

                    b.HasIndex("Title");

                    b.HasIndex("Title", "Due");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("BO.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ClientID");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime?>("LastActivity");

                    b.Property<int?>("MaxTasks");

                    b.Property<string>("Memo");

                    b.Property<string>("PasswordHash");

                    b.Property<byte[]>("Salt");

                    b.Property<string>("Token")
                        .HasMaxLength(38);

                    b.Property<string>("UserName");

                    b.HasKey("UserID");

                    b.HasIndex("ClientID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BO.Category", b =>
                {
                    b.HasOne("BO.User", "User")
                        .WithMany("CategorySet")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BO.SubTask", b =>
                {
                    b.HasOne("BO.Task", "Task")
                        .WithMany("SubTaskSet")
                        .HasForeignKey("TaskID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BO.Task", b =>
                {
                    b.HasOne("BO.Category", "Category")
                        .WithMany("TaskSet")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BO.User", b =>
                {
                    b.HasOne("BO.Client", "Client")
                        .WithMany("UserSet")
                        .HasForeignKey("ClientID");
                });
#pragma warning restore 612, 618
        }
    }
}
