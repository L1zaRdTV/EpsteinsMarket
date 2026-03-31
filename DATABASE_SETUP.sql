/* EpsteinsMarket: скрипт для SQL Server Management Studio */
IF DB_ID(N'EpsteinsMarket') IS NULL
BEGIN
    CREATE DATABASE EpsteinsMarket;
END
GO

USE EpsteinsMarket;
GO

IF OBJECT_ID(N'dbo.Favorites', N'U') IS NOT NULL DROP TABLE dbo.Favorites;
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID(N'dbo.Categories', N'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    BirthDate DATE NULL,
    Experience INT NULL,
    Login NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    [Role] NVARCHAR(20) NULL
);
GO

CREATE TABLE dbo.Categories
(
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(150) NOT NULL
);
GO

CREATE TABLE dbo.Products
(
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(250) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    Price DECIMAL(18,2) NULL,
    [Image] NVARCHAR(500) NULL,
    CategoryID INT NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES dbo.Categories(CategoryID)
);
GO

CREATE TABLE dbo.Favorites
(
    FavoriteID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    ProductID INT NOT NULL,
    CONSTRAINT FK_Favorites_Users FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID),
    CONSTRAINT FK_Favorites_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT UQ_Favorites_UserProduct UNIQUE(UserID, ProductID)
);
GO

INSERT INTO dbo.Users (FullName, BirthDate, Experience, Login, [Password], Email, Phone, [Role])
VALUES
(N'Администратор системы', '1990-05-10', 10, N'admin', N'admin1234', N'admin@market.local', N'+79990000000', N'Администратор'),
(N'Иван Петров', '1997-03-22', 3, N'ivan', N'ivan1234', N'ivan@mail.ru', N'+79991112233', N'Пользователь'),
(N'Мария Смирнова', '1995-08-14', 5, N'maria', N'maria1234', N'maria@mail.ru', N'+79994445566', N'Пользователь');
GO

INSERT INTO dbo.Categories (CategoryName)
VALUES
(N'Фрукты'),
(N'Овощи'),
(N'Напитки'),
(N'Сладости');
GO

INSERT INTO dbo.Products ([Name], [Description], Price, [Image], CategoryID)
VALUES
(N'Яблоки Гала', N'Свежие сладкие яблоки.', 129.00, NULL, 1),
(N'Бананы', N'Спелые бананы, 1 кг.', 99.00, NULL, 1),
(N'Томаты', N'Красные томаты для салатов.', 149.00, NULL, 2),
(N'Апельсиновый сок', N'Сок прямого отжима, 1 л.', 189.00, NULL, 3),
(N'Шоколад молочный', N'Плитка 90 г.', 79.00, NULL, 4);
GO

INSERT INTO dbo.Favorites (UserID, ProductID)
VALUES (2, 1), (2, 4), (3, 5);
GO
