using EpsteinMarket.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EpsteinMarket.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        public CatalogPage()
        {
            InitializeComponent();
            if (AppConnect.CurrentUser != null)
            {
                tbWelcome.Text = "Добро пожаловать, " + AppConnect.CurrentUser.FullName;
            }
            if (AppConnect.CurrentUser.RoleID == 1)
            {
                tbRole.Text = "Роль: Администратор";
                btnManageProducts.Visibility = Visibility.Visible;
                btnManageUsers.Visibility = Visibility.Visible;
            }
            else
            {
                tbRole.Text = "Роль: Пользователь";
                btnManageProducts.Visibility = Visibility.Collapsed;
                btnManageUsers.Visibility = Visibility.Collapsed;
            }
            
            LoadCategories();
            LoadSort();
            LoadProducts();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.CurrentUser = null;
            AppFrame.frmMain.Navigate(new AutorizationPage());

        }

        private void btnManageProducts_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new AdminProductsPage());
        }

        private void btnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new AdminUsersPage());
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts();
        }
        private string GetImagePath(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
                return null;

            string imagePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Images",
                imageName);

            if (System.IO.File.Exists(imagePath))
                return imagePath;

            return null;
        }
        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            Products selectedProduct = button.Tag as Products;

            if (selectedProduct == null)
                return;

            AppFrame.frmMain.Navigate(new ProductDetailsPage(selectedProduct));
        }
        private void LoadProducts()
        {
            var products = AppConnect.model01.Products.ToList();

            if (cmbCategory.SelectedIndex > 0)
            {
                string selectedCategory = cmbCategory.SelectedItem.ToString();

                products = products.Where(x =>
                    x.Categories.CategoryName == selectedCategory
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                products = products.Where(x =>
                    x.ProductName.ToLower().Contains(txtSearch.Text.ToLower()) ||
                    (x.Description != null && x.Description.ToLower().Contains(txtSearch.Text.ToLower()))
                ).ToList();
            }

            if (cmbSort.SelectedIndex == 1)
            {
                products = products.OrderBy(x => x.Price).ToList();
            }
            else if (cmbSort.SelectedIndex == 2)
            {
                products = products.OrderByDescending(x => x.Price).ToList();
            }

            var displayItems = products.Select(x => new ProductDisplayItem
            {
                SourceProduct = x,
                ProductName = x.ProductName,
                Price = x.Price,
                QuantityInStock = x.QuantityInStock,
                Description = x.Description,
                ImagePath = GetImagePath(x.MainImage)
            }).ToList();

            lvProducts.ItemsSource = displayItems;
        }


        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProducts();
        }
        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Все категории");

            var categories = AppConnect.model01.Categories.ToList();

            foreach (var category in categories)
            {
                cmbCategory.Items.Add(category.CategoryName);
            }

            cmbCategory.SelectedIndex = 0;
        }
        private void LoadSort()
        {
            cmbSort.Items.Clear();

            cmbSort.Items.Add("Без сортировки");
            cmbSort.Items.Add("Цена по возрастанию");
            cmbSort.Items.Add("Цена по убыванию");

            cmbSort.SelectedIndex = 0;
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProducts();
        }
        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            Products selectedProduct = button.Tag as Products;

            if (selectedProduct == null)
            {
                MessageBox.Show("Товар не выбран");
                return;
            }

            var userCart = AppConnect.model01.Carts
                .FirstOrDefault(x => x.UserID == AppConnect.CurrentUser.UserID);

            if (userCart == null)
            {
                MessageBox.Show("Корзина пользователя не найдена");
                return;
            }

            var existingCartItem = AppConnect.model01.CartItems
                .FirstOrDefault(x =>
                    x.CartID == userCart.CartID &&
                    x.ProductID == selectedProduct.ProductID);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
            }
            else
            {
                CartItems newCartItem = new CartItems();
                newCartItem.CartID = userCart.CartID;
                newCartItem.ProductID = selectedProduct.ProductID;
                newCartItem.Quantity = 1;
                newCartItem.PriceAtMoment = selectedProduct.Price;

                AppConnect.model01.CartItems.Add(newCartItem);
            }

            AppConnect.model01.SaveChanges();

            MessageBox.Show("Товар добавлен в корзину");
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new CartPage());
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new ProfilePage());
        }

        public class ProductDisplayItem
        {
            public Products SourceProduct { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int QuantityInStock { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
        }
    }

}
