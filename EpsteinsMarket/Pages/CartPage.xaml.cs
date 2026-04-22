using System.IO;
using EpsteinMarket.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Text;
using QRCoder;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace EpsteinMarket.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        private const string StoreName = "EpsteinsMarket";

        public CartPage()
        {
            InitializeComponent();
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            var userCart = AppConnect.model01.Carts
                .FirstOrDefault(x => x.UserID == AppConnect.CurrentUser.UserID);

            if (userCart == null)
            {
                MessageBox.Show("Корзина пользователя не найдена");
                return;
            }

            var cartItems = AppConnect.model01.CartItems
                .Where(x => x.CartID == userCart.CartID)
                .ToList();

            List<CartDisplayItem> displayItems = new List<CartDisplayItem>();

            foreach (var item in cartItems)
            {
                displayItems.Add(new CartDisplayItem
                {
                    CartItemID = item.CartItemID,
                    ProductID = item.ProductID,
                    ProductName = item.Products.ProductName,
                    Quantity = item.Quantity,
                    PriceAtMoment = item.PriceAtMoment,
                    TotalPrice = item.Quantity * item.PriceAtMoment,
                    SourceCartItem = item
                });
            }

            lvCartItems.ItemsSource = displayItems;

            decimal total = displayItems.Sum(x => x.TotalPrice);
            tbTotal.Text = "Итог: " + total + " ₽";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            CartDisplayItem selectedItem = button.Tag as CartDisplayItem;

            if (selectedItem == null)
                return;

            AppConnect.model01.CartItems.Remove(selectedItem.SourceCartItem);
            AppConnect.model01.SaveChanges();

            MessageBox.Show("Товар удален из корзины");
            LoadCartItems();
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            var userCart = AppConnect.model01.Carts
                .FirstOrDefault(x => x.UserID == AppConnect.CurrentUser.UserID);

            if (userCart == null)
            {
                MessageBox.Show("Корзина не найдена");
                return;
            }

            var cartItems = AppConnect.model01.CartItems
                .Where(x => x.CartID == userCart.CartID)
                .ToList();

            if (cartItems.Count == 0)
            {
                MessageBox.Show("Корзина пуста");
                return;
            }

            decimal totalAmount = cartItems.Sum(x => x.Quantity * x.PriceAtMoment);

            Orders newOrder = new Orders();
            newOrder.UserID = AppConnect.CurrentUser.UserID;
            newOrder.OrderDate = System.DateTime.Now;
            newOrder.TotalAmount = totalAmount;
            newOrder.OrderStatusID = 1;

            AppConnect.model01.Orders.Add(newOrder);
            AppConnect.model01.SaveChanges();

            foreach (var item in cartItems)
            {
                OrderItems newOrderItem = new OrderItems();
                newOrderItem.OrderID = newOrder.OrderID;
                newOrderItem.ProductID = item.ProductID;
                newOrderItem.Quantity = item.Quantity;
                newOrderItem.PriceAtMoment = item.PriceAtMoment;

                AppConnect.model01.OrderItems.Add(newOrderItem);
            }
            string pdfPath = GenerateReceiptPdf(newOrder, cartItems);

            Receipts newReceipt = new Receipts();
            newReceipt.OrderID = newOrder.OrderID;
            newReceipt.PdfPath = pdfPath;
            newReceipt.CreatedAt = System.DateTime.Now;

            AppConnect.model01.Receipts.Add(newReceipt);
            foreach (var item in cartItems)
            {
                AppConnect.model01.CartItems.Remove(item);
            }

            AppConnect.model01.SaveChanges();

            OpenReceiptPdf(pdfPath);
            MessageBox.Show("Заказ успешно оформлен. Чек открыт автоматически.");
            LoadCartItems();
        }

        private void OpenReceiptPdf(string pdfPath)
        {
            if (!File.Exists(pdfPath))
            {
                MessageBox.Show("Файл чека не найден");
                return;
            }

            Process.Start(pdfPath);
        }

        private string GenerateReceiptPdf(Orders order, List<CartItems> cartItems)
        {
            string receiptsFolder = System.IO.Path.Combine(
                Directory.GetCurrentDirectory(),
                "Receipts");

            if (!Directory.Exists(receiptsFolder))
            {
                Directory.CreateDirectory(receiptsFolder);
            }

            string fileName = "receipt_" + order.OrderID + ".pdf";
            string fullPath = System.IO.Path.Combine(receiptsFolder, fileName);

            string fontPath = @"C:\Windows\Fonts\arial.ttf";

            BaseFont baseFont =
                BaseFont.CreateFont(
                    fontPath,
                    BaseFont.IDENTITY_H,
                    BaseFont.EMBEDDED);

            Font normalFont = new Font(baseFont, 12);
            Font boldFont = new Font(baseFont, 14, Font.BOLD);

            Document document = new Document();
            PdfWriter.GetInstance(
                document,
                new FileStream(fullPath, FileMode.Create));

            document.Open();

            Action<string, Font> addLine = (textLine, font) =>
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Add(new Chunk(textLine, font));
                document.Add(paragraph);
            };

            addLine("Чек магазина " + StoreName, boldFont);
            addLine(" ", normalFont);
            addLine("Номер заказа: " + order.OrderID, normalFont);
            addLine("Дата заказа: " + order.OrderDate.ToString("dd.MM.yyyy HH:mm"), normalFont);
            addLine("Покупатель: " + AppConnect.CurrentUser.FullName, normalFont);
            addLine("Логин: " + AppConnect.CurrentUser.Login, normalFont);
            addLine(" ", normalFont);

            addLine("Состав заказа:", boldFont);
            addLine(" ", normalFont);

            foreach (var item in cartItems)
            {
                string line =
                    item.Products.ProductName +
                    " | Кол-во: " + item.Quantity +
                    " | Цена: " + item.PriceAtMoment +
                    " | Сумма: " + (item.Quantity * item.PriceAtMoment);

                addLine(line, normalFont);
            }

            addLine(" ", normalFont);
            addLine("Итоговая сумма: " + order.TotalAmount + " ₽", boldFont);
            addLine(" ", normalFont);

            string qrOrderDetails = BuildOrderQrContent(order, cartItems);
            AddQrCodeToDocument(document, qrOrderDetails);
            addLine("Спасибо за покупку в " + StoreName + "!", normalFont);

            document.Close();

            return fullPath;
        }


        private string BuildOrderQrContent(Orders order, List<CartItems> cartItems)
        {
            StringBuilder qrBuilder = new StringBuilder();
            qrBuilder.AppendLine("Магазин: " + StoreName);
            qrBuilder.AppendLine("Детали заказа");
            qrBuilder.AppendLine("Номер заказа: " + order.OrderID);
            qrBuilder.AppendLine("Дата заказа: " + order.OrderDate.ToString("dd.MM.yyyy HH:mm"));
            qrBuilder.AppendLine("Покупатель: " + AppConnect.CurrentUser.FullName);
            qrBuilder.AppendLine("Логин: " + AppConnect.CurrentUser.Login);
            qrBuilder.AppendLine("Состав:");

            foreach (var item in cartItems)
            {
                qrBuilder.AppendLine(
                    item.Products.ProductName +
                    " x" + item.Quantity +
                    " = " + (item.Quantity * item.PriceAtMoment) + " ₽");
            }

            qrBuilder.AppendLine("Итог: " + order.TotalAmount + " ₽");

            return qrBuilder.ToString();
        }

        private void AddQrCodeToDocument(Document document, string qrContent)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);

                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qrCode.GetGraphic(20);

                iTextSharp.text.Image qrImage = iTextSharp.text.Image.GetInstance(qrCodeBytes);
                qrImage.ScaleAbsolute(140f, 140f);
                qrImage.Alignment = Element.ALIGN_LEFT;
                document.Add(qrImage);
            }
        }
    }


        public class CartDisplayItem
    {
        public int CartItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtMoment { get; set; }
        public decimal TotalPrice { get; set; }
        public CartItems SourceCartItem { get; set; }
    }
}
