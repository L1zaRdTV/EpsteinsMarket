using System.Windows;
using EpsteinsMarket.ApplicationData;

namespace EpsteinsMarket
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!AppConnect.TryReconnect(out string error))
            {
                MessageBox.Show(
                    "Не удалось подключиться к базе данных EpsteinsMarket.\n" +
                    "Проверьте App.config (ActiveConnectionStringName), строки подключения или переменную окружения EPSTEINSMARKET_CONNECTION_STRING.\n\n" +
                    $"Техническая информация: {error}",
                    "Ошибка подключения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
