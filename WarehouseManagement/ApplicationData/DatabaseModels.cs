using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace WarehouseManagement.ApplicationData
{
    public class WarehouseDBEntitiesContext : DbContext
    {
        public WarehouseDBEntitiesContext() : base("name=WarehouseDBEntities")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Warehouses> Warehouses { get; set; }
        public virtual DbSet<StorageZones> StorageZones { get; set; }
        public virtual DbSet<StorageLocations> StorageLocations { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<StockBalances> StockBalances { get; set; }
        public virtual DbSet<Receipts> Receipts { get; set; }
        public virtual DbSet<ReceiptItems> ReceiptItems { get; set; }
        public virtual DbSet<Shipments> Shipments { get; set; }
        public virtual DbSet<ShipmentItems> ShipmentItems { get; set; }
        public virtual DbSet<Inventories> Inventories { get; set; }
        public virtual DbSet<InventoryResults> InventoryResults { get; set; }
        public virtual DbSet<ActionLogs> ActionLogs { get; set; }
        public virtual DbSet<EquipmentTypes> EquipmentTypes { get; set; }
        public virtual DbSet<WarehouseEquipment> WarehouseEquipment { get; set; }
        public virtual DbSet<MaintenancePlans> MaintenancePlans { get; set; }
        public virtual DbSet<QualityChecks> QualityChecks { get; set; }
        public virtual DbSet<ProductionLines> ProductionLines { get; set; }
        public virtual DbSet<WorkOrders> WorkOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasRequired(x => x.UserRoles).WithMany(x => x.Users).HasForeignKey(x => x.RoleID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Users>().HasOptional(x => x.Employees).WithMany(x => x.Users).HasForeignKey(x => x.EmployeeID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Products>().HasRequired(x => x.Categories).WithMany(x => x.Products).HasForeignKey(x => x.CategoryID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Products>().HasRequired(x => x.Units).WithMany(x => x.Products).HasForeignKey(x => x.UnitID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Products>().HasRequired(x => x.Suppliers).WithMany(x => x.Products).HasForeignKey(x => x.SupplierID).WillCascadeOnDelete(false);
            modelBuilder.Entity<StorageZones>().HasRequired(x => x.Warehouses).WithMany(x => x.StorageZones).HasForeignKey(x => x.WarehouseID).WillCascadeOnDelete(false);
            modelBuilder.Entity<StorageLocations>().HasRequired(x => x.StorageZones).WithMany(x => x.StorageLocations).HasForeignKey(x => x.ZoneID).WillCascadeOnDelete(false);
            modelBuilder.Entity<StockBalances>().HasRequired(x => x.Products).WithMany(x => x.StockBalances).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
            modelBuilder.Entity<StockBalances>().HasRequired(x => x.StorageLocations).WithMany(x => x.StockBalances).HasForeignKey(x => x.LocationID).WillCascadeOnDelete(false);
            modelBuilder.Entity<ReceiptItems>().HasRequired(x => x.Receipts).WithMany(x => x.ReceiptItems).HasForeignKey(x => x.ReceiptID).WillCascadeOnDelete(true);
            modelBuilder.Entity<ReceiptItems>().HasRequired(x => x.Products).WithMany(x => x.ReceiptItems).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
            modelBuilder.Entity<ShipmentItems>().HasRequired(x => x.Shipments).WithMany(x => x.ShipmentItems).HasForeignKey(x => x.ShipmentID).WillCascadeOnDelete(true);
            modelBuilder.Entity<ShipmentItems>().HasRequired(x => x.Products).WithMany(x => x.ShipmentItems).HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
            modelBuilder.Entity<InventoryResults>().HasRequired(x => x.Inventories).WithMany(x => x.InventoryResults).HasForeignKey(x => x.InventoryID).WillCascadeOnDelete(true);
            modelBuilder.Entity<InventoryResults>().HasRequired(x => x.StockBalances).WithMany(x => x.InventoryResults).HasForeignKey(x => x.BalanceID).WillCascadeOnDelete(false);
            modelBuilder.Entity<ActionLogs>().HasOptional(x => x.Users).WithMany(x => x.ActionLogs).HasForeignKey(x => x.UserID).WillCascadeOnDelete(false);
            modelBuilder.Entity<WarehouseEquipment>().HasRequired(x => x.Warehouses).WithMany().HasForeignKey(x => x.WarehouseID).WillCascadeOnDelete(false);
            modelBuilder.Entity<WarehouseEquipment>().HasRequired(x => x.EquipmentTypes).WithMany(x => x.WarehouseEquipment).HasForeignKey(x => x.EquipmentTypeID).WillCascadeOnDelete(false);
            modelBuilder.Entity<MaintenancePlans>().HasRequired(x => x.WarehouseEquipment).WithMany(x => x.MaintenancePlans).HasForeignKey(x => x.EquipmentID).WillCascadeOnDelete(false);
            modelBuilder.Entity<QualityChecks>().HasRequired(x => x.Products).WithMany().HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
            modelBuilder.Entity<QualityChecks>().HasRequired(x => x.Employees).WithMany().HasForeignKey(x => x.EmployeeID).WillCascadeOnDelete(false);
            modelBuilder.Entity<ProductionLines>().HasRequired(x => x.Warehouses).WithMany().HasForeignKey(x => x.WarehouseID).WillCascadeOnDelete(false);
            modelBuilder.Entity<WorkOrders>().HasRequired(x => x.ProductionLines).WithMany(x => x.WorkOrders).HasForeignKey(x => x.LineID).WillCascadeOnDelete(false);
            modelBuilder.Entity<WorkOrders>().HasRequired(x => x.Products).WithMany().HasForeignKey(x => x.ProductID).WillCascadeOnDelete(false);
            modelBuilder.Entity<WorkOrders>().HasRequired(x => x.Employees).WithMany().HasForeignKey(x => x.EmployeeID).WillCascadeOnDelete(false);
        }
    }

    public class UserRoles { public UserRoles(){Users=new HashSet<Users>();} [Key] public int RoleID{get;set;} public string RoleName{get;set;} public string Description{get;set;} public virtual ICollection<Users> Users{get;set;} }
    public class Employees { public Employees(){Users=new HashSet<Users>(); Receipts=new HashSet<Receipts>(); Shipments=new HashSet<Shipments>(); Inventories=new HashSet<Inventories>();} [Key] public int EmployeeID{get;set;} public string FullName{get;set;} public string Position{get;set;} public string Phone{get;set;} public string Email{get;set;} public DateTime HireDate{get;set;} public bool IsActive{get;set;} public virtual ICollection<Users> Users{get;set;} public virtual ICollection<Receipts> Receipts{get;set;} public virtual ICollection<Shipments> Shipments{get;set;} public virtual ICollection<Inventories> Inventories{get;set;} }
    public class Users { public Users(){ActionLogs=new HashSet<ActionLogs>();} [Key] public int UserID{get;set;} public string FullName{get;set;} public string Login{get;set;} public string Password{get;set;} public string Email{get;set;} public string Phone{get;set;} public int RoleID{get;set;} public int? EmployeeID{get;set;} public DateTime CreatedAt{get;set;} public bool IsBlocked{get;set;} public virtual UserRoles UserRoles{get;set;} public virtual Employees Employees{get;set;} public virtual ICollection<ActionLogs> ActionLogs{get;set;} }
    public class Categories { public Categories(){Products=new HashSet<Products>();} [Key] public int CategoryID{get;set;} public string CategoryName{get;set;} public string Description{get;set;} public virtual ICollection<Products> Products{get;set;} }
    public class Units { public Units(){Products=new HashSet<Products>();} [Key] public int UnitID{get;set;} public string UnitName{get;set;} public string ShortName{get;set;} public virtual ICollection<Products> Products{get;set;} }
    public class Suppliers { public Suppliers(){Products=new HashSet<Products>(); Receipts=new HashSet<Receipts>();} [Key] public int SupplierID{get;set;} public string SupplierName{get;set;} public string ContactPerson{get;set;} public string Phone{get;set;} public string Email{get;set;} public string Address{get;set;} public virtual ICollection<Products> Products{get;set;} public virtual ICollection<Receipts> Receipts{get;set;} }
    public class Clients { public Clients(){Shipments=new HashSet<Shipments>();} [Key] public int ClientID{get;set;} public string ClientName{get;set;} public string ContactPerson{get;set;} public string Phone{get;set;} public string Email{get;set;} public string Address{get;set;} public virtual ICollection<Shipments> Shipments{get;set;} }
    public class Warehouses { public Warehouses(){StorageZones=new HashSet<StorageZones>();} [Key] public int WarehouseID{get;set;} public string WarehouseName{get;set;} public string Address{get;set;} public virtual ICollection<StorageZones> StorageZones{get;set;} }
    public class StorageZones { public StorageZones(){StorageLocations=new HashSet<StorageLocations>();} [Key] public int ZoneID{get;set;} public int WarehouseID{get;set;} public string ZoneName{get;set;} public string TemperatureMode{get;set;} public virtual Warehouses Warehouses{get;set;} public virtual ICollection<StorageLocations> StorageLocations{get;set;} }
    public class StorageLocations { public StorageLocations(){StockBalances=new HashSet<StockBalances>();} [Key] public int LocationID{get;set;} public int ZoneID{get;set;} public string LocationCode{get;set;} public decimal MaxWeight{get;set;} public virtual StorageZones StorageZones{get;set;} public virtual ICollection<StockBalances> StockBalances{get;set;} }
    public class Products { public Products(){StockBalances=new HashSet<StockBalances>(); ReceiptItems=new HashSet<ReceiptItems>(); ShipmentItems=new HashSet<ShipmentItems>();} [Key] public int ProductID{get;set;} public string Sku{get;set;} public string ProductName{get;set;} public string Description{get;set;} public decimal PurchasePrice{get;set;} public decimal SalePrice{get;set;} public int CategoryID{get;set;} public int UnitID{get;set;} public int SupplierID{get;set;} public int MinStock{get;set;} public bool IsActive{get;set;} public virtual Categories Categories{get;set;} public virtual Units Units{get;set;} public virtual Suppliers Suppliers{get;set;} [NotMapped] public decimal Price { get { return SalePrice; } set { SalePrice = value; } } [NotMapped] public int QuantityInStock { get { return StockBalances == null ? 0 : StockBalances.SumSafe(); } } public virtual ICollection<StockBalances> StockBalances{get;set;} public virtual ICollection<ReceiptItems> ReceiptItems{get;set;} public virtual ICollection<ShipmentItems> ShipmentItems{get;set;} }
    public static class BalanceExtensions { public static int SumSafe(this ICollection<StockBalances> balances){ int total=0; foreach(var b in balances) total += b.Quantity; return total; } }
    public class StockBalances { public StockBalances(){InventoryResults=new HashSet<InventoryResults>();} [Key] public int BalanceID{get;set;} public int ProductID{get;set;} public int LocationID{get;set;} public int Quantity{get;set;} public DateTime UpdatedAt{get;set;} public virtual Products Products{get;set;} public virtual StorageLocations StorageLocations{get;set;} public virtual ICollection<InventoryResults> InventoryResults{get;set;} }
    public class Receipts { public Receipts(){ReceiptItems=new HashSet<ReceiptItems>();} [Key] public int ReceiptID{get;set;} public string ReceiptNumber{get;set;} public int SupplierID{get;set;} public int EmployeeID{get;set;} public DateTime ReceiptDate{get;set;} public string Comment{get;set;} public virtual Suppliers Suppliers{get;set;} public virtual Employees Employees{get;set;} public virtual ICollection<ReceiptItems> ReceiptItems{get;set;} }
    public class ReceiptItems { [Key] public int ReceiptItemID{get;set;} public int ReceiptID{get;set;} public int ProductID{get;set;} public int LocationID{get;set;} public int Quantity{get;set;} public decimal UnitPrice{get;set;} public virtual Receipts Receipts{get;set;} public virtual Products Products{get;set;} public virtual StorageLocations StorageLocations{get;set;} }
    public class Shipments { public Shipments(){ShipmentItems=new HashSet<ShipmentItems>();} [Key] public int ShipmentID{get;set;} public string ShipmentNumber{get;set;} public int ClientID{get;set;} public int EmployeeID{get;set;} public DateTime ShipmentDate{get;set;} public string Comment{get;set;} public virtual Clients Clients{get;set;} public virtual Employees Employees{get;set;} public virtual ICollection<ShipmentItems> ShipmentItems{get;set;} }
    public class ShipmentItems { [Key] public int ShipmentItemID{get;set;} public int ShipmentID{get;set;} public int ProductID{get;set;} public int LocationID{get;set;} public int Quantity{get;set;} public decimal UnitPrice{get;set;} public virtual Shipments Shipments{get;set;} public virtual Products Products{get;set;} public virtual StorageLocations StorageLocations{get;set;} }
    public class Inventories { public Inventories(){InventoryResults=new HashSet<InventoryResults>();} [Key] public int InventoryID{get;set;} public string InventoryNumber{get;set;} public int WarehouseID{get;set;} public int EmployeeID{get;set;} public DateTime InventoryDate{get;set;} public string Status{get;set;} public virtual Warehouses Warehouses{get;set;} public virtual Employees Employees{get;set;} public virtual ICollection<InventoryResults> InventoryResults{get;set;} }
    public class InventoryResults { [Key] public int ResultID{get;set;} public int InventoryID{get;set;} public int BalanceID{get;set;} public int ExpectedQuantity{get;set;} public int ActualQuantity{get;set;} public int Difference{get;set;} public string Comment{get;set;} public virtual Inventories Inventories{get;set;} public virtual StockBalances StockBalances{get;set;} }
    public class ActionLogs { [Key] public int LogID{get;set;} public int? UserID{get;set;} public DateTime ActionDate{get;set;} public string ActionType{get;set;} public string EntityName{get;set;} public int? EntityID{get;set;} public string Details{get;set;} public virtual Users Users{get;set;} }

    public class EquipmentTypes { public EquipmentTypes(){WarehouseEquipment=new HashSet<WarehouseEquipment>();} [Key] public int EquipmentTypeID{get;set;} public string TypeName{get;set;} public int MaintenanceIntervalDays{get;set;} public virtual ICollection<WarehouseEquipment> WarehouseEquipment{get;set;} }
    public class WarehouseEquipment { public WarehouseEquipment(){MaintenancePlans=new HashSet<MaintenancePlans>();} [Key] public int EquipmentID{get;set;} public int WarehouseID{get;set;} public int EquipmentTypeID{get;set;} public string InventoryNumber{get;set;} public string EquipmentName{get;set;} public DateTime CommissionedAt{get;set;} public string Status{get;set;} public virtual Warehouses Warehouses{get;set;} public virtual EquipmentTypes EquipmentTypes{get;set;} public virtual ICollection<MaintenancePlans> MaintenancePlans{get;set;} }
    public class MaintenancePlans { [Key] public int PlanID{get;set;} public int EquipmentID{get;set;} public DateTime PlannedDate{get;set;} public string WorkDescription{get;set;} public string Status{get;set;} public virtual WarehouseEquipment WarehouseEquipment{get;set;} }
    public class QualityChecks { [Key] public int QualityCheckID{get;set;} public int ProductID{get;set;} public int EmployeeID{get;set;} public DateTime CheckDate{get;set;} public string Result{get;set;} public string Comment{get;set;} public virtual Products Products{get;set;} public virtual Employees Employees{get;set;} }
    public class ProductionLines { public ProductionLines(){WorkOrders=new HashSet<WorkOrders>();} [Key] public int LineID{get;set;} public int WarehouseID{get;set;} public string LineName{get;set;} public string ShiftCode{get;set;} public bool IsActive{get;set;} public virtual Warehouses Warehouses{get;set;} public virtual ICollection<WorkOrders> WorkOrders{get;set;} }
    public class WorkOrders { [Key] public int WorkOrderID{get;set;} public int LineID{get;set;} public int ProductID{get;set;} public int EmployeeID{get;set;} public string OrderNumber{get;set;} public int PlannedQuantity{get;set;} public int CompletedQuantity{get;set;} public string Status{get;set;} public DateTime CreatedAt{get;set;} public virtual ProductionLines ProductionLines{get;set;} public virtual Products Products{get;set;} public virtual Employees Employees{get;set;} }
}
