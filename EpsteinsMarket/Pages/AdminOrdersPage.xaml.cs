using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpsteinMarket.ApplicationData;

namespace EpsteinMarket.Pages
{
    public partial class AdminOrdersPage : Page
    {
        public AdminOrdersPage()
        {
            InitializeComponent();

            if (AppConnect.CurrentUser == null || AppConnect.CurrentUser.RoleID != 1)
            {
                MessageBox.Show("Доступ только для администратора");
                AppFrame.frmMain.Navigate(new CatalogPage());
                return;
            }

            LoadOrders();
        }

        private void LoadOrders()
        {
            var orderList = AppConnect.model01.Orders
                .OrderByDescending(x => x.OrderDate)
                .ToList()
                .Select(x => new AdminOrderDisplayItem
                {
                    SourceOrder = x,
                    OrderID = x.OrderID,
                    UserFullName = x.Users.FullName,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    StatusName = x.OrderStatuses.StatusName
                })
                .ToList();

            lvOrders.ItemsSource = orderList;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.GoBack();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void btnOpenDetails_Click(object sender, RoutedEventArgs e)
        {
            AdminOrderDisplayItem selectedOrder = lvOrders.SelectedItem as AdminOrderDisplayItem;

            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            AppFrame.frmMain.Navigate(new OrderDetailsPage(selectedOrder.OrderID));
        }

        private void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            AdminOrderDisplayItem selectedOrder = lvOrders.SelectedItem as AdminOrderDisplayItem;

            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            string currentStatus = (selectedOrder.StatusName ?? string.Empty).ToLower();
            if (currentStatus.Contains("отмен"))
            {
                MessageBox.Show("Этот заказ уже отменен");
                return;
            }

            var canceledStatus = AppConnect.model01.OrderStatuses
                .FirstOrDefault(x => x.StatusName.ToLower().Contains("отмен"));

            if (canceledStatus == null)
            {
                MessageBox.Show("В базе не найден статус отмены заказа");
                return;
            }

            MessageBoxResult confirmResult = MessageBox.Show(
                "Отменить заказ #" + selectedOrder.OrderID + "?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmResult != MessageBoxResult.Yes)
            {
                return;
            }

            selectedOrder.SourceOrder.OrderStatusID = canceledStatus.OrderStatusID;
            AppConnect.model01.SaveChanges();

            MessageBox.Show("Заказ отменен");
            LoadOrders();
        }
    }

    public class AdminOrderDisplayItem
    {
        public Orders SourceOrder { get; set; }
        public int OrderID { get; set; }
        public string UserFullName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string StatusName { get; set; }
    }
}
