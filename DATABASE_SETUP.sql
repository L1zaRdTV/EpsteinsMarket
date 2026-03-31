/* EpsteinsMarket: скрипт инициализации БД для магазина гномиков (SQL Server) */
SET NOCOUNT ON;
GO

IF DB_ID(N'EpsteinsMarket') IS NULL
BEGIN
    CREATE DATABASE EpsteinsMarket;
END
GO

USE EpsteinsMarket;
GO

/* Пересоздаём таблицы в правильном порядке */
IF OBJECT_ID(N'dbo.Favorites', N'U') IS NOT NULL DROP TABLE dbo.Favorites;
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID(N'dbo.Categories', N'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
GO

CREATE TABLE dbo.Users
(
    UserID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    BirthDate DATE NULL,
    Experience INT NULL,
    Login NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    [Role] NVARCHAR(20) NULL,
    CONSTRAINT CK_Users_Experience CHECK (Experience IS NULL OR Experience >= 0)
);
GO

CREATE TABLE dbo.Categories
(
    CategoryID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CategoryName NVARCHAR(150) NOT NULL UNIQUE
);
GO

CREATE TABLE dbo.Products
(
    ProductID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(250) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    Price DECIMAL(18,2) NOT NULL,
    [Image] NVARCHAR(500) NULL,
    CategoryID INT NOT NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES dbo.Categories(CategoryID),
    CONSTRAINT CK_Products_Price CHECK (Price >= 0)
);
GO

CREATE TABLE dbo.Favorites
(
    FavoriteID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserID INT NOT NULL,
    ProductID INT NOT NULL,
    AddedAt DATETIME2(0) NOT NULL CONSTRAINT DF_Favorites_AddedAt DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Favorites_Users FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID),
    CONSTRAINT FK_Favorites_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT UQ_Favorites_UserProduct UNIQUE(UserID, ProductID)
);
GO

CREATE INDEX IX_Products_CategoryID ON dbo.Products(CategoryID);
CREATE INDEX IX_Favorites_UserID ON dbo.Favorites(UserID);
CREATE INDEX IX_Favorites_ProductID ON dbo.Favorites(ProductID);
GO

/* Пользователи */
INSERT INTO dbo.Users (FullName, BirthDate, Experience, Login, [Password], Email, Phone, [Role])
VALUES
(N'Администратор витрины', '1990-05-10', 10, N'admin', N'admin1234', N'admin@epsteinsmarket.local', N'+79990000000', N'Администратор'),
(N'Олег Гномов', '1994-03-22', 6, N'oleg', N'oleg1234', N'oleg@market.local', N'+79991112233', N'Пользователь'),
(N'Мария Лесная', '1996-08-14', 4, N'maria', N'maria1234', N'maria@market.local', N'+79994445566', N'Пользователь'),
(N'Игорь Камнелом', '1988-11-01', 12, N'igor', N'igor1234', N'igor@market.local', N'+79990001122', N'Пользователь'),
(N'Анна Подгорная', '1999-01-17', 2, N'anna', N'anna1234', N'anna@market.local', N'+79993334455', N'Пользователь'),
(N'Семён Фонарщик', '1992-06-09', 7, N'semen', N'semen1234', N'semen@market.local', N'+79997778899', N'Пользователь'),
(N'Леонид Мастерок', '1985-09-30', 15, N'leonid', N'leonid1234', N'leonid@market.local', N'+79995556677', N'Пользователь'),
(N'Екатерина Речная', '2000-12-12', 1, N'katya', N'katya1234', N'katya@market.local', N'+79992223344', N'Пользователь');
GO

/* Категории товаров для магазина гномиков */
INSERT INTO dbo.Categories (CategoryName)
VALUES
(N'Садовые гномы'),
(N'Интерьерные гномы'),
(N'Сезонные коллекции'),
(N'Гномы с подсветкой'),
(N'Аксессуары для гномов'),
(N'Подарочные наборы');
GO

/* Большой каталог товаров */
INSERT INTO dbo.Products ([Name], [Description], Price, [Image], CategoryID)
VALUES
(N'Гном-страж «Гранит» 30 см', N'Классический садовый гном из полистоуна, устойчив к дождю и солнцу.', 2490.00, N'images/gnome_guard_granit_30.png', 1),
(N'Гном-рыбак «Тихая бухта» 28 см', N'Фигурка с удочкой и ведёрком, отлично смотрится у пруда.', 2690.00, N'images/gnome_fisher_bay_28.png', 1),
(N'Гном-шахтёр «Рудокоп» 32 см', N'Детализированный гном с киркой и фонарём.', 2890.00, N'images/gnome_miner_32.png', 1),
(N'Гном «Лесной сторож» 40 см', N'Крупная садовая фигура для входной зоны.', 4590.00, N'images/gnome_forest_guard_40.png', 1),
(N'Гном «Яблоневый сад» 25 см', N'Яркая фигурка для клумб и плодового сада.', 1990.00, N'images/gnome_apple_garden_25.png', 1),
(N'Гном-почтальон «Добрые вести» 27 см', N'Фигурка с мини-почтовым ящиком.', 2390.00, N'images/gnome_postman_27.png', 1),
(N'Гном «Утренняя лейка» 31 см', N'Гном с лейкой, подчёркивает дачный стиль.', 2590.00, N'images/gnome_watering_31.png', 1),
(N'Гном «Тропинка удачи» 35 см', N'Гном с табличкой для садовых дорожек.', 3290.00, N'images/gnome_lucky_path_35.png', 1),

(N'Каминный гном «Тёплый вечер» 22 см', N'Интерьерная фигурка с мягкими цветами и матовым покрытием.', 2190.00, N'images/gnome_fireplace_22.png', 2),
(N'Гном-книголюб «Библиотекарь» 20 см', N'Идеален для полки с книгами и кабинета.', 1890.00, N'images/gnome_booklover_20.png', 2),
(N'Гном «Сканди» 18 см', N'Минималистичный интерьерный стиль.', 1590.00, N'images/gnome_scandi_18.png', 2),
(N'Гном «Лофт-мастер» 24 см', N'Бетонная текстура, современный декор.', 2290.00, N'images/gnome_loft_24.png', 2),
(N'Гном «Керамика премиум» 26 см', N'Глянцевая керамика ручной росписи.', 3490.00, N'images/gnome_ceramic_26.png', 2),
(N'Гном «Сонный хранитель» 19 см', N'Небольшой декор для спальни.', 1490.00, N'images/gnome_sleepy_19.png', 2),
(N'Гном «Рабочий стол» 16 см', N'Компактный гном для рабочего места.', 1290.00, N'images/gnome_desk_16.png', 2),
(N'Гном «Музыкальный» 23 см', N'Интерьерная модель с мини-гитарой.', 2090.00, N'images/gnome_music_23.png', 2),

(N'Гном «Новогодний колпак» 30 см', N'Праздничная модель с блестящим декором.', 2790.00, N'images/gnome_newyear_30.png', 3),
(N'Гном «Рождественский фонарь» 29 см', N'Сезонная фигурка с декоративным фонариком.', 2990.00, N'images/gnome_christmas_lantern_29.png', 3),
(N'Гном «Валентинка» 21 см', N'Подарочный гном с сердцем.', 1790.00, N'images/gnome_valentine_21.png', 3),
(N'Гном «Весенний» 24 см', N'Нежные пастельные оттенки, тема первоцветов.', 1990.00, N'images/gnome_spring_24.png', 3),
(N'Гном «Пасхальный» 26 см', N'Фигурка с декоративным яйцом.', 2390.00, N'images/gnome_easter_26.png', 3),
(N'Гном «Хэллоуин» 28 см', N'С тыквой и тематической шляпой.', 2690.00, N'images/gnome_halloween_28.png', 3),
(N'Гном «Осенний урожай» 27 см', N'С корзиной яблок и листьев.', 2490.00, N'images/gnome_autumn_harvest_27.png', 3),
(N'Гном «Летний пикник» 25 см', N'Сезонная лимитированная серия.', 2290.00, N'images/gnome_summer_picnic_25.png', 3),

(N'LED-гном «Ночной дозор» 30 см', N'Встроенная тёплая LED-подсветка, питание от батареек.', 3590.00, N'images/gnome_led_watch_30.png', 4),
(N'Солар-гном «Светлячок» 33 см', N'Заряжается от солнечной панели днём.', 3890.00, N'images/gnome_solar_firefly_33.png', 4),
(N'Гном «Лунный фонарь» 27 см', N'Мягкое холодное свечение для веранды.', 3190.00, N'images/gnome_moon_lantern_27.png', 4),
(N'Гном «Свет в окне» 22 см', N'Компактная светящаяся интерьерная версия.', 2690.00, N'images/gnome_window_light_22.png', 4),
(N'Гном «Радужная гирлянда» 28 см', N'Многоцветная подсветка с переключением режимов.', 4090.00, N'images/gnome_rainbow_28.png', 4),
(N'Гном «Сумеречный сад» 35 см', N'Крупная световая модель для сада.', 4890.00, N'images/gnome_twilight_garden_35.png', 4),
(N'Гном «Мини-ночник» 17 см', N'Ночной светильник для детской.', 1990.00, N'images/gnome_nightlight_17.png', 4),
(N'Гном «Звёздная тропа» 31 см', N'Декоративная подсветка дорожек.', 4290.00, N'images/gnome_starlight_path_31.png', 4),

(N'Шляпа для гнома «Классика»', N'Сменная шляпа из влагостойкого материала.', 590.00, N'images/access_hat_classic.png', 5),
(N'Шляпа для гнома «Премиум бархат»', N'Декоративная шляпа премиум-класса.', 990.00, N'images/access_hat_premium.png', 5),
(N'Мини-лейка для декора', N'Аксессуар для сюжетных композиций.', 450.00, N'images/access_watering_can.png', 5),
(N'Фонарик декоративный', N'Мини-фонарь для фигурок гномов.', 690.00, N'images/access_lantern.png', 5),
(N'Табличка «Добро пожаловать»', N'Табличка для установки рядом с фигуркой.', 790.00, N'images/access_welcome_sign.png', 5),
(N'Подставка каменная 20 см', N'Устойчивая подставка для гномов.', 1190.00, N'images/access_stand_20.png', 5),
(N'Подставка каменная 30 см', N'Усиленная подставка для больших моделей.', 1490.00, N'images/access_stand_30.png', 5),
(N'Набор красок для реставрации', N'Подходит для восстановления росписи.', 890.00, N'images/access_paint_set.png', 5),

(N'Подарочный набор «Дуэт садоводов»', N'Два садовых гнома + аксессуары.', 5190.00, N'images/set_garden_duo.png', 6),
(N'Подарочный набор «Светлая ночь»', N'LED-гном + декоративный фонарь.', 5690.00, N'images/set_light_night.png', 6),
(N'Подарочный набор «Домашний уют»', N'Интерьерный гном + мини-коврик.', 3990.00, N'images/set_home_comfort.png', 6),
(N'Подарочный набор «4 сезона»', N'Четыре сезонные фигурки в одном наборе.', 8990.00, N'images/set_4_seasons.png', 6),
(N'Подарочный набор «Семья гномов»', N'Три фигурки разного размера.', 6490.00, N'images/set_gnome_family.png', 6),
(N'Подарочный набор «Дачный старт»', N'Гном-страж + подставка + табличка.', 4590.00, N'images/set_dacha_start.png', 6),
(N'Подарочный набор «Коллекционер»', N'Лимитированный бокс с сертификатом.', 12990.00, N'images/set_collector_box.png', 6),
(N'Подарочный набор «Новоселье»', N'Интерьерный гном + приветственная табличка.', 4290.00, N'images/set_housewarming.png', 6);
GO

/* Избранные товары пользователей */
INSERT INTO dbo.Favorites (UserID, ProductID)
VALUES
(2, 1), (2, 9), (2, 18), (2, 25), (2, 41),
(3, 4), (3, 12), (3, 20), (3, 29), (3, 45),
(4, 3), (4, 7), (4, 24), (4, 31), (4, 47),
(5, 6), (5, 14), (5, 22), (5, 33), (5, 48),
(6, 2), (6, 10), (6, 19), (6, 28), (6, 43),
(7, 8), (7, 16), (7, 23), (7, 30), (7, 46),
(8, 5), (8, 11), (8, 21), (8, 34), (8, 44);
GO

/* Быстрая проверка наполненности */
SELECT
    (SELECT COUNT(*) FROM dbo.Users) AS UsersCount,
    (SELECT COUNT(*) FROM dbo.Categories) AS CategoriesCount,
    (SELECT COUNT(*) FROM dbo.Products) AS ProductsCount,
    (SELECT COUNT(*) FROM dbo.Favorites) AS FavoritesCount;
GO
