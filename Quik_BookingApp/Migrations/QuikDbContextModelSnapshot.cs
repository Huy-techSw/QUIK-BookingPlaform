﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quik_BookingApp.DAO;

#nullable disable

namespace QuikBookingApp.Migrations
{
    [DbContext(typeof(QuikDbContext))]
    partial class QuikDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Amenity", b =>
                {
                    b.Property<string>("AmenityId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AmenityText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AmenityId");

                    b.HasIndex("SpaceId");

                    b.ToTable("Amenities");

                    b.HasData(
                        new
                        {
                            AmenityId = "amenity1",
                            AmenityText = "Private bathroom",
                            SpaceId = "space006"
                        },
                        new
                        {
                            AmenityId = "amenity2",
                            AmenityText = "Free beverages",
                            SpaceId = "space006"
                        },
                        new
                        {
                            AmenityId = "amenity3",
                            AmenityText = "24/7 access",
                            SpaceId = "space007"
                        },
                        new
                        {
                            AmenityId = "amenity4",
                            AmenityText = "Free parking",
                            SpaceId = "space008"
                        });
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Booking", b =>
                {
                    b.Property<string>("BookingId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfPeople")
                        .HasColumnType("int");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("RemainingAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SpaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BookingId");

                    b.HasIndex("SpaceId");

                    b.HasIndex("Username");

                    b.ToTable("Bookings");

                    b.HasData(
                        new
                        {
                            BookingId = "booking002",
                            BookingDate = new DateTime(2024, 10, 20, 7, 49, 2, 53, DateTimeKind.Local).AddTicks(6216),
                            DepositAmount = 30000m,
                            EndTime = new DateTime(2024, 10, 21, 11, 49, 2, 53, DateTimeKind.Local).AddTicks(6233),
                            NumberOfPeople = 2,
                            PaymentId = new Guid("81758fad-148f-4928-83e5-cf60184c1523"),
                            RemainingAmount = 170000m,
                            SpaceId = "space006",
                            StartTime = new DateTime(2024, 10, 21, 9, 49, 2, 53, DateTimeKind.Local).AddTicks(6232),
                            Status = "Pending",
                            TotalAmount = 200000m,
                            Username = "bob_member"
                        },
                        new
                        {
                            BookingId = "booking003",
                            BookingDate = new DateTime(2024, 10, 19, 7, 49, 2, 53, DateTimeKind.Local).AddTicks(6237),
                            DepositAmount = 40000m,
                            EndTime = new DateTime(2024, 10, 21, 12, 49, 2, 53, DateTimeKind.Local).AddTicks(6238),
                            NumberOfPeople = 3,
                            PaymentId = new Guid("8e2e9ba0-997a-4ea5-bf1b-8a636959f880"),
                            RemainingAmount = 210000m,
                            SpaceId = "space007",
                            StartTime = new DateTime(2024, 10, 21, 10, 49, 2, 53, DateTimeKind.Local).AddTicks(6238),
                            Status = "Confirmed",
                            TotalAmount = 250000m,
                            Username = "alice_admin"
                        });
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Business", b =>
                {
                    b.Property<string>("BusinessId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BusinessName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.HasKey("BusinessId");

                    b.ToTable("Businesses");

                    b.HasData(
                        new
                        {
                            BusinessId = "business002",
                            BusinessName = "Workspace Deluxe",
                            Description = "A deluxe workspace offering premium services.",
                            Email = "contact@workspace-deluxe.com",
                            Location = "456 Elm Street",
                            Password = "hashedpassword",
                            PhoneNumber = "987654321",
                            Rating = 4f
                        },
                        new
                        {
                            BusinessId = "business003",
                            BusinessName = "Startup Hub",
                            Description = "An energetic space for young startups.",
                            Email = "info@startup-hub.com",
                            Location = "789 Startup Blvd",
                            Password = "hashedpassword123",
                            PhoneNumber = "123456987",
                            Rating = 5f
                        },
                        new
                        {
                            BusinessId = "business004",
                            BusinessName = "Freelancers Corner",
                            Description = "A cozy spot for freelancers.",
                            Email = "freelancers@corner.com",
                            Location = "101 Freelance Road",
                            Password = "hashedpassword789",
                            PhoneNumber = "654321987",
                            Rating = 4f
                        });
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.ImageWS", b =>
                {
                    b.Property<string>("ImageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WSCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("WSImages")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("WorkingSpaceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImageId");

                    b.HasIndex("SpaceId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.OtpManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("OtpText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtpType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OtpManagers");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("BookingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VNPayResponseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VNPayTransactionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentId");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.ToTable("Payments");

                    b.HasData(
                        new
                        {
                            PaymentId = new Guid("56cc030e-86ff-4f6e-a6df-053218c77d5e"),
                            Amount = 30000.0,
                            BookingId = "booking002",
                            PaymentDate = new DateTime(2024, 10, 21, 7, 49, 2, 53, DateTimeKind.Local).AddTicks(6258),
                            PaymentMethod = "PayPal",
                            PaymentStatus = "Pending",
                            PaymentUrl = "payment002@example.com",
                            VNPayResponseCode = "OK",
                            VNPayTransactionId = "VNPay002"
                        },
                        new
                        {
                            PaymentId = new Guid("b93fb0bf-2858-449c-907b-1897f6841f1e"),
                            Amount = 40000.0,
                            BookingId = "booking003",
                            PaymentDate = new DateTime(2024, 10, 21, 7, 49, 2, 53, DateTimeKind.Local).AddTicks(6263),
                            PaymentMethod = "Credit Card",
                            PaymentStatus = "Success",
                            PaymentUrl = "payment003@example.com",
                            VNPayResponseCode = "OK",
                            VNPayTransactionId = "VNPay003"
                        });
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.PwdManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PwdManagers");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Review", b =>
                {
                    b.Property<Guid>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("SpaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReviewId");

                    b.HasIndex("SpaceId");

                    b.HasIndex("Username");

                    b.ToTable("Reviews");

                    b.HasData(
                        new
                        {
                            ReviewId = new Guid("58ff3f9f-a5a7-4b9b-8584-bb6f3ddc866e"),
                            Comment = "Amazing experience, highly recommend!",
                            CreatedAt = new DateTime(2024, 10, 16, 7, 49, 2, 53, DateTimeKind.Local).AddTicks(6278),
                            Rating = 5f,
                            SpaceId = "space006",
                            Title = "Fantastic Experience",
                            Username = "alice_admin"
                        },
                        new
                        {
                            ReviewId = new Guid("68f38126-8cad-4f81-9ab5-96c9c01fb3b5"),
                            Comment = "Great place for team collaboration!",
                            CreatedAt = new DateTime(2024, 10, 18, 7, 49, 2, 53, DateTimeKind.Local).AddTicks(6280),
                            Rating = 4f,
                            SpaceId = "space007",
                            Title = "Good Collaboration Space",
                            Username = "bob_member"
                        },
                        new
                        {
                            ReviewId = new Guid("77b55126-b082-4400-8d02-fa38886d9864"),
                            Comment = "Nice and quiet workspace.",
                            CreatedAt = new DateTime(2024, 10, 20, 7, 49, 2, 53, DateTimeKind.Local).AddTicks(6283),
                            Rating = 4f,
                            SpaceId = "space008",
                            Title = "Quiet Workspace",
                            Username = "charlie_business"
                        });
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.TblRefreshToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TokenId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "TokenId");

                    b.ToTable("TblRefreshtokens");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Tempuser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tempusers");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OTPVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Username = "alice_admin",
                            Email = "alice.admin@example.com",
                            ImageId = "img003",
                            IsActive = true,
                            IsLocked = false,
                            Name = "Alice Admin",
                            OTPVerified = true,
                            Password = "hashedpassword789",
                            PhoneNumber = "1231231234",
                            Role = "Admin",
                            Status = "Active"
                        },
                        new
                        {
                            Username = "bob_member",
                            Email = "bob.member@example.com",
                            ImageId = "img004",
                            IsActive = true,
                            IsLocked = false,
                            Name = "Bob Member",
                            OTPVerified = true,
                            Password = "hashedpassword789",
                            PhoneNumber = "3213214321",
                            Role = "Member",
                            Status = "Active"
                        },
                        new
                        {
                            Username = "charlie_business",
                            Email = "charlie.business@example.com",
                            ImageId = "img005",
                            IsActive = true,
                            IsLocked = false,
                            Name = "Charlie Business",
                            OTPVerified = true,
                            Password = "hashedpassword789",
                            PhoneNumber = "6549871230",
                            Role = "Business",
                            Status = "Active"
                        },
                        new
                        {
                            Username = "david_user",
                            Email = "david.user@example.com",
                            ImageId = "img006",
                            IsActive = true,
                            IsLocked = false,
                            Name = "David User",
                            OTPVerified = true,
                            Password = "hashedpassword321",
                            PhoneNumber = "9876543210",
                            Role = "User",
                            Status = "Active"
                        });
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.WorkingSpace", b =>
                {
                    b.Property<string>("SpaceId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BusinessId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PricePerHour")
                        .HasColumnType("decimal(18,2)");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("RoomType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SpaceId");

                    b.HasIndex("BusinessId");

                    b.ToTable("WorkingSpaces");

                    b.HasData(
                        new
                        {
                            SpaceId = "space006",
                            BusinessId = "business002",
                            Capacity = 3,
                            Description = "An executive office with all luxury amenities, including high-speed internet, ergonomic furniture, and personalized services to enhance productivity and comfort. Ideal for high-stakes meetings and presentations.",
                            ImageId = "img_space006",
                            Location = "456 Elm Street, Room 101",
                            PricePerHour = 100000m,
                            Rating = 4.5f,
                            RoomType = "Executive",
                            Title = "VIP Executive Office"
                        },
                        new
                        {
                            SpaceId = "space007",
                            BusinessId = "business003",
                            Capacity = 5,
                            Description = "Perfect for innovative teams, this lab offers a vibrant atmosphere with collaborative spaces, whiteboards, and high-speed internet. It's designed to foster creativity and teamwork, helping your startup thrive.",
                            ImageId = "img_space007",
                            Location = "789 Startup Blvd, Room 303",
                            PricePerHour = 20000m,
                            Rating = 4.5f,
                            RoomType = "Lab",
                            Title = "Startup Lab"
                        },
                        new
                        {
                            SpaceId = "space008",
                            BusinessId = "business004",
                            Capacity = 6,
                            Description = "An open studio designed for remote workers, featuring a relaxed environment with natural light, comfortable seating, and high-speed internet. It's the perfect place for freelancers to get work done efficiently.",
                            ImageId = "img_space008",
                            Location = "101 Freelance Road, Room 102",
                            PricePerHour = 15000m,
                            Rating = 4.3f,
                            RoomType = "Studio",
                            Title = "Freelance Studio"
                        });
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Amenity", b =>
                {
                    b.HasOne("Quik_BookingApp.DAO.Models.WorkingSpace", null)
                        .WithMany("Amenities")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Booking", b =>
                {
                    b.HasOne("Quik_BookingApp.DAO.Models.WorkingSpace", "WorkingSpace")
                        .WithMany("Bookings")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Quik_BookingApp.DAO.Models.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WorkingSpace");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.ImageWS", b =>
                {
                    b.HasOne("Quik_BookingApp.DAO.Models.WorkingSpace", "WorkingSpace")
                        .WithMany("Images")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkingSpace");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Payment", b =>
                {
                    b.HasOne("Quik_BookingApp.DAO.Models.Booking", "Booking")
                        .WithOne("Payment")
                        .HasForeignKey("Quik_BookingApp.DAO.Models.Payment", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Review", b =>
                {
                    b.HasOne("Quik_BookingApp.DAO.Models.WorkingSpace", "WorkingSpace")
                        .WithMany("Reviews")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quik_BookingApp.DAO.Models.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WorkingSpace");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.WorkingSpace", b =>
                {
                    b.HasOne("Quik_BookingApp.DAO.Models.Business", "Business")
                        .WithMany("WorkingSpaces")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Booking", b =>
                {
                    b.Navigation("Payment")
                        .IsRequired();
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.Business", b =>
                {
                    b.Navigation("WorkingSpaces");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Quik_BookingApp.DAO.Models.WorkingSpace", b =>
                {
                    b.Navigation("Amenities");

                    b.Navigation("Bookings");

                    b.Navigation("Images");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
