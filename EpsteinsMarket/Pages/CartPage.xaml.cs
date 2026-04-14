using System.IO;
using EpsteinMarket.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace EpsteinMarket.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
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

            MessageBox.Show("Заказ успешно оформлен");
            LoadCartItems();
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

            iTextSharp.text.pdf.BaseFont baseFont =
                iTextSharp.text.pdf.BaseFont.CreateFont(
                    fontPath,
                    iTextSharp.text.pdf.BaseFont.IDENTITY_H,
                    iTextSharp.text.pdf.BaseFont.EMBEDDED);

            iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 12);
            iTextSharp.text.Font boldFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);

            iTextSharp.text.Document document = new iTextSharp.text.Document();
            iTextSharp.text.pdf.PdfWriter.GetInstance(
                document,
                new FileStream(fullPath, FileMode.Create));

            document.Open();

            document.Add(new iTextSharp.text.Paragraph("Чек магазина клонов", boldFont));
            document.Add(new iTextSharp.text.Paragraph(" ", normalFont));
            document.Add(new iTextSharp.text.Paragraph("Номер заказа: " + order.OrderID, normalFont));
            document.Add(new iTextSharp.text.Paragraph("Дата заказа: " + order.OrderDate.ToString("dd.MM.yyyy HH:mm"), normalFont));
            document.Add(new iTextSharp.text.Paragraph("Покупатель: " + AppConnect.CurrentUser.FullName, normalFont));
            document.Add(new iTextSharp.text.Paragraph("Логин: " + AppConnect.CurrentUser.Login, normalFont));
            document.Add(new iTextSharp.text.Paragraph(" ", normalFont));

            document.Add(new iTextSharp.text.Paragraph("Состав заказа:", boldFont));
            document.Add(new iTextSharp.text.Paragraph(" ", normalFont));

            foreach (var item in cartItems)
            {
                string line =
                    item.Products.ProductName +
                    " | Кол-во: " + item.Quantity +
                    " | Цена: " + item.PriceAtMoment +
                    " | Сумма: " + (item.Quantity * item.PriceAtMoment);

                document.Add(new iTextSharp.text.Paragraph(line, normalFont));
            }

            document.Add(new iTextSharp.text.Paragraph(" ", normalFont));
            document.Add(new iTextSharp.text.Paragraph("Итоговая сумма: " + order.TotalAmount + " ₽", boldFont));

            document.Close();

            return fullPath;
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
