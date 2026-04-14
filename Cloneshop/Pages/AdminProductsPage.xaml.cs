using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpsteinMarket.ApplicationData;

namespace EpsteinMarket.Pages
{
    public partial class AdminProductsPage : Page
    {
        public AdminProductsPage()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            lvAdminProducts.ItemsSource = AppConnect.model01.Products.ToList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new AddEditProductPage(null));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Products selectedProduct = lvAdminProducts.SelectedItem as Products;

            if (selectedProduct == null)
            {
                MessageBox.Show("Выберите товар для редактирования");
                return;
            }

            AppFrame.frmMain.Navigate(new AddEditProductPage(selectedProduct));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Products selectedProduct = lvAdminProducts.SelectedItem as Products;

            if (selectedProduct == null)
            {
                MessageBox.Show("Выберите товар для удаления");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Удалить товар \"" + selectedProduct.ProductName + "\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                AppConnect.model01.Products.Remove(selectedProduct);
                AppConnect.model01.SaveChanges();

                MessageBox.Show("Товар удален");
                LoadProducts();
            }
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new AdminUsersPage());
        }
    }
}