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
    (N'Праздничные гномики'),
    (N'Гномики для дома'),
    (N'Мини-гномики'),
    (N'Большие фигуры');

INSERT INTO dbo.Brands (BrandName)
VALUES
    (N'NordGnome'),
    (N'GnomeCraft'),
    (N'DwarfArt'),
    (N'ForestSmile'),
    (N'ClayMaster'),
    (N'GnomeGalaxy');

INSERT INTO dbo.ProductStatuses (StatusName)
VALUES (N'В наличии'), (N'Под заказ'), (N'Закончился');

INSERT INTO dbo.OrderStatuses (StatusName)
VALUES (N'Новый'), (N'Оплачен'), (N'Отправлен'), (N'Завершен'), (N'Отменен');

INSERT INTO dbo.Users (FullName, Login, Password, Email, Phone, RoleID, CreatedAt, IsBlocked)
VALUES
    (N'Админ Эпштейн', N'admin', N'admin123', N'admin@epsteins.market', N'+70000000001', 1, DATEADD(DAY, -180, GETDATE()), 0),
    (N'Менеджер Каталога', N'manager', N'manager123', N'manager@epsteins.market', N'+70000000002', 1, DATEADD(DAY, -120, GETDATE()), 0),
    (N'Покупатель Гномов', N'user', N'user123', N'user@epsteins.market', N'+70000000003', 2, DATEADD(DAY, -90, GETDATE()), 0);

DECLARE @i INT = 1;
WHILE @i <= 27
BEGIN
    INSERT INTO dbo.Users (FullName, Login, Password, Email, Phone, RoleID, CreatedAt, IsBlocked)
    VALUES (
        N'Клиент №' + CAST(@i AS NVARCHAR(10)),
        N'client' + RIGHT('00' + CAST(@i AS NVARCHAR(10)), 2),
        N'pass' + CAST(@i AS NVARCHAR(10)),
        N'client' + CAST(@i AS NVARCHAR(10)) + N'@epsteins.market',
        N'+7999000' + RIGHT('000' + CAST(@i AS NVARCHAR(10)), 3),
        2,
        DATEADD(DAY, -@i, GETDATE()),
        CASE WHEN @i IN (8, 19) THEN 1 ELSE 0 END
    );

    SET @i = @i + 1;
END

INSERT INTO dbo.Carts (UserID, CreatedAt)
SELECT UserID, DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 50, GETDATE())
FROM dbo.Users;

SET @i = 1;
WHILE @i <= 80
BEGIN
    INSERT INTO dbo.Products
    (
        ProductName,
        Description,
        Price,
        QuantityInStock,
        CategoryID,
        StatusID,
        BrandID,
        MainImage
    )
    VALUES
    (
        N'Гномик модель #' + CAST(@i AS NVARCHAR(10)),
        N'Декоративный гномик серии #' + CAST(@i AS NVARCHAR(10)) + N'. Материал: полистоун. Подходит для сада и интерьера.',
        CAST(650 + (@i * 47) AS DECIMAL(10,2)),
        5 + (@i * 3) % 80,
        ((@i - 1) % 6) + 1,
        CASE WHEN @i % 13 = 0 THEN 3 WHEN @i % 5 = 0 THEN 2 ELSE 1 END,
        ((@i - 1) % 6) + 1,
        N'gnome-' + RIGHT('000' + CAST(@i AS NVARCHAR(10)), 3) + N'.jpg'
    );

    SET @i = @i + 1;
END

INSERT INTO dbo.ProductImages (ProductID, ImagePath)
SELECT ProductID, N'gnome-' + RIGHT('000' + CAST(ProductID AS NVARCHAR(10)), 3) + N'-side.jpg'
FROM dbo.Products;

INSERT INTO dbo.ProductImages (ProductID, ImagePath)
SELECT ProductID, N'gnome-' + RIGHT('000' + CAST(ProductID AS NVARCHAR(10)), 3) + N'-box.jpg'
FROM dbo.Products
WHERE ProductID % 2 = 0;

INSERT INTO dbo.TelegramLinks (Url, QrImagePath, IsActive)
VALUES
    (N'https://t.me/epsteins_market', N'telegram-qr-main.png', 1),
    (N'https://t.me/epsteins_market_backup', N'telegram-qr-backup.png', 0);

INSERT INTO dbo.CartItems (CartID, ProductID, Quantity, PriceAtMoment)
SELECT TOP (120)
    c.CartID,
    p.ProductID,
    1 + ABS(CHECKSUM(NEWID())) % 4,
    p.Price
FROM dbo.Carts c
CROSS JOIN dbo.Products p
WHERE c.UserID > 2
ORDER BY NEWID();

SET @i = 1;
WHILE @i <= 45
BEGIN
    DECLARE @OrderUserID INT = 3 + (@i % 27);
    DECLARE @OrderStatusID INT = CASE WHEN @i % 10 = 0 THEN 5 WHEN @i % 4 = 0 THEN 4 WHEN @i % 3 = 0 THEN 3 ELSE 2 END;

    INSERT INTO dbo.Orders (UserID, OrderDate, TotalAmount, OrderStatusID)
    VALUES (@OrderUserID, DATEADD(DAY, -@i, GETDATE()), 0, @OrderStatusID);

    DECLARE @NewOrderID INT = SCOPE_IDENTITY();

    INSERT INTO dbo.OrderItems (OrderID, ProductID, Quantity, PriceAtMoment)
    SELECT TOP (3)
        @NewOrderID,
        p.ProductID,
        1 + ABS(CHECKSUM(NEWID())) % 3,
        p.Price
    FROM dbo.Products p
    ORDER BY NEWID();

    UPDATE o
    SET o.TotalAmount = (
        SELECT SUM(oi.Quantity * oi.PriceAtMoment)
        FROM dbo.OrderItems oi
        WHERE oi.OrderID = o.OrderID
    )
    FROM dbo.Orders o
    WHERE o.OrderID = @NewOrderID;

    IF @OrderStatusID IN (2,3,4)
    BEGIN
        INSERT INTO dbo.Receipts (OrderID, PdfPath, CreatedAt)
        VALUES (@NewOrderID, N'Receipts\\receipt_' + CAST(@NewOrderID AS NVARCHAR(20)) + N'.pdf', DATEADD(DAY, -@i, GETDATE()));
    END

    SET @i = @i + 1;
END
GO

SELECT
    (SELECT COUNT(*) FROM dbo.Users) AS UsersCount,
    (SELECT COUNT(*) FROM dbo.Products) AS ProductsCount,
    (SELECT COUNT(*) FROM dbo.CartItems) AS CartItemsCount,
    (SELECT COUNT(*) FROM dbo.Orders) AS OrdersCount,
    (SELECT COUNT(*) FROM dbo.OrderItems) AS OrderItemsCount,
    (SELECT COUNT(*) FROM dbo.Receipts) AS ReceiptsCount;
GO
