﻿// <auto-generated />
using System;
using KoalaChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KoalaChatApp.Infrastructure.Data.Migrations
{
    [DbContext(typeof(KoalaChatDbContext))]
    [Migration("20200918211706_InitialDatabaseCreation")]
    partial class InitialDatabaseCreation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KoalaChatApp.ApplicationCore.Entities.ChatRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<short>("MaxCharactersCount")
                        .HasColumnType("smallint");

                    b.Property<short>("MaxMessagesCount")
                        .HasColumnType("smallint");

                    b.Property<short>("MaxUsersAllowed")
                        .HasColumnType("smallint");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("KoalaChatApp.ApplicationCore.Entities.ChatRoom", b =>
                {
                    b.OwnsMany("KoalaChatApp.ApplicationCore.Entities.ChatMessageText", "Messages", b1 =>
                        {
                            b1.Property<Guid>("ChatRoomId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset");

                            b1.Property<bool>("IsDeleted")
                                .HasColumnType("bit");

                            b1.Property<int>("MessageType")
                                .HasColumnType("int");

                            b1.Property<DateTimeOffset>("ModifiedAt")
                                .HasColumnType("datetimeoffset");

                            b1.Property<DateTimeOffset>("SentDate")
                                .HasColumnType("datetimeoffset");

                            b1.Property<string>("Text")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("ChatRoomId", "Id");

                            b1.ToTable("ChatMessageText");

                            b1.WithOwner()
                                .HasForeignKey("ChatRoomId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
