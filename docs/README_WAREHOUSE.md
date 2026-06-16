# WarehouseManagement

WPF-приложение для учета склада. Проект больше не содержит старого имени приложения: корневая папка проекта, пространства имен и XAML-классы переименованы в `WarehouseManagement`.

## База данных

Основная схема находится в `database/warehouse_schema_and_seed.sql`. Подключение выполняется через EF6/MySQL по строке `WarehouseDBEntities` в `WarehouseManagement/App.config`. Модель `WarehouseManagement/ApplicationData/DatabaseModels.cs` повторяет структуру SQL-скрипта и используется как кодовая часть ADO.NET EDM-подхода; файл `WarehouseManagement/ApplicationData/WarehouseModel.edmx` добавлен как точка для регенерации EDMX из базы в Visual Studio.

## Автономный режим

Если MySQL недоступен, приложение автоматически переключается на `OfflineWarehouseDbContext`. Этот контекст заполняет демонстрационные справочники, пользователя `admin/admin`, товар, остаток и отгрузку в памяти. В автономном режиме изменения сохраняются только до закрытия приложения.
