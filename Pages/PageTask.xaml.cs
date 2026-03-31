using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            try
            {
                InitializeFilters();
                LoadProducts();
                UpdateCounter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации страницы: {ex.Message}");
            }
        }

        private void InitializeFilters()
        {
            var categories = AppConnect.model01.Categories
                .OrderBy(c => c.Name)
                .ToList();

            categories.Insert(0, new Category { ID = 0, Name = "Все категории" });
            cbCategory.ItemsSource = categories;
            cbCategory.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            try
            {
                var query = AppConnect.model01.Products.AsQueryable();

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
                        query = query.OrderByDescending(p => p.CreatedAt);
                        break;
                    default:
                        query = query.OrderBy(p => p.Name);
                        break;
                }

                _products = query.ToList();
                lvProducts.ItemsSource = _products;
                icProducts.ItemsSource = _products;

                UpdateCounter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}");
            }
        }

        private void UpdateCounter()
        {
            tbCounter.Text = $"Найдено товаров: {_products.Count}";
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts();
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }

            LoadProducts();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }

            LoadProducts();
        }

        private void btnList_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.Visibility = Visibility.Visible;
            icProducts.Visibility = Visibility.Collapsed;
        }

        private void btnTile_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.Visibility = Visibility.Collapsed;
            icProducts.Visibility = Visibility.Visible;
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                MessageBox.Show("Редактирование доступно только администратору.");
                return;
            }

            if (sender is Button button && int.TryParse(button.Tag?.ToString(), out int id))
            {
                AppFrame.frmMain.Navigate(new AddRecip(id));
            }
        }

        private void btnFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (AppSession.CurrentUser == null)
            {
                MessageBox.Show("Сначала выполните вход.");
                return;
            }

            if (!(sender is Button button) || !int.TryParse(button.Tag?.ToString(), out int productId))
            {
                return;
            }

            try
            {
                bool favoriteExists = AppConnect.model01.Favorites
                    .Any(f => f.UserID == AppSession.CurrentUser.UserID && f.ProductID == productId);

                if (favoriteExists)
                {
                    MessageBox.Show("Товар уже есть в избранном.");
                    return;
                }

                AppConnect.model01.Favorites.Add(new Favorite
                {
                    UserID = AppSession.CurrentUser.UserID,
                    ProductID = productId
                });

                AppConnect.model01.SaveChanges();
                MessageBox.Show("Товар добавлен в избранное.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления в избранное: {ex.Message}");
            }
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                return;
            }

            if (lvProducts.SelectedItem is Product selectedProduct)
            {
                AppFrame.frmMain.Navigate(new AddRecip(selectedProduct.ID));
            }
        }
    }
}
