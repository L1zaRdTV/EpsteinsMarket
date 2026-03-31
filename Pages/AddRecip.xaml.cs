using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using EpsteinsMarket.ApplicationData;
using EpsteinsMarket.Models;
using Microsoft.Win32;

namespace EpsteinsMarket.Pages
{
    public partial class AddRecip : Page
    {
        private readonly int _productId;
        private Product _currentProduct;
        private int _currentIndex;
        private readonly List<string> _images = new List<string>();

        public AddRecip(int id)
        {
            InitializeComponent();
            _productId = id;
            InitializePage();
        }

        private void InitializePage()
        {
            try
            {
                cbCategory.ItemsSource = AppConnect.model01.Categories.OrderBy(c => c.Name).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
                AppFrame.frmMain.Navigate(new PageTask());
                return;
            }

            if (_productId == 0)
            {
                tbTitle.Text = "Добавление товара";
                _currentProduct = new Product();
            }
            else
            {
                tbTitle.Text = "Редактирование товара";
                _currentProduct = AppConnect.model01.Products.FirstOrDefault(p => p.ID == _productId);

                if (_currentProduct == null)
                {
                    MessageBox.Show("Товар не найден.");
                    AppFrame.frmMain.Navigate(new PageTask());
                    return;
                }
            }

            DataContext = _currentProduct;
            LoadImageList();
            SetSelectedCategory();
        }

        private void LoadImageList()
        {
            _images.Clear();
            if (!string.IsNullOrWhiteSpace(_currentProduct.Image))
            {
                _images.AddRange(_currentProduct.Image
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Distinct());
            }

            _currentIndex = 0;
            ShowCurrentImage();
        }

        private void SetSelectedCategory()
        {
            if (_currentProduct == null)
            {
                return;
            }

            cbCategory.SelectedItem = AppConnect.model01.Categories.FirstOrDefault(c => c.ID == _currentProduct.CategoryID);
        }

        private void ShowCurrentImage()
        {
            if (_images.Count == 0)
            {
                imgProduct.Source = null;
                return;
            }

            string imageFileName = _images[_currentIndex];
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", imageFileName);

            if (!File.Exists(fullPath))
            {
                imgProduct.Source = null;
                return;
            }

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            imgProduct.Source = bitmap;
        }

        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp",
                Multiselect = true
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            try
            {
                string imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images");
                Directory.CreateDirectory(imagesDirectory);

                foreach (string file in dialog.FileNames)
                {
                    string newFileName = $"{Guid.NewGuid():N}{Path.GetExtension(file)}";
                    string destinationPath = Path.Combine(imagesDirectory, newFileName);
                    File.Copy(file, destinationPath, true);
                    _images.Add(newFileName);
                }

                _currentProduct.Image = string.Join(";", _images);
                _currentIndex = _images.Count - 1;
                ShowCurrentImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
            }
        }

        private void btnPrevImage_Click(object sender, RoutedEventArgs e)
        {
            if (_images.Count == 0)
            {
                return;
            }

            _currentIndex = (_currentIndex - 1 + _images.Count) % _images.Count;
            ShowCurrentImage();
        }

        private void btnNextImage_Click(object sender, RoutedEventArgs e)
        {
            if (_images.Count == 0)
            {
                return;
            }

            _currentIndex = (_currentIndex + 1) % _images.Count;
            ShowCurrentImage();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentProduct.Name))
            {
                MessageBox.Show("Введите название товара.");
                return;
            }

            if (!_currentProduct.Price.HasValue)
            {
                MessageBox.Show("Введите цену товара.");
                return;
            }

            if (_currentProduct.Price.Value < 0)
            {
                MessageBox.Show("Цена не может быть отрицательной.");
                return;
            }

            if (!(cbCategory.SelectedItem is Category selectedCategory))
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }

            try
            {
                Category dbCategory = AppConnect.model01.Categories.FirstOrDefault(c => c.ID == selectedCategory.ID);
                if (dbCategory == null)
                {
                    throw new InvalidOperationException("Выбранная категория не найдена в базе данных.");
                }

                _currentProduct.CategoryID = dbCategory.ID;
                _currentProduct.Image = string.Join(";", _images);

                if (_productId == 0)
                {
                    AppConnect.model01.Products.Add(_currentProduct);
                }

                AppConnect.model01.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения товара: {ex.Message}");
                return;
            }

            AppFrame.frmMain.Navigate(new PageTask());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new PageTask());
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Событие сохранено в соответствии с требованиями задания.
        }
    }
}
