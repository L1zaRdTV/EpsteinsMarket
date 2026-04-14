using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace EpsteinMarket.ApplicationData
{
    public class EpsteinMarketDBEntities : DbContext
    {
        public EpsteinMarketDBEntities() : base("name=EpsteinMarketDBEntities")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<ProductStatuses> ProductStatuses { get; set; }
        public virtual DbSet<OrderStatuses> OrderStatuses { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Receipts> Receipts { get; set; }
        public virtual DbSet<TelegramLinks> TelegramLinks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasRequired(x => x.UserRoles)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasRequired(x => x.Categories)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasRequired(x => x.Brands)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasRequired(x => x.ProductStatuses)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.StatusID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductImages>()
                .HasRequired(x => x.Products)
                .WithMany(x => x.ProductImages)
                .HasForeignKey(x => x.ProductID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Carts>()
                .HasRequired(x => x.Users)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.UserID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CartItems>()
                .HasRequired(x => x.Carts)
                .WithMany(x => x.CartItems)
                .HasForeignKey(x => x.CartID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CartItems>()
                .HasRequired(x => x.Products)
                .WithMany(x => x.CartItems)
                .HasForeignKey(x => x.ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orders>()
                .HasRequired(x => x.Users)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orders>()
                .HasRequired(x => x.OrderStatuses)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.OrderStatusID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderItems>()
                .HasRequired(x => x.Orders)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<OrderItems>()
                .HasRequired(x => x.Products)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Receipts>()
                .HasRequired(x => x.Orders)
                .WithMany(x => x.Receipts)
                .HasForeignKey(x => x.OrderID)
                .WillCascadeOnDelete(true);
        }
    }

    public class UserRoles
    {
        public UserRoles()
        {
            Users = new HashSet<Users>();
        }

        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }

    public class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }

    public class Brands
    {
        public Brands()
        {
            Products = new HashSet<Products>();
        }

        public int BrandID { get; set; }
        public string BrandName { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }

    public class ProductStatuses
    {
        public ProductStatuses()
        {
            Products = new HashSet<Products>();
        }

        public int StatusID { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }

    public class OrderStatuses
    {
        public OrderStatuses()
        {
            Orders = new HashSet<Orders>();
        }

        public int OrderStatusID { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }

    public class Users
    {
        public Users()
        {
            Carts = new HashSet<Carts>();
            Orders = new HashSet<Orders>();
        }

        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleID { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsBlocked { get; set; }

        public virtual UserRoles UserRoles { get; set; }
        public virtual ICollection<Carts> Carts { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }

    public class Products
    {
        public Products()
        {
            ProductImages = new HashSet<ProductImages>();
            CartItems = new HashSet<CartItems>();
            OrderItems = new HashSet<OrderItems>();
        }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int CategoryID { get; set; }
        public int StatusID { get; set; }
        public int BrandID { get; set; }
        public string MainImage { get; set; }

        public virtual Categories Categories { get; set; }
        public virtual ProductStatuses ProductStatuses { get; set; }
        public virtual Brands Brands { get; set; }
        public virtual ICollection<ProductImages> ProductImages { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }

    public class ProductImages
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string ImagePath { get; set; }

        public virtual Products Products { get; set; }
    }

    public class Carts
    {
        public Carts()
        {
            CartItems = new HashSet<CartItems>();
        }

        public int CartID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Users Users { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
    }

    public class CartItems
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtMoment { get; set; }

        public virtual Carts Carts { get; set; }
        public virtual Products Products { get; set; }
    }

    public class Orders
    {
        public Orders()
        {
            OrderItems = new HashSet<OrderItems>();
            Receipts = new HashSet<Receipts>();
        }

        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int OrderStatusID { get; set; }

        public virtual Users Users { get; set; }
        public virtual OrderStatuses OrderStatuses { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<Receipts> Receipts { get; set; }
    }

    public class OrderItems
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtMoment { get; set; }

        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }

    public class Receipts
    {
        public int ReceiptID { get; set; }
        public int OrderID { get; set; }
        public string PdfPath { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Orders Orders { get; set; }
    }

    public class TelegramLinks
    {
        public int TelegramLinkID { get; set; }
        public string Url { get; set; }
        public string QrImagePath { get; set; }
        public bool IsActive { get; set; }
    }
}
