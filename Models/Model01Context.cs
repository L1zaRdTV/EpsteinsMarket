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
}
