using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using EpsteinsMarket.ApplicationData;
using EpsteinsMarket.Models;

namespace EpsteinsMarket.Pages
{
    public partial class PageTask : Page
    {
        private List<Product> _products = new List<Product>();

        public PageTask()
        {
            InitializeComponent();
            InitializePage();
        }

        private void InitializePage()
        {
            InitializeFilters();
            LoadProducts();
            ApplyRolePermissions();
        }

        private void ApplyRolePermissions()
        {
            btnAdd.Visibility = AppSession.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void InitializeFilters()
        {
            try
            {
                List<Category> categories = AppConnect.model01.Categories.OrderBy(c => c.Name).ToList();
                categories.Insert(0, new Category { ID = 0, Name = "Все категории" });
                cbCategory.ItemsSource = categories;
                cbCategory.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
            }
        }

        private void LoadProducts()
        {
            try
            {
                var query = AppConnect.model01.Products.AsNoTracking().AsQueryable();

                string searchText = tbSearch.Text?.Trim() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(p => p.Name.Contains(searchText));
                }

                if (cbCategory.SelectedItem is Category selectedCategory && selectedCategory.ID > 0)
                {
                    query = query.Where(p => p.CategoryID == selectedCategory.ID);
                }

                string sortMode = (cbSort.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "default";
                switch (sortMode)
                {
                    case "price_asc":
                        query = query.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(p => p.Price);
                        break;
                    case "new":
                        query = query.OrderByDescending(p => p.ID);
                        break;
                    default:
                        query = query.OrderBy(p => p.Name);
                        break;
                }

                _products = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}");
                _products = new List<Product>();
            }

            icProducts.ItemsSource = _products;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            tbCounter.Text = $"Найдено товаров: {_products.Count}";
        }

        private Product GetProductByButtonTag(object sender)
        {
            if (!(sender is Button button) || !int.TryParse(button.Tag?.ToString(), out int productId))
            {
                return null;
            }

            return _products.FirstOrDefault(p => p.ID == productId)
                ?? AppConnect.model01.Products.FirstOrDefault(p => p.ID == productId);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e) => LoadProducts();

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                LoadProducts();
            }
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                LoadProducts();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                MessageBox.Show("Добавление товаров доступно только администратору.");
                return;
            }

            AppFrame.frmMain.Navigate(new AddRecip(0));
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (AppSession.CurrentUser == null)
            {
                MessageBox.Show("Сначала выполните вход.");
                return;
            }

            Product product = GetProductByButtonTag(sender);
            if (product == null)
            {
                return;
            }

            AppSession.CartProducts.Add(product);
            MessageBox.Show($"\"{product.Name}\" добавлен в корзину.");
        }

        private void btnCabinet_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new ProfilePage());
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            AppSession.CurrentUser = null;
            AppSession.CartProducts.Clear();
            AppSession.PurchasedProducts.Clear();
            AppFrame.frmMain.Navigate(new Auth());
        }
    }

    public class ProductImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string firstImage = value?.ToString()?
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(firstImage))
            {
                return null;
            }

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", firstImage);
            if (!File.Exists(fullPath))
            {
                return null;
            }

            return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
