USE [master]
GO
/****** Object:  Database [CloneShopDB]    Script Date: 10.04.2026 13:51:49 ******/
CREATE DATABASE [CloneShopDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CloneShopDB', FILENAME = N'C:\Users\10230037\CloneShopDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CloneShopDB_log', FILENAME = N'C:\Users\10230037\CloneShopDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CloneShopDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CloneShopDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CloneShopDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CloneShopDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CloneShopDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CloneShopDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CloneShopDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CloneShopDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CloneShopDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CloneShopDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CloneShopDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CloneShopDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CloneShopDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CloneShopDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CloneShopDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CloneShopDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CloneShopDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CloneShopDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CloneShopDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CloneShopDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CloneShopDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CloneShopDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CloneShopDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CloneShopDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CloneShopDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CloneShopDB] SET  MULTI_USER 
GO
ALTER DATABASE [CloneShopDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CloneShopDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CloneShopDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CloneShopDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CloneShopDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CloneShopDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CloneShopDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [CloneShopDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CloneShopDB]
GO
/****** Object:  Table [dbo].[Brands]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brands](
	[BrandID] [int] IDENTITY(1,1) NOT NULL,
	[BrandName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Brands] PRIMARY KEY CLUSTERED 
(
	[BrandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItems]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItems](
	[CartItemID] [int] IDENTITY(1,1) NOT NULL,
	[CartID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[PriceAtMoment] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED 
(
	[CartItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carts]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carts](
	[CartID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED 
(
	[CartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItemID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[PriceAtMoment] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[OrderItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[OrderStatusID] [int] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatuses]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatuses](
	[OrderStatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OrderStatuses] PRIMARY KEY CLUSTERED 
(
	[OrderStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImages]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImages](
	[ImageID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[ImagePath] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ProductImages] PRIMARY KEY CLUSTERED 
(
	[ImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](150) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[QuantityInStock] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[MainImage] [nvarchar](255) NOT NULL,
	[BrandID] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductStatuses]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductStatuses](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProductStatuses] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Receipts]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receipts](
	[ReceiptID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[PdfPath] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Receipts] PRIMARY KEY CLUSTERED 
(
	[ReceiptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TelegramLinks]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TelegramLinks](
	[TelegramLinkID] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](255) NOT NULL,
	[QrImagePath] [nvarchar](255) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TelegramLinks] PRIMARY KEY CLUSTERED 
(
	[TelegramLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10.04.2026 13:51:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](70) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](30) NULL,
	[RoleID] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[IsBlocked] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Brands] ON 
GO
INSERT [dbo].[Brands] ([BrandID], [BrandName]) VALUES (1, N'Valakas Industries')
GO
INSERT [dbo].[Brands] ([BrandID], [BrandName]) VALUES (2, N'George Floyd Corp')
GO
INSERT [dbo].[Brands] ([BrandID], [BrandName]) VALUES (3, N'Burmalda Factory')
GO
INSERT [dbo].[Brands] ([BrandID], [BrandName]) VALUES (4, N'BioanalReplica Systems')
GO
INSERT [dbo].[Brands] ([BrandID], [BrandName]) VALUES (5, N'Autistic Kids')
GO
SET IDENTITY_INSERT [dbo].[Brands] OFF
GO
SET IDENTITY_INSERT [dbo].[Carts] ON 
GO
INSERT [dbo].[Carts] ([CartID], [UserID], [CreatedAt]) VALUES (1, 10, CAST(N'2026-04-09T13:42:06.893' AS DateTime))
GO
INSERT [dbo].[Carts] ([CartID], [UserID], [CreatedAt]) VALUES (2, 12, CAST(N'2026-04-09T13:42:52.030' AS DateTime))
GO
INSERT [dbo].[Carts] ([CartID], [UserID], [CreatedAt]) VALUES (3, 13, CAST(N'2026-04-09T13:54:22.833' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Carts] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (1, N'Боевой клон')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (2, N'Премиум-клон')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (3, N'Кибер клон')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (4, N'Нига клон')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (5, N'Рофло клон')
GO
INSERT [dbo].[Categories] ([CategoryID], [CategoryName]) VALUES (6, N'Водяные клоны')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderItems] ON 
GO
INSERT [dbo].[OrderItems] ([OrderItemID], [OrderID], [ProductID], [Quantity], [PriceAtMoment]) VALUES (1, 1, 1, 1, CAST(14999.90 AS Decimal(10, 2)))
GO
INSERT [dbo].[OrderItems] ([OrderItemID], [OrderID], [ProductID], [Quantity], [PriceAtMoment]) VALUES (2, 2, 1, 1, CAST(14999.90 AS Decimal(10, 2)))
GO
INSERT [dbo].[OrderItems] ([OrderItemID], [OrderID], [ProductID], [Quantity], [PriceAtMoment]) VALUES (3, 3, 3, 1, CAST(45999.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[OrderItems] ([OrderItemID], [OrderID], [ProductID], [Quantity], [PriceAtMoment]) VALUES (1002, 1002, 1, 1, CAST(14999.90 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[OrderItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [TotalAmount], [OrderStatusID]) VALUES (1, 13, CAST(N'2026-04-09T13:55:08.490' AS DateTime), CAST(14999.90 AS Decimal(10, 2)), 1)
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [TotalAmount], [OrderStatusID]) VALUES (2, 13, CAST(N'2026-04-09T14:55:49.410' AS DateTime), CAST(14999.90 AS Decimal(10, 2)), 1)
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [TotalAmount], [OrderStatusID]) VALUES (3, 13, CAST(N'2026-04-09T15:21:52.477' AS DateTime), CAST(45999.00 AS Decimal(10, 2)), 1)
GO
INSERT [dbo].[Orders] ([OrderID], [UserID], [OrderDate], [TotalAmount], [OrderStatusID]) VALUES (1002, 13, CAST(N'2026-04-10T12:25:38.497' AS DateTime), CAST(14999.90 AS Decimal(10, 2)), 1)
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderStatuses] ON 
GO
INSERT [dbo].[OrderStatuses] ([OrderStatusID], [StatusName]) VALUES (1, N'Создан')
GO
INSERT [dbo].[OrderStatuses] ([OrderStatusID], [StatusName]) VALUES (2, N'Оплачен')
GO
INSERT [dbo].[OrderStatuses] ([OrderStatusID], [StatusName]) VALUES (3, N'Отменен')
GO
INSERT [dbo].[OrderStatuses] ([OrderStatusID], [StatusName]) VALUES (4, N'Выдан')
GO
SET IDENTITY_INSERT [dbo].[OrderStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductImages] ON 
GO
INSERT [dbo].[ProductImages] ([ImageID], [ProductID], [ImagePath]) VALUES (3, 4, N'462c77ff76b14de3833341e415504027.jpg')
GO
SET IDENTITY_INSERT [dbo].[ProductImages] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [StatusID], [MainImage], [BrandID]) VALUES (1, N'Кибер Глад Валакассс', N'Кибернетическая модель патау Глада Валакасааааааа', CAST(14999.90 AS Decimal(10, 2)), 5, 3, 1, N'cyborg-glad.png', 1)
GO
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [StatusID], [MainImage], [BrandID]) VALUES (3, N'Нига Глад Валакас', N'Черномазая точная копия патау Глада Валакаса', CAST(45999.00 AS Decimal(10, 2)), 2, 4, 1, N'nigaglad.jpg', 1)
GO
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [StatusID], [MainImage], [BrandID]) VALUES (4, N'Кибер Джордж Флойд', N'Легенда..... Но теперь кибернетическая и без зависимостей', CAST(4999.00 AS Decimal(10, 2)), 6, 3, 1, N'george.jpg', 2)
GO
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [QuantityInStock], [CategoryID], [StatusID], [MainImage], [BrandID]) VALUES (5, N'динь динь динь диринь', N'динь динь григорий  moroz', CAST(1499999.99 AS Decimal(10, 2)), 1, 5, 4, N'e8ri3i1aqlwtr1i2ydv9rn2js74ek1z3.png', 3)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductStatuses] ON 
GO
INSERT [dbo].[ProductStatuses] ([StatusID], [StatusName]) VALUES (1, N'В наличии')
GO
INSERT [dbo].[ProductStatuses] ([StatusID], [StatusName]) VALUES (2, N'Нет в наличии')
GO
INSERT [dbo].[ProductStatuses] ([StatusID], [StatusName]) VALUES (3, N'Архив')
GO
INSERT [dbo].[ProductStatuses] ([StatusID], [StatusName]) VALUES (4, N'Предзаказ')
GO
SET IDENTITY_INSERT [dbo].[ProductStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[Receipts] ON 
GO
INSERT [dbo].[Receipts] ([ReceiptID], [OrderID], [PdfPath], [CreatedAt]) VALUES (1, 2, N'Receipts/receipt_2.pdf', CAST(N'2026-04-09T14:55:49.487' AS DateTime))
GO
INSERT [dbo].[Receipts] ([ReceiptID], [OrderID], [PdfPath], [CreatedAt]) VALUES (2, 3, N'C:\Users\10230037\Source\Repos\CloneShop\CloneShop\bin\Debug\Receipts\receipt_3.pdf', CAST(N'2026-04-09T15:21:52.767' AS DateTime))
GO
INSERT [dbo].[Receipts] ([ReceiptID], [OrderID], [PdfPath], [CreatedAt]) VALUES (1002, 1002, N'C:\Users\10230037\Source\Repos\CloneShop\CloneShop\bin\Debug\Receipts\receipt_1002.pdf', CAST(N'2026-04-10T12:25:38.837' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Receipts] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Администратор')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Пользователь')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[TelegramLinks] ON 
GO
INSERT [dbo].[TelegramLinks] ([TelegramLinkID], [Url], [QrImagePath], [IsActive]) VALUES (5, N'https://t.me/maddysontg', N'qr-code.png', 1)
GO
SET IDENTITY_INSERT [dbo].[TelegramLinks] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Login], [Password], [Email], [Phone], [RoleID], [CreatedAt], [IsBlocked]) VALUES (10, N'Админ', N'admin', N'admin123', N'admin@mail.ru', N'7999000000', 1, CAST(N'2026-04-02T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Login], [Password], [Email], [Phone], [RoleID], [CreatedAt], [IsBlocked]) VALUES (12, N'Тест', N'user', N'user', N'user@mail.ru', N'7999191929', 1, CAST(N'2026-04-02T22:57:06.893' AS DateTime), 0)
GO
INSERT [dbo].[Users] ([UserID], [FullName], [Login], [Password], [Email], [Phone], [RoleID], [CreatedAt], [IsBlocked]) VALUES (13, N'чурбан', N'123', N'123', N'123@mail.ru', N'123123123123', 2, CAST(N'2026-04-09T13:54:22.700' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_Carts]    Script Date: 10.04.2026 13:51:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Carts] ON [dbo].[Carts]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users]    Script Date: 10.04.2026 13:51:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users] ON [dbo].[Users]
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CartItems] ADD  CONSTRAINT [DF_CartItems_Quantity]  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Carts] ADD  CONSTRAINT [DF_Carts_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_OrderDate]  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_TotalAmount]  DEFAULT ((0)) FOR [TotalAmount]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_QuantityInStock]  DEFAULT ((0)) FOR [QuantityInStock]
GO
ALTER TABLE [dbo].[Receipts] ADD  CONSTRAINT [DF_Receipts_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[TelegramLinks] ADD  CONSTRAINT [DF_TelegramLinks_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsBlocked]  DEFAULT ((0)) FOR [IsBlocked]
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_Carts] FOREIGN KEY([CartID])
REFERENCES [dbo].[Carts] ([CartID])
GO
ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_Carts]
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD  CONSTRAINT [FK_CartItems_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[CartItems] CHECK CONSTRAINT [FK_CartItems_Products]
GO
ALTER TABLE [dbo].[Carts]  WITH CHECK ADD  CONSTRAINT [FK_Carts_Carts] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Carts] CHECK CONSTRAINT [FK_Carts_Carts]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_OrderItems] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_OrderItems]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Orders]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrderStatuses] FOREIGN KEY([OrderStatusID])
REFERENCES [dbo].[OrderStatuses] ([OrderStatusID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrderStatuses]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD  CONSTRAINT [FK_ProductImages_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[ProductImages] CHECK CONSTRAINT [FK_ProductImages_Products]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Brands] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brands] ([BrandID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Brands]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_ProductStatuses] FOREIGN KEY([StatusID])
REFERENCES [dbo].[ProductStatuses] ([StatusID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_ProductStatuses]
GO
ALTER TABLE [dbo].[Receipts]  WITH CHECK ADD  CONSTRAINT [FK_Receipts_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[Receipts] CHECK CONSTRAINT [FK_Receipts_Orders]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
USE [master]
GO
ALTER DATABASE [CloneShopDB] SET  READ_WRITE 
GO
