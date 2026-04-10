# Устранение ошибки «Не найден компонент …» в EpsteinsMarket

Если при открытии/сборке проекта появляются ошибки вида:

- `Не найден компонент "System"`
- `Не найден компонент "PresentationFramework"`
- `Не найден компонент "System.Data"`
- `Не найден компонент "EntityFramework"`

то обычно причина в окружении, а не в коде проекта.

## Почему это происходит

Проект `EpsteinsMarket` — это **WPF на .NET Framework 4.7.2** (не .NET 6/7/8).
Для таких проектов нужны:

1. Visual Studio с desktop-workload для .NET Framework
2. Targeting Pack/Developer Pack для .NET Framework 4.7.2
3. Восстановленные NuGet-пакеты (`EntityFramework`)

Если открыть проект в среде без .NET Framework reference assemblies (например, только .NET SDK), MSBuild не сможет найти системные сборки (`System`, `System.Core`, `WindowsBase`, `PresentationCore` и т.д.).

## Что сделать (Windows)

1. Установите Visual Studio 2022 (или 2019) с workload:
   - **.NET desktop development**.

2. Убедитесь, что установлен:
   - **.NET Framework 4.7.2 Developer Pack (Targeting Pack)**.

3. В корне решения выполните восстановление пакетов:

   ```powershell
   nuget restore .\EpsteinsMarket.sln
   ```

   или через Visual Studio: **Restore NuGet Packages**.

4. Откройте `EpsteinsMarket.sln` (а не только `.csproj`) и пересоберите решение.

## Как понять, что всё исправилось

После корректной установки workload + targeting pack должны исчезнуть ошибки по:

- `System*`
- `Presentation*`
- `WindowsBase`
- `EntityFramework*`

и проект начнёт собираться в конфигурации `Debug|AnyCPU`.

## Важно

На Linux/macOS этот WPF-проект в текущем виде не собирается, потому что ориентирован на .NET Framework/WPF (Windows-only стек).
