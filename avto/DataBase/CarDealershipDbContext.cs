using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using avto.Pages;
using System.Data;

namespace avto.DataBase
{
    public class CarDealershipDbContext : DbContext
    {
        public CarDealershipDbContext(DbContextOptions<CarDealershipDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatusTracking> OrderStatusTrackings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация сущностей

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.RoleName).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Address).HasColumnType("text");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);
                entity.HasOne(e => e.User).WithMany(u => u.UserRoles).HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Role).WithMany().HasForeignKey(e => e.RoleId);
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.CarId);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.Vin).IsRequired().HasMaxLength(17);
                entity.HasIndex(e => e.Vin).IsUnique();
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(10,2)");
                entity.Property(e => e.Available).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.Characteristics).HasColumnType("text");
                entity.Property(e => e.Category).IsRequired();
            });

            modelBuilder.Entity<CarImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.HasOne(e => e.Car).WithMany(c => c.CarImages).HasForeignKey(e => e.CarId);
            });

            modelBuilder.Entity<InventoryMovement>(entity =>
            {
                entity.HasKey(e => e.MovementId);
                entity.Property(e => e.MovementType).IsRequired();
                entity.Property(e => e.Date).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.Quantity).IsRequired();
                entity.HasOne(e => e.Car).WithMany().HasForeignKey(e => e.CarId);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Address).HasColumnType("text");
                entity.HasOne(e => e.User)
             .WithMany() // Если у пользователя есть коллекция клиентов, то можно указать .WithMany(u => u.Clients)
             .HasForeignKey(e => e.UserId)
             .OnDelete(DeleteBehavior.SetNull);

            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.Status).IsRequired();
                entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientId);
                entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(10,2)");
                entity.HasOne(e => e.Order).WithMany(o => o.OrderDetails).HasForeignKey(e => e.OrderId);
                entity.HasOne(e => e.Car).WithMany().HasForeignKey(e => e.CarId);
            });

            modelBuilder.Entity<OrderStatusTracking>(entity =>
            {
                entity.HasKey(e => e.StatusId);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.HasOne(e => e.Order).WithMany(o => o.OrderStatusTrackings).HasForeignKey(e => e.OrderId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }

    public class Car
    {
        public int CarId { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Vin { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }
        public string Characteristics { get; set; }
        public CarCategory Category { get; set; }

        public List<CarImage> CarImages { get; set; } = new List<CarImage>();
    }

    public enum CarCategory
    {
        Elite,
        Economy,
        Comfort
    }

    public class CarImage
    {
        public int ImageId { get; set; }
        public int CarId { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        public Car Car { get; set; }
    }

    public class InventoryMovement
    {
        public int MovementId { get; set; }
        public int CarId { get; set; }
        public MovementType MovementType { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }

        public Car Car { get; set; }
    }

    public enum MovementType
    {
        Incoming,
        Sale,
        Return
    }

    public class Client
    {
        public int ClientId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string? Message { get; set; }  // Новое поле для хранения сообщения
        public User User { get; set; }


    }
    public class Order
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }

        public Client Client { get; set; }
        public User User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public List<OrderStatusTracking> OrderStatusTrackings { get; set; } = new List<OrderStatusTracking>();
    }

    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled
    }

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int CarId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Order Order { get; set; }
        public Car Car { get; set; }
    }

    public class OrderStatusTracking
    {
        public int StatusId { get; set; }
        public int OrderId { get; set; }
        public TrackingStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Order Order { get; set; }
    }

    public enum TrackingStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}