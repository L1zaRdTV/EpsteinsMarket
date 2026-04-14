using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using EpsteinMarket.ApplicationData;

namespace EpsteinMarket.Pages
{
    public partial class QrPage : Page
    {
        private TelegramLinks activeTelegramLink;

        public QrPage()
        {
            InitializeComponent();
            LoadTelegramData();
        }

        private void LoadTelegramData()
        {
            activeTelegramLink = AppConnect.model01.TelegramLinks
                .FirstOrDefault(x => x.IsActive == true);

            if (activeTelegramLink == null)
            {
                MessageBox.Show("Активная Telegram-ссылка не найдена");
                return;
            }

            tbTelegramLink.Text = activeTelegramLink.Url;

            if (!string.IsNullOrWhiteSpace(activeTelegramLink.QrImagePath))
            {
                string imagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Resources",
                    activeTelegramLink.QrImagePath);

                if (!File.Exists(imagePath))
                {
                    imagePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "Images",
                        activeTelegramLink.QrImagePath);
                }

                if (File.Exists(imagePath))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    imgQr.Source = bitmap;
                }
            }
        }

        private void btnOpenTelegram_Click(object sender, RoutedEventArgs e)
        {
            if (activeTelegramLink == null)
            {
                MessageBox.Show("Ссылка не найдена");
                return;
            }

            Process.Start(activeTelegramLink.Url);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }
    }
}