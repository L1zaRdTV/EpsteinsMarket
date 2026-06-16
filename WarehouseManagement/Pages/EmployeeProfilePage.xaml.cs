using System;
using System.Windows;
using System.Windows.Controls;
using WarehouseManagement.ApplicationData;

namespace WarehouseManagement.Pages
{
    public partial class EmployeeProfilePage : Page
    {
        private bool isEditing;

        public EmployeeProfilePage()
        {
            InitializeComponent();
            LoadProfile();
        }

        private void LoadProfile()
        {
            var user = AppConnect.CurrentUser;
            tbFullName.Text = "ФИО: " + user.FullName;
            tbLogin.Text = "Логин: " + user.Login;
            tbEmail.Text = "Email: " + user.Email;
            tbPhone.Text = "Телефон: " + user.Phone;
            tbRole.Text = "Роль: " + (user.UserRoles == null ? "не указана" : user.UserRoles.RoleName);
            tbCreatedAt.Text = "Дата регистрации: " + user.CreatedAt.ToString("dd.MM.yyyy HH:mm");

            txtFullName.Text = user.FullName;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;
        }

        private void SetEditMode(bool value)
        {
            isEditing = value;
            spReadOnlyInfo.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            spEditInfo.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            btnCancelEdit.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            btnEditProfile.Content = value ? "Сохранить" : "Изменить данные";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                SetEditMode(true);
                return;
            }

            var user = AppConnect.CurrentUser;
            user.FullName = txtFullName.Text.Trim();
            user.Email = txtEmail.Text.Trim();
            user.Phone = txtPhone.Text.Trim();
            AppConnect.model01.SaveChanges();
            WarehouseService.Log("Изменение", "Users", user.UserID, "Пользователь обновил данные профиля");
            LoadProfile();
            SetEditMode(false);
        }

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            LoadProfile();
            SetEditMode(false);
        }
    }
}
