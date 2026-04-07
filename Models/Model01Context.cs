using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace EpsteinsMarket.Models
{
    // Database First: контекст и сущности проекта используются через AppConnect.model01.
    public partial class model01Entities : DbContext
    {
        public model01Entities() : this("name=model01Entities")
        {
        }

        public model01Entities(string connectionStringOrName) : base(connectionStringOrName)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<UserAddress> UserAddresses { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    }

    [Table("Users")]
    public partial class User
    {
        [Key]
        [Column("UserID")]
        public int UserID { get; set; }

        [Required]
        [Column("FullName")]
        [MaxLength(100)]
        public string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? Experience { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(20)]
        public string Role { get; set; }
    }

    [Table("Products")]
    public partial class Product
    {
        [Key]
        [Column("ProductID")]
        public int ID { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        [MaxLength(500)]
        public string Image { get; set; }

        public int? CategoryID { get; set; }
    }

    [Table("Categories")]
    public partial class Category
    {
        [Key]
        [Column("CategoryID")]
        public int ID { get; set; }

        [Required]
        [Column("CategoryName")]
        [MaxLength(150)]
        public string Name { get; set; }
    }

    [Table("Favorites")]
    public partial class Favorite
    {
        [Key]
        [Column("FavoriteID")]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int ProductID { get; set; }
    }

    [Table("Suppliers")]
    public partial class Supplier
    {
        [Key]
        [Column("SupplierID")]
        public int ID { get; set; }

        [Required]
        [MaxLength(150)]
        public string SupplierName { get; set; }

        [MaxLength(100)]
        public string ContactName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }
    }

    [Table("UserAddresses")]
    public partial class UserAddress
    {
        [Key]
        [Column("AddressID")]
        public int ID { get; set; }

        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(150)]
        public string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public string Building { get; set; }

        [MaxLength(20)]
        public string Apartment { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }

        public bool IsPrimary { get; set; }
    }

    [Table("Reviews")]
    public partial class Review
    {
        [Key]
        [Column("ReviewID")]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int ProductID { get; set; }

        public byte Rating { get; set; }

        [MaxLength(1000)]
        public string ReviewText { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    [Table("ProductSuppliers")]
    public partial class ProductSupplier
    {
        [Key]
        [Column("ProductSupplierID")]
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int SupplierID { get; set; }

        public decimal PurchasePrice { get; set; }

        public int LeadTimeDays { get; set; }
    }

    [Table("Orders")]
    public partial class Order
    {
        [Key]
        [Column("OrderID")]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int? AddressID { get; set; }

        public DateTime OrderDate { get; set; }

        [MaxLength(30)]
        public string Status { get; set; }

        public decimal TotalAmount { get; set; }
    }

    [Table("OrderItems")]
    public partial class OrderItem
    {
        [Key]
        [Column("OrderItemID")]
        public int ID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }

    [Table("PaymentTransactions")]
    public partial class PaymentTransaction
    {
        [Key]
        [Column("TransactionID")]
        public int ID { get; set; }

        public int OrderID { get; set; }

        [Required]
        [MaxLength(30)]
        public string PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        [Required]
        [MaxLength(30)]
        public string PaymentStatus { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}
