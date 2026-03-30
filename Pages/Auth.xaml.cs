using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpsteinsMarket.ApplicationData;

namespace EpsteinsMarket.Pages
{
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = tbLogin.Text.Trim();
            var password = pbPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                tbStatus.Text = "Введите логин и пароль.";
                return;
            }

            var user = AppConnect.model01.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user == null)
            {
                tbStatus.Text = "Неверный логин или пароль.";
                return;
            }

            AppSession.CurrentUser = user;
            tbStatus.Text = user.RoleID == 1 ? "Вход выполнен как администратор." : "Вход выполнен как пользователь.";
            AppFrame.frmMain.Navigate(new PageTask());
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new Reg());
        }

        private void tbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbStatus.Text = string.Empty;
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            tbStatus.Text = string.Empty;
        }
    }
}
