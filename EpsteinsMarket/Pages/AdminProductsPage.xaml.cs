using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpsteinMarket.ApplicationData;
using System.Data.Entity.Infrastructure;

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

        private void lvAdminProducts_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Products selectedProduct = lvAdminProducts.SelectedItem as Products;

            if (selectedProduct == null)
            {
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
                bool hasOrderItems = AppConnect.model01.OrderItems
                    .Any(x => x.ProductID == selectedProduct.ProductID);

                var cartItems = AppConnect.model01.CartItems
                    .Where(x => x.ProductID == selectedProduct.ProductID)
                    .ToList();

                foreach (var cartItem in cartItems)
                {
                    AppConnect.model01.CartItems.Remove(cartItem);
                }

                try
                {
                    if (hasOrderItems)
                    {
                        var endedStatus = AppConnect.model01.ProductStatuses
                            .FirstOrDefault(x => x.StatusName.ToLower().Contains("законч"));

                        if (endedStatus != null)
                        {
                            selectedProduct.StatusID = endedStatus.StatusID;
                        }

                        selectedProduct.QuantityInStock = 0;

                        AppConnect.model01.SaveChanges();
                        MessageBox.Show("Товар есть в истории заказов, поэтому он снят с продажи и удален из корзин.");
                    }
                    else
                    {
                        AppConnect.model01.Products.Remove(selectedProduct);
                        AppConnect.model01.SaveChanges();
                        MessageBox.Show("Товар удален");
                    }

                    LoadProducts();
                }
                catch (DbUpdateException)
                {
                    MessageBox.Show("Не удалось удалить товар из-за связанных данных. Товар оставлен в системе.");
                }
            }
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new AdminUsersPage());
        }
    }
}
