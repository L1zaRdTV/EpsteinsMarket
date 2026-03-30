using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using EpsteinsMarket.ApplicationData;
using EpsteinsMarket.Models;

namespace EpsteinsMarket.Pages
{
    public partial class Reg : Page
    {
        public Reg()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAllFields(out var error))
            {
                tbStatus.Text = error;
                return;
            }

            var login = tbLogin.Text.Trim();
            bool exists = AppConnect.model01.Users.Any(u => u.Login == login);
            if (exists)
            {
                tbStatus.Text = "Пользователь с таким логином уже существует.";
                return;
            }

            int userRoleId = AppConnect.model01.Roles.FirstOrDefault(r => r.Name == "Пользователь")?.ID ?? 2;

            var user = new User
            {
                UserName = tbUserName.Text.Trim(),
                BirthDate = dpBirthDate.SelectedDate.Value,
                Experience = tbExperience.Text.Trim(),
                Login = login,
                Password = pbPassword.Password,
                Email = tbEmail.Text.Trim(),
                Phone = tbPhone.Text.Trim(),
                RoleID = userRoleId
            };

            AppConnect.model01.Users.Add(user);
            AppConnect.model01.SaveChanges();

            tbStatus.Text = "Пользователь зарегистрирован. Выполните вход.";
            AppFrame.frmMain.Navigate(new Auth());
        }

        private bool ValidateAllFields(out string message)
        {
            message = string.Empty;
            if (string.IsNullOrWhiteSpace(tbUserName.Text)
                || !dpBirthDate.SelectedDate.HasValue
                || string.IsNullOrWhiteSpace(tbExperience.Text)
                || string.IsNullOrWhiteSpace(tbLogin.Text)
                || string.IsNullOrWhiteSpace(pbPassword.Password)
                || string.IsNullOrWhiteSpace(pbConfirmPassword.Password)
                || string.IsNullOrWhiteSpace(tbEmail.Text)
                || string.IsNullOrWhiteSpace(tbPhone.Text))
            {
                message = "Заполните все поля формы.";
                return false;
            }

            if (pbPassword.Password != pbConfirmPassword.Password)
            {
                message = "Пароли не совпадают.";
                return false;
            }

            if (dpBirthDate.SelectedDate > DateTime.Now)
            {
                message = "Дата рождения не может быть в будущем.";
                return false;
            }

            if (!Regex.IsMatch(tbEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                message = "Некорректный формат электронной почты.";
                return false;
            }

            if (!Regex.IsMatch(tbPhone.Text.Trim(), @"^[0-9\+\-\(\)\s]{6,20}$"))
            {
                message = "Некорректный формат телефона.";
                return false;
            }

            return true;
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool passwordsMatch = pbPassword.Password == pbConfirmPassword.Password && !string.IsNullOrWhiteSpace(pbPassword.Password);
            btnRegister.IsEnabled = passwordsMatch;
            tbStatus.Text = passwordsMatch ? string.Empty : "Кнопка регистрации активируется после совпадения паролей.";
        }

        private void tbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            string login = tbLogin.Text.Trim();
            if (string.IsNullOrWhiteSpace(login))
            {
                tbLoginHint.Text = string.Empty;
                return;
            }

            bool exists = AppConnect.model01.Users.Any(u => u.Login == login);
            tbLoginHint.Text = exists ? "Логин уже занят." : "Логин свободен.";
        }

        private void AnyField_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbStatus.Text = string.Empty;
        }

        private void dpBirthDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbStatus.Text = string.Empty;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new Auth());
        }
    }
}
