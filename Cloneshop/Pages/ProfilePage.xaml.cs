using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpsteinMarket.ApplicationData;
using System.Diagnostics;
using System.IO;

namespace EpsteinMarket.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            LoadUserData();
            LoadOrders();
            LoadReceipts();
        }

        private void LoadUserData()
        {
            if (AppConnect.CurrentUser == null)
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            tbFullName.Text = "Имя: " + AppConnect.CurrentUser.FullName;
            tbLogin.Text = "Логин: " + AppConnect.CurrentUser.Login;
            tbEmail.Text = "Email: " + AppConnect.CurrentUser.Email;
            tbPhone.Text = "Телефон: " + AppConnect.CurrentUser.Phone;
            tbCreatedAt.Text = "Дата регистрации: " + AppConnect.CurrentUser.CreatedAt;
        }

        private void LoadOrders()
        {
            var orders = AppConnect.model01.Orders
                .Where(x => x.UserID == AppConnect.CurrentUser.UserID)
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            lvOrders.ItemsSource = orders;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }
        private void LoadReceipts()
        {
            var receipts = AppConnect.model01.Receipts
                .Where(x => x.Orders.UserID == AppConnect.CurrentUser.UserID)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            lvReceipts.ItemsSource = receipts;
        }
        private void btnOpenReceipt_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            Receipts selectedReceipt = button.Tag as Receipts;

            if (selectedReceipt == null)
                return;

            if (!File.Exists(selectedReceipt.PdfPath))
            {
                MessageBox.Show("Файл чека не найден");
                return;
            }

            Process.Start(selectedReceipt.PdfPath);
        }
    }
}