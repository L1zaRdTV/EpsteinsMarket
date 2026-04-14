using EpsteinMarket.ApplicationData;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Microsoft.VisualBasic;
namespace EpsteinMarket.Pages
{
    public partial class AddEditProductPage : Page
    {
        private Products currentProduct;

        public AddEditProductPage(Products product)
        {
            InitializeComponent();

            LoadComboBoxes();

            if (product == null)
            {
                currentProduct = new Products();
                tbTitle.Text = "Добавление товара";
            }
            else
            {
                currentProduct = product;
                tbTitle.Text = "Редактирование товара";

                txtName.Text = currentProduct.ProductName;
                txtDescription.Text = currentProduct.Description;
                txtPrice.Text = currentProduct.Price.ToString();
                txtQuantity.Text = currentProduct.QuantityInStock.ToString();

                cmbCategory.SelectedValue = currentProduct.CategoryID;
                cmbBrand.SelectedValue = currentProduct.BrandID;
                cmbStatus.SelectedValue = currentProduct.StatusID;
                txtImage.Text = currentProduct.MainImage;
                LoadPreviewImage(currentProduct.MainImage);
                LoadAdditionalImages();
            }
        }

        private void LoadPreviewImage(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
                return;

            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", imageName);

            if (File.Exists(imagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgPreview.Source = bitmap;
            }
        }

        private void LoadComboBoxes()
        {
            cmbCategory.ItemsSource = AppConnect.model01.Categories.ToList();
            cmbCategory.DisplayMemberPath = "CategoryName";
            cmbCategory.SelectedValuePath = "CategoryID";

            cmbBrand.ItemsSource = AppConnect.model01.Brands.ToList();
            cmbBrand.DisplayMemberPath = "BrandName";
            cmbBrand.SelectedValuePath = "BrandID";

            cmbStatus.ItemsSource = AppConnect.model01.ProductStatuses.ToList();
            cmbStatus.DisplayMemberPath = "StatusName";
            cmbStatus.SelectedValuePath = "StatusID";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара");
                txtName.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Цена введена неверно");
                txtPrice.Focus();
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Количество введено неверно");
                txtQuantity.Focus();
                return;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            if (cmbBrand.SelectedValue == null)
            {
                MessageBox.Show("Выберите бренд");
                return;
            }

            if (cmbStatus.SelectedValue == null)
            {
                MessageBox.Show("Выберите статус");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtImage.Text))
            {
                MessageBox.Show("Выберите изображение товара");
                return;
            }

            currentProduct.ProductName = txtName.Text;
            currentProduct.Description = txtDescription.Text;
            currentProduct.Price = price;
            currentProduct.QuantityInStock = quantity;
            currentProduct.CategoryID = (int)cmbCategory.SelectedValue;
            currentProduct.BrandID = (int)cmbBrand.SelectedValue;
            currentProduct.StatusID = (int)cmbStatus.SelectedValue;
            currentProduct.MainImage = txtImage.Text;

            try
            {
                if (currentProduct.ProductID == 0)
                {
                    AppConnect.model01.Products.Add(currentProduct);
                }


                var oldImages = AppConnect.model01.ProductImages
     .Where(x => x.ProductID == currentProduct.ProductID)
     .ToList();

                foreach (var oldImage in oldImages)
                {
                    AppConnect.model01.ProductImages.Remove(oldImage);
                }

                foreach (var imageName in additionalImages)
                {
                    ProductImages newImage = new ProductImages();
                    newImage.ProductID = currentProduct.ProductID;
                    newImage.ImagePath = imageName;

                    AppConnect.model01.ProductImages.Add(newImage);
                }

                AppConnect.model01.SaveChanges();
                MessageBox.Show("Товар сохранен");
                AppFrame.frmMain.GoBack();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var entityErrors in ex.EntityValidationErrors)
                {
                    sb.AppendLine("Сущность: " + entityErrors.Entry.Entity.GetType().Name);

                    foreach (var validationError in entityErrors.ValidationErrors)
                    {
                        sb.AppendLine("Поле: " + validationError.PropertyName);
                        sb.AppendLine("Ошибка: " + validationError.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                MessageBox.Show(sb.ToString(), "Ошибка валидации");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }
        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string fileName = Path.GetFileName(dialog.FileName);
                string destinationPath = Path.Combine(imagesFolder, fileName);

                File.Copy(dialog.FileName, destinationPath, true);

                txtImage.Text = fileName;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(destinationPath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgPreview.Source = bitmap;
            }
        }
        private void LoadAdditionalImages()
        {
            additionalImages = AppConnect.model01.ProductImages
                .Where(x => x.ProductID == currentProduct.ProductID)
                .Select(x => x.ImagePath)
                .ToList();

            lbAdditionalImages.ItemsSource = null;
            lbAdditionalImages.ItemsSource = additionalImages;
        }
        private void btnAddAdditionalImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string fileName = Path.GetFileName(dialog.FileName);
                string destinationPath = Path.Combine(imagesFolder, fileName);

                File.Copy(dialog.FileName, destinationPath, true);

                additionalImages.Add(fileName);

                lbAdditionalImages.ItemsSource = null;
                lbAdditionalImages.ItemsSource = additionalImages;
            }
        }
        private void btnRemoveAdditionalImage_Click(object sender, RoutedEventArgs e)
        {
            string selectedImage = lbAdditionalImages.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(selectedImage))
            {
                MessageBox.Show("Выберите изображение");
                return;
            }

            additionalImages.Remove(selectedImage);

            lbAdditionalImages.ItemsSource = null;
            lbAdditionalImages.ItemsSource = additionalImages;
        }
        private List<string> additionalImages = new List<string>();
        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string newCategoryName = Microsoft.VisualBasic.Interaction.InputBox (
                "Введите название новой категории:",
                "Добавление категории",
                "");

            if (string.IsNullOrWhiteSpace(newCategoryName))
                return;

            var existingCategory = AppConnect.model01.Categories
                .FirstOrDefault(x => x.CategoryName == newCategoryName);


            if (existingCategory != null)
            {
                MessageBox.Show("Такая категория уже существует");
                return;
            }
            if (string.IsNullOrWhiteSpace(newCategoryName))
                return;

            if (newCategoryName.Length > 100)
            {
                MessageBox.Show("Название слишком длинное. Максимум 100 символов.");
                return;
            }

                Categories newCategory = new Categories();
            newCategory.CategoryName = newCategoryName;

            AppConnect.model01.Categories.Add(newCategory);
            AppConnect.model01.SaveChanges();

            LoadComboBoxes();
            cmbCategory.SelectedValue = newCategory.CategoryID;

            MessageBox.Show("Категория добавлена");
        }
        private void btnAddBrand_Click(object sender, RoutedEventArgs e)
        {
            string newBrandName = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите название нового бренда:",
                "Добавление бренда",
                "");

            if (string.IsNullOrWhiteSpace(newBrandName))
                return;

            var existingBrand = AppConnect.model01.Brands
                .FirstOrDefault(x => x.BrandName == newBrandName);
            if (string.IsNullOrWhiteSpace(newBrandName))
                return;

            if (newBrandName.Length > 50)
            {
                MessageBox.Show("Название слишком длинное. Максимум 50 символов.");
                return;
            }
            if (existingBrand != null)
            {
                MessageBox.Show("Такой бренд уже существует");
                return;
            }


            Brands newBrand = new Brands();
            newBrand.BrandName = newBrandName;

            AppConnect.model01.Brands.Add(newBrand);
            AppConnect.model01.SaveChanges();

            LoadComboBoxes();
            cmbBrand.SelectedValue = newBrand.BrandID;

            MessageBox.Show("Бренд добавлен");
        }
    }
}