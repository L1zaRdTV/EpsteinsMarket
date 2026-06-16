using System;

namespace WarehouseManagement.ApplicationData
{
    internal static class OfflineDataSeeder
    {
        public static void Seed(OfflineWarehouseDbContext db)
        {
            var adminRole = new UserRoles { RoleID = 1, RoleName = "Администратор", Description = "Полный доступ" };
            var storeRole = new UserRoles { RoleID = 2, RoleName = "Кладовщик", Description = "Складские операции" };
            var managerRole = new UserRoles { RoleID = 3, RoleName = "Менеджер", Description = "Просмотр и отгрузки клиентам" };
            db.UserRoles.Add(adminRole); db.UserRoles.Add(storeRole); db.UserRoles.Add(managerRole);

            var employee = new Employees { EmployeeID = 1, FullName = "Иван Петров", Position = "Заведующий складом", Phone = "+7 900 111-22-33", Email = "petrov@warehouse.local", HireDate = new DateTime(2023, 2, 1), IsActive = true };
            db.Employees.Add(employee);
            var admin = new Users { UserID = 1, FullName = employee.FullName, Login = "admin", Password = "admin", Email = employee.Email, Phone = employee.Phone, RoleID = adminRole.RoleID, EmployeeID = employee.EmployeeID, CreatedAt = DateTime.Now, IsBlocked = false, UserRoles = adminRole, Employees = employee };
            adminRole.Users.Add(admin); employee.Users.Add(admin); db.Users.Add(admin);

            var category = new Categories { CategoryID = 1, CategoryName = "Электроинструмент", Description = "Инструменты с электропитанием" };
            var unit = new Units { UnitID = 1, UnitName = "Штука", ShortName = "шт" };
            var supplier = new Suppliers { SupplierID = 1, SupplierName = "ТехноСнаб", ContactPerson = "Мария Волкова", Phone = "+7 495 100-10-10", Email = "sales@tehnosnab.ru", Address = "Москва, Складская 1" };
            var client = new Clients { ClientID = 1, ClientName = "РемСтрой ООО", ContactPerson = "Павел Егоров", Phone = "+7 499 555-10-10", Email = "zakupki@remstroy.ru", Address = "Москва, Ленина 10" };
            var warehouse = new Warehouses { WarehouseID = 1, WarehouseName = "Центральный склад", Address = "Москва, Промышленная 12" };
            var zone = new StorageZones { ZoneID = 1, WarehouseID = warehouse.WarehouseID, ZoneName = "A", TemperatureMode = "Обычный", Warehouses = warehouse };
            var location = new StorageLocations { LocationID = 1, ZoneID = zone.ZoneID, LocationCode = "A-01-01", MaxWeight = 500, StorageZones = zone };
            db.Categories.Add(category); db.Units.Add(unit); db.Suppliers.Add(supplier); db.Clients.Add(client); db.Warehouses.Add(warehouse); db.StorageZones.Add(zone); db.StorageLocations.Add(location);
            warehouse.StorageZones.Add(zone); zone.StorageLocations.Add(location);

            var drill = new Products { ProductID = 1, Sku = "DRL-500", ProductName = "Дрель ударная 500 Вт", Description = "Бытовая ударная дрель", PurchasePrice = 2600, SalePrice = 3490, CategoryID = category.CategoryID, UnitID = unit.UnitID, SupplierID = supplier.SupplierID, MinStock = 5, IsActive = true, Categories = category, Units = unit, Suppliers = supplier };
            db.Products.Add(drill); category.Products.Add(drill); unit.Products.Add(drill); supplier.Products.Add(drill);
            var balance = new StockBalances { BalanceID = 1, ProductID = drill.ProductID, LocationID = location.LocationID, Quantity = 12, UpdatedAt = DateTime.Now, Products = drill, StorageLocations = location };
            db.StockBalances.Add(balance); drill.StockBalances.Add(balance); location.StockBalances.Add(balance);

            var shipment = new Shipments { ShipmentID = 1, ShipmentNumber = "SHP-OFFLINE-0001", ClientID = client.ClientID, EmployeeID = employee.EmployeeID, ShipmentDate = DateTime.Now, Comment = "Демонстрационная отгрузка", Clients = client, Employees = employee };
            var shipmentItem = new ShipmentItems { ShipmentItemID = 1, ShipmentID = shipment.ShipmentID, ProductID = drill.ProductID, LocationID = location.LocationID, Quantity = 1, UnitPrice = drill.SalePrice, Shipments = shipment, Products = drill, StorageLocations = location };
            db.Shipments.Add(shipment); db.ShipmentItems.Add(shipmentItem); shipment.ShipmentItems.Add(shipmentItem); client.Shipments.Add(shipment);
        }
    }
}
