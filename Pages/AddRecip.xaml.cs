using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using EpsteinsMarket.ApplicationData;
using EpsteinsMarket.Models;

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
            cbCategory.ItemsSource = AppConnect.model01.Categories.OrderBy(c => c.Name).ToList();

            if (_productId == 0)
            {
                tbTitle.Text = "Добавление товара";
                _currentProduct = new Product { CreatedAt = DateTime.Now };
            }
            else
            {
                tbTitle.Text = "Редактирование товара";
                _currentProduct = AppConnect.model01.Products.FirstOrDefault(p => p.ID == _productId) ?? new Product();
            }

            DataContext = _currentProduct;
            SetupImages();
            SelectCategory();
        }

        private void SetupImages()
        {
            _images.Clear();
            if (!string.IsNullOrWhiteSpace(_currentProduct.Image))
            {
                _images.AddRange(_currentProduct.Image.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }

            _currentIndex = 0;
            RefreshImage();
        }

        private void RefreshImage()
        {
            if (_images.Count == 0)
            {
                imgProduct.Source = null;
                return;
            }

            string imageFileName = _images[_currentIndex];
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", imageFileName);

            if (File.Exists(fullPath))
            {
                imgProduct.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            }
            else
            {
                imgProduct.Source = null;
            }
        }

        private void SelectCategory()
        {
            cbCategory.SelectedItem = AppConnect.model01.Categories.FirstOrDefault(c => c.ID == _currentProduct.CategoryID);
        }

        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp",
                Multiselect = true
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            string targetDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images");
            Directory.CreateDirectory(targetDir);

            foreach (var file in dialog.FileNames)
            {
                string uniqueName = $"{Guid.NewGuid():N}{Path.GetExtension(file)}";
                string targetPath = Path.Combine(targetDir, uniqueName);
                File.Copy(file, targetPath, true);
                _images.Add(uniqueName);
            }

            _currentProduct.Image = string.Join(";", _images);
            _currentIndex = _images.Count - 1;
            RefreshImage();
        }

        private void btnPrevImage_Click(object sender, RoutedEventArgs e)
        {
            if (_images.Count == 0)
            {
                return;
            }

            _currentIndex = (_currentIndex - 1 + _images.Count) % _images.Count;
            RefreshImage();
        }

        private void btnNextImage_Click(object sender, RoutedEventArgs e)
        {
            if (_images.Count == 0)
            {
                return;
            }

            _currentIndex = (_currentIndex + 1) % _images.Count;
            RefreshImage();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentProduct.Name))
            {
                MessageBox.Show("Название товара не может быть пустым.");
                return;
            }

            if (!(cbCategory.SelectedItem is Category category))
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }

            var dbCategory = AppConnect.model01.Categories.FirstOrDefault(c => c.ID == category.ID);
            if (dbCategory == null)
            {
                MessageBox.Show("Категория не найдена в базе данных.");
                return;
            }

            _currentProduct.CategoryID = dbCategory.ID;
            _currentProduct.Image = string.Join(";", _images);

            if (_productId == 0)
            {
                AppConnect.model01.Products.Add(_currentProduct);
            }

            AppConnect.model01.SaveChanges();
            AppFrame.frmMain.Navigate(new PageTask());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new PageTask());
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Событие оставлено для соответствия требованиям.
        }
    }
}
