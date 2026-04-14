using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpsteinMarket.ApplicationData;

namespace EpsteinMarket.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        public AutorizationPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Введите логин");
                txtLogin.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                txtPassword.Focus();
                return;
            }

            var userObj = AppConnect.model01.Users.FirstOrDefault(x =>
                x.Login == login && x.Password == password);

            if (userObj == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                txtPassword.Clear();
                txtLogin.Focus();
                return;
            }

            if (userObj.IsBlocked)
            {
                MessageBox.Show("Ваш аккаунт заблокирован");
                return;
            }

            MessageBox.Show("Добро пожаловать, " + userObj.FullName);
            AppConnect.CurrentUser = userObj;
            AppFrame.frmMain.Navigate(new CatalogPage());
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new RegisterPage());
        }
    }
}
