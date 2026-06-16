IF DB_ID(N'warehouse_management') IS NOT NULL
BEGIN
    ALTER DATABASE warehouse_management SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE warehouse_management;
END;
GO

CREATE DATABASE warehouse_management;
GO

USE warehouse_management;
GO

CREATE TABLE dbo.UserRoles (
    RoleID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_UserRoles PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL CONSTRAINT UQ_UserRoles_RoleName UNIQUE,
    Description NVARCHAR(255) NULL
);
GO

CREATE TABLE dbo.Employees (
    EmployeeID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Employees PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Position NVARCHAR(80) NOT NULL,
    Phone NVARCHAR(40) NULL,
    Email NVARCHAR(120) NULL,
    HireDate DATE NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Employees_IsActive DEFAULT (1)
);
GO

CREATE TABLE dbo.Users (
    UserID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Login NVARCHAR(80) NOT NULL CONSTRAINT UQ_Users_Login UNIQUE,
    Password NVARCHAR(120) NOT NULL,
    Email NVARCHAR(120) NULL,
    Phone NVARCHAR(40) NULL,
    RoleID INT NOT NULL,
    EmployeeID INT NULL,
    CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSDATETIME()),
    IsBlocked BIT NOT NULL CONSTRAINT DF_Users_IsBlocked DEFAULT (0),
    CONSTRAINT FK_Users_UserRoles FOREIGN KEY (RoleID) REFERENCES dbo.UserRoles(RoleID),
    CONSTRAINT FK_Users_Employees FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID)
);
GO

CREATE TABLE dbo.Categories (
    CategoryID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL CONSTRAINT UQ_Categories_CategoryName UNIQUE,
    Description NVARCHAR(255) NULL
);
GO

CREATE TABLE dbo.Units (
    UnitID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Units PRIMARY KEY,
    UnitName NVARCHAR(80) NOT NULL CONSTRAINT UQ_Units_UnitName UNIQUE,
    ShortName NVARCHAR(20) NOT NULL CONSTRAINT UQ_Units_ShortName UNIQUE
);
GO

CREATE TABLE dbo.Suppliers (
    SupplierID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Suppliers PRIMARY KEY,
    SupplierName NVARCHAR(150) NOT NULL,
    ContactPerson NVARCHAR(120) NULL,
    Phone NVARCHAR(40) NULL,
    Email NVARCHAR(120) NULL,
    Address NVARCHAR(255) NULL
);
GO

CREATE TABLE dbo.Clients (
    ClientID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clients PRIMARY KEY,
    ClientName NVARCHAR(150) NOT NULL,
    ContactPerson NVARCHAR(120) NULL,
    Phone NVARCHAR(40) NULL,
    Email NVARCHAR(120) NULL,
    Address NVARCHAR(255) NULL
);
GO

CREATE TABLE dbo.Warehouses (
    WarehouseID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Warehouses PRIMARY KEY,
    WarehouseName NVARCHAR(120) NOT NULL,
    Address NVARCHAR(255) NOT NULL
);
GO

CREATE TABLE dbo.StorageZones (
    ZoneID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StorageZones PRIMARY KEY,
    WarehouseID INT NOT NULL,
    ZoneName NVARCHAR(80) NOT NULL,
    TemperatureMode NVARCHAR(60) NULL,
    CONSTRAINT FK_StorageZones_Warehouses FOREIGN KEY (WarehouseID) REFERENCES dbo.Warehouses(WarehouseID),
    CONSTRAINT UQ_StorageZones_WarehouseID_ZoneName UNIQUE (WarehouseID, ZoneName)
);
GO

CREATE TABLE dbo.StorageLocations (
    LocationID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StorageLocations PRIMARY KEY,
    ZoneID INT NOT NULL,
    LocationCode NVARCHAR(40) NOT NULL CONSTRAINT UQ_StorageLocations_LocationCode UNIQUE,
    MaxWeight DECIMAL(10,2) NOT NULL,
    CONSTRAINT CK_StorageLocations_MaxWeight CHECK (MaxWeight > 0),
    CONSTRAINT FK_StorageLocations_StorageZones FOREIGN KEY (ZoneID) REFERENCES dbo.StorageZones(ZoneID)
);
GO

CREATE TABLE dbo.Products (
    ProductID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    Sku NVARCHAR(60) NOT NULL CONSTRAINT UQ_Products_Sku UNIQUE,
    ProductName NVARCHAR(180) NOT NULL,
    Description NVARCHAR(500) NULL,
    PurchasePrice DECIMAL(12,2) NOT NULL,
    SalePrice DECIMAL(12,2) NOT NULL,
    CategoryID INT NOT NULL,
    UnitID INT NOT NULL,
    SupplierID INT NOT NULL,
    MinStock INT NOT NULL CONSTRAINT DF_Products_MinStock DEFAULT (0),
    IsActive BIT NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT (1),
    CONSTRAINT CK_Products_PurchasePrice CHECK (PurchasePrice >= 0),
    CONSTRAINT CK_Products_SalePrice CHECK (SalePrice >= 0),
    CONSTRAINT CK_Products_MinStock CHECK (MinStock >= 0),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES dbo.Categories(CategoryID),
    CONSTRAINT FK_Products_Units FOREIGN KEY (UnitID) REFERENCES dbo.Units(UnitID),
    CONSTRAINT FK_Products_Suppliers FOREIGN KEY (SupplierID) REFERENCES dbo.Suppliers(SupplierID)
);
GO

CREATE TABLE dbo.StockBalances (
    BalanceID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_StockBalances PRIMARY KEY,
    ProductID INT NOT NULL,
    LocationID INT NOT NULL,
    Quantity INT NOT NULL CONSTRAINT DF_StockBalances_Quantity DEFAULT (0),
    UpdatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_StockBalances_UpdatedAt DEFAULT (SYSDATETIME()),
    CONSTRAINT CK_StockBalances_Quantity CHECK (Quantity >= 0),
    CONSTRAINT FK_StockBalances_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT FK_StockBalances_StorageLocations FOREIGN KEY (LocationID) REFERENCES dbo.StorageLocations(LocationID),
    CONSTRAINT UQ_StockBalances_ProductID_LocationID UNIQUE (ProductID, LocationID)
);
GO

CREATE TABLE dbo.Receipts (
    ReceiptID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Receipts PRIMARY KEY,
    ReceiptNumber NVARCHAR(40) NOT NULL CONSTRAINT UQ_Receipts_ReceiptNumber UNIQUE,
    SupplierID INT NOT NULL,
    EmployeeID INT NOT NULL,
    ReceiptDate DATETIME2(0) NOT NULL,
    Comment NVARCHAR(255) NULL,
    CONSTRAINT FK_Receipts_Suppliers FOREIGN KEY (SupplierID) REFERENCES dbo.Suppliers(SupplierID),
    CONSTRAINT FK_Receipts_Employees FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID)
);
GO

CREATE TABLE dbo.ReceiptItems (
    ReceiptItemID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ReceiptItems PRIMARY KEY,
    ReceiptID INT NOT NULL,
    ProductID INT NOT NULL,
    LocationID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(12,2) NOT NULL,
    CONSTRAINT CK_ReceiptItems_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_ReceiptItems_UnitPrice CHECK (UnitPrice >= 0),
    CONSTRAINT FK_ReceiptItems_Receipts FOREIGN KEY (ReceiptID) REFERENCES dbo.Receipts(ReceiptID) ON DELETE CASCADE,
    CONSTRAINT FK_ReceiptItems_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT FK_ReceiptItems_StorageLocations FOREIGN KEY (LocationID) REFERENCES dbo.StorageLocations(LocationID)
);
GO

CREATE TABLE dbo.Shipments (
    ShipmentID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Shipments PRIMARY KEY,
    ShipmentNumber NVARCHAR(40) NOT NULL CONSTRAINT UQ_Shipments_ShipmentNumber UNIQUE,
    ClientID INT NOT NULL,
    EmployeeID INT NOT NULL,
    ShipmentDate DATETIME2(0) NOT NULL,
    Comment NVARCHAR(255) NULL,
    CONSTRAINT FK_Shipments_Clients FOREIGN KEY (ClientID) REFERENCES dbo.Clients(ClientID),
    CONSTRAINT FK_Shipments_Employees FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID)
);
GO

CREATE TABLE dbo.ShipmentItems (
    ShipmentItemID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ShipmentItems PRIMARY KEY,
    ShipmentID INT NOT NULL,
    ProductID INT NOT NULL,
    LocationID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(12,2) NOT NULL,
    CONSTRAINT CK_ShipmentItems_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_ShipmentItems_UnitPrice CHECK (UnitPrice >= 0),
    CONSTRAINT FK_ShipmentItems_Shipments FOREIGN KEY (ShipmentID) REFERENCES dbo.Shipments(ShipmentID) ON DELETE CASCADE,
    CONSTRAINT FK_ShipmentItems_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT FK_ShipmentItems_StorageLocations FOREIGN KEY (LocationID) REFERENCES dbo.StorageLocations(LocationID)
);
GO

CREATE TABLE dbo.Inventories (
    InventoryID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Inventories PRIMARY KEY,
    InventoryNumber NVARCHAR(40) NOT NULL CONSTRAINT UQ_Inventories_InventoryNumber UNIQUE,
    WarehouseID INT NOT NULL,
    EmployeeID INT NOT NULL,
    InventoryDate DATETIME2(0) NOT NULL,
    Status NVARCHAR(40) NOT NULL,
    CONSTRAINT FK_Inventories_Warehouses FOREIGN KEY (WarehouseID) REFERENCES dbo.Warehouses(WarehouseID),
    CONSTRAINT FK_Inventories_Employees FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID)
);
GO

CREATE TABLE dbo.InventoryResults (
    ResultID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_InventoryResults PRIMARY KEY,
    InventoryID INT NOT NULL,
    BalanceID INT NOT NULL,
    ExpectedQuantity INT NOT NULL,
    ActualQuantity INT NOT NULL,
    Difference INT NOT NULL,
    Comment NVARCHAR(255) NULL,
    CONSTRAINT CK_InventoryResults_ExpectedQuantity CHECK (ExpectedQuantity >= 0),
    CONSTRAINT CK_InventoryResults_ActualQuantity CHECK (ActualQuantity >= 0),
    CONSTRAINT FK_InventoryResults_Inventories FOREIGN KEY (InventoryID) REFERENCES dbo.Inventories(InventoryID) ON DELETE CASCADE,
    CONSTRAINT FK_InventoryResults_StockBalances FOREIGN KEY (BalanceID) REFERENCES dbo.StockBalances(BalanceID)
);
GO

CREATE TABLE dbo.ActionLogs (
    LogID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ActionLogs PRIMARY KEY,
    UserID INT NULL,
    ActionDate DATETIME2(0) NOT NULL CONSTRAINT DF_ActionLogs_ActionDate DEFAULT (SYSDATETIME()),
    ActionType NVARCHAR(60) NOT NULL,
    EntityName NVARCHAR(80) NULL,
    EntityID INT NULL,
    Details NVARCHAR(500) NULL,
    CONSTRAINT FK_ActionLogs_Users FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID)
);
GO

CREATE TABLE dbo.EquipmentTypes (
    EquipmentTypeID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_EquipmentTypes PRIMARY KEY,
    TypeName NVARCHAR(80) NOT NULL CONSTRAINT UQ_EquipmentTypes_TypeName UNIQUE,
    MaintenanceIntervalDays INT NOT NULL CONSTRAINT DF_EquipmentTypes_MaintenanceIntervalDays DEFAULT (30)
);
GO

CREATE TABLE dbo.WarehouseEquipment (
    EquipmentID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_WarehouseEquipment PRIMARY KEY,
    WarehouseID INT NOT NULL,
    EquipmentTypeID INT NOT NULL,
    InventoryNumber NVARCHAR(60) NOT NULL CONSTRAINT UQ_WarehouseEquipment_InventoryNumber UNIQUE,
    EquipmentName NVARCHAR(120) NOT NULL,
    CommissionedAt DATE NOT NULL,
    Status NVARCHAR(40) NOT NULL,
    CONSTRAINT FK_WarehouseEquipment_Warehouses FOREIGN KEY (WarehouseID) REFERENCES dbo.Warehouses(WarehouseID),
    CONSTRAINT FK_WarehouseEquipment_EquipmentTypes FOREIGN KEY (EquipmentTypeID) REFERENCES dbo.EquipmentTypes(EquipmentTypeID)
);
GO

CREATE TABLE dbo.MaintenancePlans (
    PlanID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_MaintenancePlans PRIMARY KEY,
    EquipmentID INT NOT NULL,
    PlannedDate DATE NOT NULL,
    WorkDescription NVARCHAR(255) NOT NULL,
    Status NVARCHAR(40) NOT NULL,
    CONSTRAINT FK_MaintenancePlans_WarehouseEquipment FOREIGN KEY (EquipmentID) REFERENCES dbo.WarehouseEquipment(EquipmentID)
);
GO

CREATE TABLE dbo.QualityChecks (
    QualityCheckID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_QualityChecks PRIMARY KEY,
    ProductID INT NOT NULL,
    EmployeeID INT NOT NULL,
    CheckDate DATETIME2(0) NOT NULL,
    Result NVARCHAR(40) NOT NULL,
    Comment NVARCHAR(255) NULL,
    CONSTRAINT FK_QualityChecks_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT FK_QualityChecks_Employees FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID)
);
GO

CREATE TABLE dbo.ProductionLines (
    LineID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ProductionLines PRIMARY KEY,
    WarehouseID INT NOT NULL,
    LineName NVARCHAR(120) NOT NULL,
    ShiftCode NVARCHAR(20) NOT NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_ProductionLines_IsActive DEFAULT (1),
    CONSTRAINT FK_ProductionLines_Warehouses FOREIGN KEY (WarehouseID) REFERENCES dbo.Warehouses(WarehouseID)
);
GO

CREATE TABLE dbo.WorkOrders (
    WorkOrderID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_WorkOrders PRIMARY KEY,
    LineID INT NOT NULL,
    ProductID INT NOT NULL,
    EmployeeID INT NOT NULL,
    OrderNumber NVARCHAR(40) NOT NULL CONSTRAINT UQ_WorkOrders_OrderNumber UNIQUE,
    PlannedQuantity INT NOT NULL,
    CompletedQuantity INT NOT NULL CONSTRAINT DF_WorkOrders_CompletedQuantity DEFAULT (0),
    Status NVARCHAR(40) NOT NULL,
    CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_WorkOrders_CreatedAt DEFAULT (SYSDATETIME()),
    CONSTRAINT CK_WorkOrders_PlannedQuantity CHECK (PlannedQuantity >= 0),
    CONSTRAINT CK_WorkOrders_CompletedQuantity CHECK (CompletedQuantity >= 0),
    CONSTRAINT FK_WorkOrders_ProductionLines FOREIGN KEY (LineID) REFERENCES dbo.ProductionLines(LineID),
    CONSTRAINT FK_WorkOrders_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT FK_WorkOrders_Employees FOREIGN KEY (EmployeeID) REFERENCES dbo.Employees(EmployeeID)
);
GO

INSERT INTO dbo.UserRoles (RoleName, Description) VALUES
(N'Администратор', N'Полный доступ'),
(N'Кладовщик', N'Складские операции'),
(N'Менеджер', N'Просмотр и отгрузки клиентам');

INSERT INTO dbo.Employees (FullName, Position, Phone, Email, HireDate) VALUES
(N'Иван Петров', N'Заведующий складом', N'+7 900 111-22-33', N'petrov@warehouse.local', '2023-02-01'),
(N'Анна Смирнова', N'Кладовщик', N'+7 900 222-33-44', N'smirnova@warehouse.local', '2023-05-12'),
(N'Олег Соколов', N'Менеджер', N'+7 900 333-44-55', N'sokolov@warehouse.local', '2024-01-15');

INSERT INTO dbo.Users (FullName, Login, Password, Email, Phone, RoleID, EmployeeID) VALUES
(N'Иван Петров', N'admin', N'admin', N'petrov@warehouse.local', N'+7 900 111-22-33', 1, 1),
(N'Анна Смирнова', N'store', N'store', N'smirnova@warehouse.local', N'+7 900 222-33-44', 2, 2),
(N'Олег Соколов', N'manager', N'manager', N'sokolov@warehouse.local', N'+7 900 333-44-55', 3, 3);

INSERT INTO dbo.Categories (CategoryName, Description) VALUES
(N'Электроинструмент', N'Инструменты с электропитанием'),
(N'Крепёж', N'Метизы и расходники'),
(N'Сантехника', N'Трубы, фитинги, смесители'),
(N'Освещение', N'Лампы и светильники');

INSERT INTO dbo.Units (UnitName, ShortName) VALUES
(N'Штука', N'шт'),
(N'Упаковка', N'уп'),
(N'Метр', N'м'),
(N'Килограмм', N'кг');

INSERT INTO dbo.Suppliers (SupplierName, ContactPerson, Phone, Email, Address) VALUES
(N'ТехноСнаб', N'Мария Волкова', N'+7 495 100-10-10', N'sales@tehnosnab.ru', N'Москва, Складская 1'),
(N'СтройКомплект', N'Дмитрий Орлов', N'+7 812 200-20-20', N'info@stroikom.ru', N'Санкт-Петербург, Индустриальная 9'),
(N'СветМаркет', N'Елена Никитина', N'+7 343 300-30-30', N'opt@svetmarket.ru', N'Екатеринбург, Мира 15');

INSERT INTO dbo.Clients (ClientName, ContactPerson, Phone, Email, Address) VALUES
(N'РемСтрой ООО', N'Павел Егоров', N'+7 499 555-10-10', N'zakupki@remstroy.ru', N'Москва, Ленина 10'),
(N'МастерДом ИП', N'Ирина Алексеева', N'+7 921 555-20-20', N'order@masterdom.ru', N'Псков, Советская 8'),
(N'ГородСервис', N'Николай Морозов', N'+7 831 555-30-30', N'supply@gorodservice.ru', N'Нижний Новгород, Заречная 2');

INSERT INTO dbo.Warehouses (WarehouseName, Address) VALUES
(N'Центральный склад', N'Москва, Промышленная 12'),
(N'Северный склад', N'Химки, Логистическая 4');

INSERT INTO dbo.StorageZones (WarehouseID, ZoneName, TemperatureMode) VALUES
(1, N'A', N'Обычный'),
(1, N'B', N'Обычный'),
(2, N'C', N'Тёплый');

INSERT INTO dbo.StorageLocations (ZoneID, LocationCode, MaxWeight) VALUES
(1, N'A-01-01', 500),
(1, N'A-01-02', 500),
(2, N'B-02-01', 750),
(3, N'C-01-01', 400);

INSERT INTO dbo.Products (Sku, ProductName, Description, PurchasePrice, SalePrice, CategoryID, UnitID, SupplierID, MinStock) VALUES
(N'DRL-500', N'Дрель ударная 500 Вт', N'Бытовая ударная дрель', 2600, 3490, 1, 1, 1, 5),
(N'SCR-18', N'Шуруповёрт 18 В', N'Аккумуляторный шуруповёрт', 4100, 5590, 1, 1, 1, 4),
(N'BOLT-M8', N'Болт М8 оцинкованный', N'Упаковка 100 шт', 180, 260, 2, 2, 2, 20),
(N'PIPE-20', N'Труба ПП 20 мм', N'Полипропиленовая труба', 55, 89, 3, 3, 2, 100),
(N'LED-12', N'Лампа LED 12 Вт', N'Тёплый свет E27', 95, 149, 4, 1, 3, 50),
(N'MIX-01', N'Смеситель кухонный', N'Однорычажный смеситель', 2100, 2990, 3, 1, 2, 6);

INSERT INTO dbo.StockBalances (ProductID, LocationID, Quantity) VALUES
(1, 1, 12),
(2, 1, 8),
(3, 2, 120),
(4, 3, 350),
(5, 4, 210),
(6, 3, 9);

INSERT INTO dbo.Receipts (ReceiptNumber, SupplierID, EmployeeID, ReceiptDate, Comment) VALUES
(N'REC-2026-0001', 1, 2, '2026-06-10T10:00:00', N'Плановая поставка'),
(N'REC-2026-0002', 2, 2, '2026-06-11T11:30:00', N'Метизы и сантехника');

INSERT INTO dbo.ReceiptItems (ReceiptID, ProductID, LocationID, Quantity, UnitPrice) VALUES
(1, 1, 1, 10, 2600),
(1, 2, 1, 5, 4100),
(2, 3, 2, 100, 180),
(2, 4, 3, 300, 55);

INSERT INTO dbo.Shipments (ShipmentNumber, ClientID, EmployeeID, ShipmentDate, Comment) VALUES
(N'SHP-2026-0001', 1, 3, '2026-06-12T15:20:00', N'Отгрузка по заявке'),
(N'SHP-2026-0002', 2, 3, '2026-06-13T09:40:00', N'Самовывоз');

INSERT INTO dbo.ShipmentItems (ShipmentID, ProductID, LocationID, Quantity, UnitPrice) VALUES
(1, 1, 1, 3, 3490),
(1, 5, 4, 20, 149),
(2, 3, 2, 30, 260);

INSERT INTO dbo.Inventories (InventoryNumber, WarehouseID, EmployeeID, InventoryDate, Status) VALUES
(N'INV-2026-0001', 1, 1, '2026-06-14T08:00:00', N'Завершена');

INSERT INTO dbo.InventoryResults (InventoryID, BalanceID, ExpectedQuantity, ActualQuantity, Difference, Comment) VALUES
(1, 1, 12, 12, 0, N'Совпадает'),
(1, 3, 120, 118, -2, N'Недостача 2 уп.');

INSERT INTO dbo.ActionLogs (UserID, ActionType, EntityName, EntityID, Details) VALUES
(1, N'Вход', N'Users', 1, N'Первичный вход администратора'),
(2, N'Поступление', N'Receipts', 1, N'Создано тестовое поступление'),
(3, N'Отгрузка', N'Shipments', 1, N'Создана тестовая отгрузка');

INSERT INTO dbo.EquipmentTypes (TypeName, MaintenanceIntervalDays) VALUES
(N'Погрузчик', 30),
(N'Конвейер', 14),
(N'Весовой терминал', 45),
(N'Холодильная установка', 21);

INSERT INTO dbo.WarehouseEquipment (WarehouseID, EquipmentTypeID, InventoryNumber, EquipmentName, CommissionedAt, Status) VALUES
(1, 1, N'EQ-FORK-001', N'Погрузчик Still RX20', '2024-02-10', N'В работе'),
(1, 2, N'EQ-CONV-001', N'Конвейер приемки A1', '2024-03-18', N'В работе'),
(2, 3, N'EQ-SCALE-002', N'Весовой терминал зона B', '2024-04-05', N'В работе'),
(2, 4, N'EQ-COLD-002', N'Холодильный контур C2', '2024-05-11', N'Плановое ТО');

INSERT INTO dbo.MaintenancePlans (EquipmentID, PlannedDate, WorkDescription, Status) VALUES
(1, CAST(GETDATE() AS DATE), N'Проверка гидравлики и АКБ', N'Запланировано'),
(2, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), N'Диагностика роликов и привода', N'Запланировано'),
(4, DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), N'Проверка температурных датчиков', N'Запланировано');

INSERT INTO dbo.QualityChecks (ProductID, EmployeeID, CheckDate, Result, Comment) VALUES
(1, 1, SYSDATETIME(), N'Пройдено', N'Маркировка и упаковка соответствуют норме'),
(2, 2, SYSDATETIME(), N'Пройдено', N'Габариты партии подтверждены'),
(3, 3, SYSDATETIME(), N'На контроле', N'Требуется повторная проверка паллеты');

INSERT INTO dbo.ProductionLines (WarehouseID, LineName, ShiftCode, IsActive) VALUES
(1, N'Линия комплектации A', N'День', 1),
(1, N'Линия упаковки B', N'Ночь', 1),
(2, N'Линия маркировки C', N'День', 1);

INSERT INTO dbo.WorkOrders (LineID, ProductID, EmployeeID, OrderNumber, PlannedQuantity, CompletedQuantity, Status) VALUES
(1, 1, 1, N'WO-2026-0001', 120, 75, N'В работе'),
(2, 2, 2, N'WO-2026-0002', 80, 80, N'Завершен'),
(3, 3, 3, N'WO-2026-0003', 60, 15, N'В работе');
GO
