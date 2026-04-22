using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpsteinMarket.ApplicationData;

namespace EpsteinMarket.Pages
{
    public partial class OrderDetailsPage : Page
    {
        private readonly int orderId;

        public OrderDetailsPage(int orderId)
        {
            InitializeComponent();
            this.orderId = orderId;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            var order = AppConnect.model01.Orders
                .FirstOrDefault(x => x.OrderID == orderId);

            if (order == null)
            {
                MessageBox.Show("Заказ не найден");
                AppFrame.frmMain.GoBack();
                return;
            }

            tbOrderNumber.Text = "Заказ #" + order.OrderID;
            tbOrderDate.Text = "Дата: " + order.OrderDate.ToString("dd.MM.yyyy HH:mm");
            tbOrderStatus.Text = "Статус: " + order.OrderStatuses.StatusName;
            tbOrderTotal.Text = "Итог: " + order.TotalAmount + " ₽";

            var orderItems = AppConnect.model01.OrderItems
                .Where(x => x.OrderID == order.OrderID)
                .Select(x => new OrderItemDisplay
                {
                    ProductName = x.Products.ProductName,
                    Quantity = x.Quantity,
                    PriceAtMoment = x.PriceAtMoment,
                    TotalPrice = x.Quantity * x.PriceAtMoment
                })
                .ToList();

            lvOrderItems.ItemsSource = orderItems;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }
    }

    public class OrderItemDisplay
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtMoment { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
