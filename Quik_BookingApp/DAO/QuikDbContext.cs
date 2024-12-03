using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Quik_BookingApp.DAO.Models;

namespace Quik_BookingApp.DAO
{
    public class QuikDbContext : DbContext
    {
        public QuikDbContext() { }

        public QuikDbContext(DbContextOptions<QuikDbContext> options) : base(options) 
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if(databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<WorkingSpace> WorkingSpaces { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TblRefreshToken> TblRefreshtokens { get; set; }
        public DbSet<ImageWS> Images { get; set; }
        public DbSet<Tempuser> Tempusers { get; set; }
        public DbSet<OtpManager> OtpManagers { get; set; }
        public DbSet<PwdManager> PwdManagers { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa khóa chính cho các thực thể
            modelBuilder.Entity<User>().HasKey(u => u.Username);
            modelBuilder.Entity<Business>().HasKey(b => b.BusinessId);
            modelBuilder.Entity<WorkingSpace>().HasKey(ws => ws.SpaceId);
            modelBuilder.Entity<Booking>().HasKey(b => b.BookingId);
            modelBuilder.Entity<Payment>().HasKey(p => p.PaymentId);
            modelBuilder.Entity<ImageWS>().HasKey(iws => iws.ImageId);
            modelBuilder.Entity<Tempuser>().HasKey(tu => tu.Id);
            modelBuilder.Entity<OtpManager>().HasKey(om => om.Id);
            modelBuilder.Entity<PwdManager>().HasKey(pm => pm.Id);
            modelBuilder.Entity<Amenity>().HasKey(a => a.AmenityId);
            modelBuilder.Entity<TblRefreshToken>().HasKey(rt => new { rt.UserId, rt.TokenId });
            modelBuilder.Entity<Review>().HasKey(rt => rt.ReviewId);


            modelBuilder.Entity<WorkingSpace>()
                .HasOne(ws => ws.Business)
                .WithMany(b => b.WorkingSpaces)
                .HasForeignKey(ws => ws.BusinessId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkingSpace>()
                .HasMany(ws => ws.Images)
                .WithOne(iws => iws.WorkingSpace)
                .HasForeignKey(iws => iws.SpaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkingSpace>()
                .HasMany(ws => ws.Amenities)
                .WithOne()
                .HasForeignKey(a => a.SpaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.Username)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.WorkingSpace)
                .WithMany(ws => ws.Bookings)
                .HasForeignKey(b => b.SpaceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews) // A user can have many reviews
                .HasForeignKey(r => r.Username)
                .OnDelete(DeleteBehavior.NoAction); // No cascading delete

            modelBuilder.Entity<Review>()
                .HasOne(r => r.WorkingSpace)
                .WithMany(ws => ws.Reviews) // A working space can have many reviews
                .HasForeignKey(r => r.SpaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasData(
                 new User
                 {
                     Username = "alice_admin",
                     Name = "Alice Admin",
                     Email = "alice.admin@example.com",
                     Password = "hashedpassword789",
                     Role = "Admin",
                     ImageId = "img003",
                     PhoneNumber = "1231231234",
                     OTPVerified = true,
                     IsActive = true,
                     IsLocked = false,
                     Status = "Active"
                 },
                 new User
                 {
                     Username = "bob_member",
                     Name = "Bob Member",
                     Email = "bob.member@example.com",
                     Password = "hashedpassword789",
                     Role = "Member",
                     ImageId = "img004",
                     PhoneNumber = "3213214321",
                     OTPVerified = true,
                     IsActive = true,
                     IsLocked = false,
                     Status = "Active"
                 },
                 new User
                 {
                     Username = "charlie_business",
                     Name = "Charlie Business",
                     Email = "charlie.business@example.com",
                     Password = "hashedpassword789",
                     Role = "Business",
                     ImageId = "img005",
                     PhoneNumber = "6549871230",
                     OTPVerified = true,
                     IsActive = true,
                     IsLocked = false,
                     Status = "Active"
                 },
                 new User
                 {
                     Username = "david_user",
                     Name = "David User",
                     Email = "david.user@example.com",
                     Password = "hashedpassword321",
                     Role = "User",
                     ImageId = "img006",
                     PhoneNumber = "9876543210",
                     OTPVerified = true,
                     IsActive = true,
                     IsLocked = false,
                     Status = "Active"
                 }
             );

            // Seed thêm dữ liệu cho Business
            modelBuilder.Entity<Business>().HasData(
                new Business
                {
                    BusinessId = "business001",
                    BusinessName = "Deer Coffee",
                    PhoneNumber = "1234567890",
                    Email = "contact@deercoffee.com",
                    Password = "hashedpassword001",
                    Location = "Quận 9 - Thủ Đức, S202 Vinhomes Grand Park",
                    Description = "Working cafe",
                    Rating = 4
                },
                new Business
                {
                    BusinessId = "business002",
                    BusinessName = "Ori Working",
                    PhoneNumber = "0987654321",
                    Email = "info@oriworking.com",
                    Password = "hashedpassword002",
                    Location = "Quận 9 - Thủ Đức, S802 Vinhomes Grand Park",
                    Description = "Coworking space",
                    Rating = 5
                },
                new Business
                {
                    BusinessId = "business003",
                    BusinessName = "Work flow",
                    PhoneNumber = "1122334455",
                    Email = "contact@workflow.com",
                    Password = "hashedpassword003",
                    Location = "Quận 3, 193 Hai Bà Trưng, Phường 6",
                    Description = "Working cafe",
                    Rating = 4.5f
                }
            );

            // Seed thêm dữ liệu cho WorkingSpace
            modelBuilder.Entity<WorkingSpace>().HasData(
                new WorkingSpace
                {
                    SpaceId = "space001",
                    ImageId = "img_space001",
                    BusinessId = "business001",
                    Title = "Deer Coffee Workspace",
                    Description = "Cozy workspace with a productive atmosphere and excellent coffee.",
                    PricePerHour = 50000,
                    RoomType = "Working Cafe",
                    Capacity = 20,
                    Location = "S202 Vinhomes Grand Park, Quận 9 - Thủ Đức",
                    Rating = 4.2f
                },
                new WorkingSpace
                {
                    SpaceId = "space002",
                    ImageId = "img_space002",
                    BusinessId = "business002",
                    Title = "Ori Coworking Space",
                    Description = "Modern coworking space with high-speed internet and collaborative atmosphere.",
                    PricePerHour = 80000,
                    RoomType = "Coworking Space",
                    Capacity = 50,
                    Location = "S802 Vinhomes Grand Park, Quận 9 - Thủ Đức",
                    Rating = 4.8f
                },
                new WorkingSpace
                {
                    SpaceId = "space003",
                    ImageId = "img_space003",
                    BusinessId = "business003",
                    Title = "Flow Workspace",
                    Description = "Conveniently located workspace with ample seating and natural lighting.",
                    PricePerHour = 60000,
                    RoomType = "Working Cafe",
                    Capacity = 30,
                    Location = "193 Hai Bà Trưng, Phường 6, Quận 3",
                    Rating = 4.3f
                }
            );


            // Seed thêm dữ liệu cho Amenity
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity
                {
                    SpaceId = "space006",
                    AmenityId = "amenity1",
                    AmenityText = "Private bathroom"
                },
                new Amenity
                {
                    SpaceId = "space006",
                    AmenityId = "amenity2",
                    AmenityText = "Free beverages"
                },
                new Amenity
                {
                    SpaceId = "space007",
                    AmenityId = "amenity3",
                    AmenityText = "24/7 access"
                },
                new Amenity
                {
                    SpaceId = "space008",
                    AmenityId = "amenity4",
                    AmenityText = "Free parking"
                }
            );

            // Seed thêm dữ liệu cho Booking
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = "booking002",
                    PaymentId = Guid.NewGuid(),
                    Username = "bob_member",
                    SpaceId = "space006",
                    BookingDate = DateTime.Now.AddDays(-1),
                    StartTime = DateTime.Now.AddHours(2),
                    EndTime = DateTime.Now.AddHours(4),
                    NumberOfPeople = 2,
                    TotalAmount = 200000,
                    DepositAmount = 30000,
                    RemainingAmount = 170000,
                    Status = "Pending"
                },
                new Booking
                {
                    BookingId = "booking003",
                    PaymentId = Guid.NewGuid(),
                    Username = "alice_admin",
                    SpaceId = "space007",
                    BookingDate = DateTime.Now.AddDays(-2),
                    StartTime = DateTime.Now.AddHours(3),
                    EndTime = DateTime.Now.AddHours(5),
                    NumberOfPeople = 3,
                    TotalAmount = 250000,
                    DepositAmount = 40000,
                    RemainingAmount = 210000,
                    Status = "Confirmed"
                }
            );

            // Seed thêm dữ liệu cho Payment
            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    PaymentId = Guid.NewGuid(),
                    BookingId = "booking002",
                    Amount = 30000,
                    PaymentMethod = "PayPal",
                    PaymentDate = DateTime.Now,
                    PaymentStatus = "Pending",
                    VNPayTransactionId = "VNPay002",
                    VNPayResponseCode = "OK",
                    PaymentUrl = "payment002@example.com"
                },
                new Payment
                {
                    PaymentId = Guid.NewGuid(),
                    BookingId = "booking003",
                    Amount = 40000,
                    PaymentMethod = "Credit Card",
                    PaymentDate = DateTime.Now,
                    PaymentStatus = "Success",
                    VNPayTransactionId = "VNPay003",
                    VNPayResponseCode = "OK",
                    PaymentUrl = "payment003@example.com"
                }
            );

        modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    ReviewId = Guid.NewGuid(),
                    Username = "alice_admin",
                    SpaceId = "space006",
                    Rating = 5,
                    Comment = "Amazing experience, highly recommend!",
                    Title = "Fantastic Experience", // New title field
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new Review
                {
                    ReviewId = Guid.NewGuid(),
                    Username = "bob_member",
                    SpaceId = "space007",
                    Rating = 4,
                    Comment = "Great place for team collaboration!",
                    Title = "Good Collaboration Space", // New title field
                    CreatedAt = DateTime.Now.AddDays(-3)
                },
                new Review
                {
                    ReviewId = Guid.NewGuid(),
                    Username = "charlie_business",
                    SpaceId = "space008",
                    Rating = 4,
                    Comment = "Nice and quiet workspace.",
                    Title = "Quiet Workspace", // New title field
                    CreatedAt = DateTime.Now.AddDays(-1)
                }
            );

        }
    }
}
