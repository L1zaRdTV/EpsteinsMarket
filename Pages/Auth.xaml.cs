using System;
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
            string login = tbLogin.Text.Trim();
            string password = pbPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                tbStatus.Text = "Введите логин и пароль.";
                return;
            }

            try
            {
                var user = AppConnect.model01.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                if (user == null)
                {
                    tbStatus.Text = "Неверный логин или пароль.";
                    return;
                }

                AppSession.CurrentUser = user;

                if (AppSession.IsAdmin)
                {
                    tbStatus.Text = "Авторизация успешна: администратор.";
                }
                else
                {
                    tbStatus.Text = "Авторизация успешна: пользователь.";
                }

                AppFrame.frmMain.Navigate(new PageTask());
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"Ошибка авторизации: {ex.Message}";
            }
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
