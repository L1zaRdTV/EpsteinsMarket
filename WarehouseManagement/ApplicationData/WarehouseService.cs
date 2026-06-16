using System;
using System.Data.Entity;
using System.Linq;

namespace WarehouseManagement.ApplicationData
{
    public static class WarehouseService
    {
        public static bool HasRole(params string[] roles)
        {
            if (AppConnect.CurrentUser == null || AppConnect.CurrentUser.UserRoles == null) return false;
            return roles.Any(r => string.Equals(AppConnect.CurrentUser.UserRoles.RoleName, r, StringComparison.OrdinalIgnoreCase));
        }

        public static void Log(string actionType, string entityName, int? entityId, string details)
        {
            AppConnect.model01.ActionLogs.Add(new ActionLogs
            {
                UserID = AppConnect.CurrentUser == null ? (int?)null : AppConnect.CurrentUser.UserID,
                ActionDate = DateTime.Now,
                ActionType = actionType,
                EntityName = entityName,
                EntityID = entityId,
                Details = details
            });
            AppConnect.model01.SaveChanges();
        }

        public static void AddReceiptItem(Receipts receipt, Products product, StorageLocations location, int quantity, decimal price)
        {
            AppConnect.model01.ReceiptItems.Add(new ReceiptItems { Receipts = receipt, ProductID = product.ProductID, LocationID = location.LocationID, Quantity = quantity, UnitPrice = price });
            var balance = AppConnect.model01.StockBalances.FirstOrDefault(x => x.ProductID == product.ProductID && x.LocationID == location.LocationID);
            if (balance == null)
            {
                balance = new StockBalances { ProductID = product.ProductID, LocationID = location.LocationID, Quantity = 0, UpdatedAt = DateTime.Now };
                AppConnect.model01.StockBalances.Add(balance);
            }
            balance.Quantity += quantity;
            balance.UpdatedAt = DateTime.Now;
        }

        public static bool TryAddShipmentItem(Shipments shipment, Products product, StorageLocations location, int quantity, decimal price, out string error)
        {
            error = null;
            var balance = AppConnect.model01.StockBalances.FirstOrDefault(x => x.ProductID == product.ProductID && x.LocationID == location.LocationID);
            if (balance == null || balance.Quantity < quantity)
            {
                error = "Недостаточно товара на выбранном месте хранения";
                return false;
            }
            AppConnect.model01.ShipmentItems.Add(new ShipmentItems { Shipments = shipment, ProductID = product.ProductID, LocationID = location.LocationID, Quantity = quantity, UnitPrice = price });
            balance.Quantity -= quantity;
            balance.UpdatedAt = DateTime.Now;
            return true;
        }

        public static void CreateInventoryForWarehouse(Warehouses warehouse, Employees employee)
        {
            var inv = new Inventories { InventoryNumber = "INV-" + DateTime.Now.ToString("yyyyMMddHHmmss"), WarehouseID = warehouse.WarehouseID, EmployeeID = employee.EmployeeID, InventoryDate = DateTime.Now, Status = "Создана" };
            AppConnect.model01.Inventories.Add(inv);
            foreach (var b in AppConnect.model01.StockBalances.Include(x => x.StorageLocations.StorageZones).Where(x => x.StorageLocations.StorageZones.WarehouseID == warehouse.WarehouseID).ToList())
            {
                AppConnect.model01.InventoryResults.Add(new InventoryResults { Inventories = inv, BalanceID = b.BalanceID, ExpectedQuantity = b.Quantity, ActualQuantity = b.Quantity, Difference = 0, Comment = "Без расхождений" });
            }
            AppConnect.model01.SaveChanges();
            Log("Создание", "Inventories", inv.InventoryID, inv.InventoryNumber);
        }
    }
}
