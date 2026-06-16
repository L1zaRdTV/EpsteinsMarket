using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace WarehouseManagement.ApplicationData
{
    public class OfflineWarehouseDbContext : WarehouseDBEntitiesContext
    {
        public bool IsOffline { get { return true; } }

        public OfflineWarehouseDbContext()
        {
            UserRoles = new OfflineDbSet<UserRoles>();
            Employees = new OfflineDbSet<Employees>();
            Users = new OfflineDbSet<Users>();
            Categories = new OfflineDbSet<Categories>();
            Units = new OfflineDbSet<Units>();
            Suppliers = new OfflineDbSet<Suppliers>();
            Clients = new OfflineDbSet<Clients>();
            Warehouses = new OfflineDbSet<Warehouses>();
            StorageZones = new OfflineDbSet<StorageZones>();
            StorageLocations = new OfflineDbSet<StorageLocations>();
            Products = new OfflineDbSet<Products>();
            StockBalances = new OfflineDbSet<StockBalances>();
            Receipts = new OfflineDbSet<Receipts>();
            ReceiptItems = new OfflineDbSet<ReceiptItems>();
            Shipments = new OfflineDbSet<Shipments>();
            ShipmentItems = new OfflineDbSet<ShipmentItems>();
            Inventories = new OfflineDbSet<Inventories>();
            InventoryResults = new OfflineDbSet<InventoryResults>();
            ActionLogs = new OfflineDbSet<ActionLogs>();

            OfflineDataSeeder.Seed(this);
        }

        public override DbSet<UserRoles> UserRoles { get; set; }
        public override DbSet<Employees> Employees { get; set; }
        public override DbSet<Users> Users { get; set; }
        public override DbSet<Categories> Categories { get; set; }
        public override DbSet<Units> Units { get; set; }
        public override DbSet<Suppliers> Suppliers { get; set; }
        public override DbSet<Clients> Clients { get; set; }
        public override DbSet<Warehouses> Warehouses { get; set; }
        public override DbSet<StorageZones> StorageZones { get; set; }
        public override DbSet<StorageLocations> StorageLocations { get; set; }
        public override DbSet<Products> Products { get; set; }
        public override DbSet<StockBalances> StockBalances { get; set; }
        public override DbSet<Receipts> Receipts { get; set; }
        public override DbSet<ReceiptItems> ReceiptItems { get; set; }
        public override DbSet<Shipments> Shipments { get; set; }
        public override DbSet<ShipmentItems> ShipmentItems { get; set; }
        public override DbSet<Inventories> Inventories { get; set; }
        public override DbSet<InventoryResults> InventoryResults { get; set; }
        public override DbSet<ActionLogs> ActionLogs { get; set; }

        public override int SaveChanges()
        {
            AssignIds(UserRoles, x => x.RoleID, (x, v) => x.RoleID = v);
            AssignIds(Employees, x => x.EmployeeID, (x, v) => x.EmployeeID = v);
            AssignIds(Users, x => x.UserID, (x, v) => x.UserID = v);
            AssignIds(Categories, x => x.CategoryID, (x, v) => x.CategoryID = v);
            AssignIds(Units, x => x.UnitID, (x, v) => x.UnitID = v);
            AssignIds(Suppliers, x => x.SupplierID, (x, v) => x.SupplierID = v);
            AssignIds(Clients, x => x.ClientID, (x, v) => x.ClientID = v);
            AssignIds(Warehouses, x => x.WarehouseID, (x, v) => x.WarehouseID = v);
            AssignIds(StorageZones, x => x.ZoneID, (x, v) => x.ZoneID = v);
            AssignIds(StorageLocations, x => x.LocationID, (x, v) => x.LocationID = v);
            AssignIds(Products, x => x.ProductID, (x, v) => x.ProductID = v);
            AssignIds(StockBalances, x => x.BalanceID, (x, v) => x.BalanceID = v);
            AssignIds(Receipts, x => x.ReceiptID, (x, v) => x.ReceiptID = v);
            AssignIds(ReceiptItems, x => x.ReceiptItemID, (x, v) => x.ReceiptItemID = v);
            AssignIds(Shipments, x => x.ShipmentID, (x, v) => x.ShipmentID = v);
            AssignIds(ShipmentItems, x => x.ShipmentItemID, (x, v) => x.ShipmentItemID = v);
            AssignIds(Inventories, x => x.InventoryID, (x, v) => x.InventoryID = v);
            AssignIds(InventoryResults, x => x.ResultID, (x, v) => x.ResultID = v);
            AssignIds(ActionLogs, x => x.LogID, (x, v) => x.LogID = v);
            return 1;
        }

        private static void AssignIds<T>(IEnumerable<T> items, Func<T, int> getter, Action<T, int> setter)
        {
            var next = items.Any() ? items.Max(getter) + 1 : 1;
            foreach (var item in items.Where(x => getter(x) == 0)) setter(item, next++);
        }
    }

    public class OfflineDbSet<T> : DbSet<T>, IQueryable<T>, IEnumerable<T> where T : class
    {
        private readonly ObservableCollection<T> data = new ObservableCollection<T>();
        private readonly IQueryable<T> query;

        public OfflineDbSet() { query = data.AsQueryable(); }
        public override T Add(T item) { data.Add(item); return item; }
        public override T Remove(T item) { data.Remove(item); return item; }
        public override ObservableCollection<T> Local { get { return data; } }
        public override T Attach(T item) { if (!data.Contains(item)) data.Add(item); return item; }
        public override T Create() { return Activator.CreateInstance<T>(); }
        public IEnumerator<T> GetEnumerator() { return data.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return data.GetEnumerator(); }
        public Type ElementType { get { return query.ElementType; } }
        public Expression Expression { get { return query.Expression; } }
        public IQueryProvider Provider { get { return query.Provider; } }
    }
}
