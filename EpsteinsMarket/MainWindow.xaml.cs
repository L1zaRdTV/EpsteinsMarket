using System.Windows;
using System.Data.Entity;
using EpsteinMarket.ApplicationData;
using EpsteinMarket.Pages;

namespace EpsteinMarket
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
                Database.SetInitializer<EpsteinMarketDBEntities>(null);
                AppConnect.model01 = new EpsteinMarketDBEntities();
                AppConnect.model01.Database.Initialize(false);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    "Не удалось подключиться к базе данных:\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SetupNavigationSystem()
        {
            AppFrame.frmMain = FrmMain;
        }

        private void LoadAuthorizationPage()
        {
            FrmMain.Navigate(new AutorizationPage());
        }
    }
}
