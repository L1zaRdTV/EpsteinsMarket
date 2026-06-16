using System.Windows;
using System.Data.Entity;
using WarehouseManagement.ApplicationData;
using WarehouseManagement.Pages;

namespace WarehouseManagement
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeDatabaseConnection();
            SetupNavigationSystem();
            LoadAuthorizationPage();
        }

        private void InitializeDatabaseConnection()
        {
            try
            {
                Database.SetInitializer<WarehouseDBEntitiesContext>(null);
                AppConnect.model01 = new WarehouseDBEntitiesContext();
                AppConnect.model01.Database.Initialize(false);
                AppConnect.IsDatabaseAvailable = true;
            }
            catch (System.Exception ex)
            {
                AppConnect.model01 = new OfflineWarehouseDbContext();
                AppConnect.IsDatabaseAvailable = false;
                MessageBox.Show(
                    "База данных недоступна, поэтому приложение запущено в автономном демонстрационном режиме. " +
                    "Данные доступны до закрытия программы.\n\nПричина: " + ex.Message,
                    "Автономный режим",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void SetupNavigationSystem()
        {
            AppFrame.frmMain = FrmMain;
        }

        private void LoadAuthorizationPage()
        {
            FrmMain.Navigate(new LoginPage());
        }
    }
}
