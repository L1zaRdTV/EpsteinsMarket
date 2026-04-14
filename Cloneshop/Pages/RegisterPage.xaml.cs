using EpsteinMarket.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EpsteinMarket.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string login = txtLogin.Text;
            string password = txtPassword.Password;
            string passwordRepeat = txtPasswordRepeat.Password;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите имя");
                txtName.Focus();
                return;
            }

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

            if (string.IsNullOrWhiteSpace(passwordRepeat))
            {
                MessageBox.Show("Повторите пароль");
                txtPasswordRepeat.Focus();
                return;
            }

            if (password != passwordRepeat)
            {
                MessageBox.Show("Пароли не совпадают");
                txtPassword.Clear();
                txtPasswordRepeat.Clear();
                txtPassword.Focus();
                return;
            }
            var existingUser = AppConnect.model01.Users.FirstOrDefault(x => x.Login == login);

            if (existingUser != null)
            {
                MessageBox.Show("Такой логин уже занят");
                txtLogin.Focus();
                return;
            }
            var existingEmail = AppConnect.model01.Users.FirstOrDefault(x => x.Email == email);

            if (existingEmail != null)
            {
                MessageBox.Show("Почта уже занята");
                txtEmail.Focus();
                return;
            }
            var existingPhone = AppConnect.model01.Users.FirstOrDefault(x => x.Phone == phone);

            if (existingPhone != null)
            {
                MessageBox.Show("Пользователь с таким номером телефона уже зарегистрирован");
                txtPhone.Focus();
                return;
            }
            Users newUser = new Users();
            newUser.FullName = name;
            newUser.Login = login;
            newUser.Password = password;
            newUser.Email = email;
            newUser.Phone = phone;
            newUser.RoleID = 2;
            newUser.CreatedAt = DateTime.Now;
            newUser.IsBlocked = false;
            AppConnect.model01.Users.Add(newUser);
            AppConnect.model01.SaveChanges();
            Carts newCart = new Carts();
            newCart.UserID = newUser.UserID;
            newCart.CreatedAt = DateTime.Now;

            AppConnect.model01.Carts.Add(newCart);
            AppConnect.model01.SaveChanges();
            MessageBox.Show("Регистрация прошла успешно");
            AppFrame.frmMain.GoBack();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }
    }
}
