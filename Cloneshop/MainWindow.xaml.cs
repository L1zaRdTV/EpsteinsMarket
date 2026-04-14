using System.Windows;
using CloneShop.ApplicationData;
using CloneShop.Pages;

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
                AppConnect.model01 = new EpsteinMarketDBEntities();
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