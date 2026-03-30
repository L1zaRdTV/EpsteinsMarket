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
            if (!ValidateAllFields(out string errorText))
            {
                tbStatus.Text = errorText;
                return;
            }

            string login = tbLogin.Text.Trim();

            if (AppConnect.model01.Users.Any(u => u.Login == login))
            {
                tbStatus.Text = "Пользователь с таким логином уже существует.";
                return;
            }

            int roleId = AppConnect.model01.Roles.FirstOrDefault(r => r.Name == "Пользователь")?.ID ?? 2;

            var newUser = new User
            {
                UserName = tbUserName.Text.Trim(),
                BirthDate = dpBirthDate.SelectedDate.Value,
                Experience = tbExperience.Text.Trim(),
                Login = login,
                Password = pbPassword.Password,
                Email = tbEmail.Text.Trim(),
                Phone = tbPhone.Text.Trim(),
                RoleID = roleId
            };

            AppConnect.model01.Users.Add(newUser);
            AppConnect.model01.SaveChanges();

            MessageBox.Show("Регистрация выполнена успешно.");
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
                message = "Заполните все поля формы регистрации.";
                return false;
            }

            if (dpBirthDate.SelectedDate.Value > DateTime.Now.Date)
            {
                message = "Дата рождения не может быть больше текущей даты.";
                return false;
            }

            if (pbPassword.Password != pbConfirmPassword.Password)
            {
                message = "Пароль и подтверждение не совпадают.";
                return false;
            }

            if (pbPassword.Password.Length < 4)
            {
                message = "Пароль должен содержать минимум 4 символа.";
                return false;
            }

            if (!Regex.IsMatch(tbEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                message = "Некорректный формат email.";
                return false;
            }

            if (!Regex.IsMatch(tbPhone.Text.Trim(), @"^\+?[0-9\-\s\(\)]{6,20}$"))
            {
                message = "Некорректный формат телефона.";
                return false;
            }

            return true;
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool passwordsMatch = !string.IsNullOrWhiteSpace(pbPassword.Password)
                                  && pbPassword.Password == pbConfirmPassword.Password;
            btnRegister.IsEnabled = passwordsMatch;

            if (!passwordsMatch)
            {
                tbStatus.Text = "Кнопка регистрации станет активной после совпадения паролей.";
            }
            else
            {
                tbStatus.Text = string.Empty;
            }
        }

        private void tbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbStatus.Text = string.Empty;

            string login = tbLogin.Text.Trim();
            if (string.IsNullOrWhiteSpace(login))
            {
                tbLoginHint.Text = string.Empty;
                return;
            }

            bool loginExists = AppConnect.model01.Users.Any(u => u.Login == login);
            tbLoginHint.Text = loginExists ? "Логин уже занят." : "Логин свободен.";
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
