USE [master];
GO

IF DB_ID(N'EpsteinsMarketDB') IS NULL
BEGIN
    CREATE DATABASE [EpsteinsMarketDB];
END
GO

USE [EpsteinsMarketDB];
GO

IF OBJECT_ID(N'dbo.Receipts', N'U') IS NOT NULL DROP TABLE dbo.Receipts;
IF OBJECT_ID(N'dbo.OrderItems', N'U') IS NOT NULL DROP TABLE dbo.OrderItems;
IF OBJECT_ID(N'dbo.Orders', N'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID(N'dbo.CartItems', N'U') IS NOT NULL DROP TABLE dbo.CartItems;
IF OBJECT_ID(N'dbo.Carts', N'U') IS NOT NULL DROP TABLE dbo.Carts;
IF OBJECT_ID(N'dbo.ProductImages', N'U') IS NOT NULL DROP TABLE dbo.ProductImages;
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID(N'dbo.TelegramLinks', N'U') IS NOT NULL DROP TABLE dbo.TelegramLinks;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID(N'dbo.OrderStatuses', N'U') IS NOT NULL DROP TABLE dbo.OrderStatuses;
IF OBJECT_ID(N'dbo.ProductStatuses', N'U') IS NOT NULL DROP TABLE dbo.ProductStatuses;
IF OBJECT_ID(N'dbo.Brands', N'U') IS NOT NULL DROP TABLE dbo.Brands;
IF OBJECT_ID(N'dbo.Categories', N'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID(N'dbo.UserRoles', N'U') IS NOT NULL DROP TABLE dbo.UserRoles;
GO

CREATE TABLE dbo.UserRoles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE dbo.Brands (
    BrandID INT IDENTITY(1,1) PRIMARY KEY,
    BrandName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.ProductStatuses (
    StatusID INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.OrderStatuses (
    OrderStatusID INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Login NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Phone NVARCHAR(20) NOT NULL UNIQUE,
    RoleID INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    IsBlocked BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_Users_UserRoles FOREIGN KEY (RoleID) REFERENCES dbo.UserRoles(RoleID)
);

CREATE TABLE dbo.Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    QuantityInStock INT NOT NULL,
    CategoryID INT NOT NULL,
    StatusID INT NOT NULL,
    BrandID INT NOT NULL,
    MainImage NVARCHAR(255) NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES dbo.Categories(CategoryID),
    CONSTRAINT FK_Products_ProductStatuses FOREIGN KEY (StatusID) REFERENCES dbo.ProductStatuses(StatusID),
    CONSTRAINT FK_Products_Brands FOREIGN KEY (BrandID) REFERENCES dbo.Brands(BrandID)
);

CREATE TABLE dbo.ProductImages (
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    ImagePath NVARCHAR(255) NOT NULL,
    CONSTRAINT FK_ProductImages_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID) ON DELETE CASCADE
);

CREATE TABLE dbo.Carts (
    CartID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL UNIQUE,
    CreatedAt DATETIME NOT NULL,
    CONSTRAINT FK_Carts_Users FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID) ON DELETE CASCADE
);

CREATE TABLE dbo.CartItems (
    CartItemID INT IDENTITY(1,1) PRIMARY KEY,
    CartID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    PriceAtMoment DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_CartItems_Carts FOREIGN KEY (CartID) REFERENCES dbo.Carts(CartID) ON DELETE CASCADE,
    CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID)
);

CREATE TABLE dbo.Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderStatusID INT NOT NULL,
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID),
    CONSTRAINT FK_Orders_OrderStatuses FOREIGN KEY (OrderStatusID) REFERENCES dbo.OrderStatuses(OrderStatusID)
);

CREATE TABLE dbo.OrderItems (
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    PriceAtMoment DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderID) REFERENCES dbo.Orders(OrderID) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID)
);

CREATE TABLE dbo.Receipts (
    ReceiptID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    PdfPath NVARCHAR(500) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    CONSTRAINT FK_Receipts_Orders FOREIGN KEY (OrderID) REFERENCES dbo.Orders(OrderID) ON DELETE CASCADE
);

CREATE TABLE dbo.TelegramLinks (
    TelegramLinkID INT IDENTITY(1,1) PRIMARY KEY,
    Url NVARCHAR(255) NOT NULL,
    QrImagePath NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT(0)
);
GO

INSERT INTO dbo.UserRoles (RoleName)
VALUES (N'Администратор'), (N'Пользователь');

INSERT INTO dbo.Categories (CategoryName)
VALUES
    (N'Садовые гномики'),
    (N'Коллекционные гномики'),
    (N'Праздничные гномики');

INSERT INTO dbo.Brands (BrandName)
VALUES (N'NordGnome'), (N'GnomeCraft'), (N'DwarfArt');

INSERT INTO dbo.ProductStatuses (StatusName)
VALUES (N'В наличии'), (N'Под заказ'), (N'Закончился');

INSERT INTO dbo.OrderStatuses (StatusName)
VALUES (N'Новый'), (N'Оплачен'), (N'Отправлен'), (N'Завершен');

INSERT INTO dbo.Users (FullName, Login, Password, Email, Phone, RoleID, CreatedAt, IsBlocked)
VALUES
    (N'Админ Эпштейн', N'admin', N'admin123', N'admin@epsteins.market', N'+70000000001', 1, GETDATE(), 0),
    (N'Покупатель Гномов', N'user', N'user123', N'user@epsteins.market', N'+70000000002', 2, GETDATE(), 0);

INSERT INTO dbo.Carts (UserID, CreatedAt)
SELECT UserID, GETDATE() FROM dbo.Users;

INSERT INTO dbo.Products (ProductName, Description, Price, QuantityInStock, CategoryID, StatusID, BrandID, MainImage)
VALUES
    (N'Гном-хранитель сада', N'Классический маленький гномик для дачи и сада.', 1490.00, 25, 1, 1, 1, N'gnome-garden-guardian.jpg'),
    (N'Гном-рыбак', N'Ручная роспись, коллекционная серия.', 2390.00, 10, 2, 1, 2, N'gnome-fisherman.jpg'),
    (N'Новогодний гномик', N'Праздничный гном в красном колпаке.', 1790.00, 18, 3, 1, 3, N'gnome-holiday-red.jpg'),
    (N'Мини-гном лесной', N'Компактный размер, подходит для домашнего декора.', 990.00, 40, 1, 1, 2, N'gnome-forest-mini.jpg');

INSERT INTO dbo.ProductImages (ProductID, ImagePath)
VALUES
    (1, N'gnome-garden-guardian-side.jpg'),
    (2, N'gnome-fisherman-back.jpg'),
    (3, N'gnome-holiday-red-side.jpg'),
    (4, N'gnome-forest-mini-box.jpg');

INSERT INTO dbo.TelegramLinks (Url, QrImagePath, IsActive)
VALUES (N'https://t.me/epsteins_market', NULL, 1);
GO
