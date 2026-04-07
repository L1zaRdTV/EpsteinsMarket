using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EpsteinsMarket.ApplicationData;
using EpsteinsMarket.Models;

namespace EpsteinsMarket.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            LoadPage();
        }

        private void LoadPage()
        {
            LoadProfile();
            LoadFavorites();
            LoadPurchaseHistory();
            RefreshLists();
        }

        private void LoadProfile()
        {
            if (AppSession.CurrentUser == null)
            {
                tbEditName.Text = string.Empty;
                tbEditEmail.Text = string.Empty;
                tbEditPhone.Text = string.Empty;
                return;
            }

            tbEditName.Text = AppSession.CurrentUser.FullName ?? string.Empty;
            tbEditEmail.Text = AppSession.CurrentUser.Email ?? string.Empty;
            tbEditPhone.Text = AppSession.CurrentUser.Phone ?? string.Empty;
        }

        private void LoadFavorites()
        {
            if (AppSession.CurrentUser == null)
            {
                lbFavorites.ItemsSource = null;
                return;
            }

            try
            {
                List<Product> favorites = AppConnect.model01.Favorites
                    .Where(f => f.UserID == AppSession.CurrentUser.UserID)
                    .Join(AppConnect.model01.Products, f => f.ProductID, p => p.ID, (f, p) => p)
                    .OrderBy(p => p.Name)
                    .ToList();

                lbFavorites.ItemsSource = favorites;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки избранного: {ex.Message}");
            }
        }

        private void LoadPurchaseHistory()
        {
            AppSession.PurchasedProducts.Clear();
            if (AppSession.CurrentUser == null)
            {
                return;
            }

            try
            {
                List<int> purchasedProductIds = AppConnect.model01.OrderItems
                    .Join(AppConnect.model01.Orders,
                        oi => oi.OrderID,
                        o => o.ID,
                        (oi, o) => new { oi.ProductID, o.UserID })
                    .Where(x => x.UserID == AppSession.CurrentUser.UserID)
                    .Select(x => x.ProductID)
                    .Distinct()
                    .ToList();

                if (!purchasedProductIds.Any())
                {
                    return;
                }

                List<Product> purchasedProducts = AppConnect.model01.Products
                    .Where(p => purchasedProductIds.Contains(p.ID))
                    .OrderBy(p => p.Name)
                    .ToList();

                AppSession.PurchasedProducts.AddRange(purchasedProducts);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории покупок: {ex.Message}");
            }
        }

        private void RefreshLists()
        {
            lbCart.ItemsSource = AppSession.CartProducts;
            lbCart.Items.Refresh();

            lbPurchases.ItemsSource = AppSession.PurchasedProducts;
            lbPurchases.Items.Refresh();
        }

        private int? EnsurePrimaryAddressId(int userId)
        {
            UserAddress primaryAddress = AppConnect.model01.UserAddresses
                .FirstOrDefault(a => a.UserID == userId && a.IsPrimary);
            if (primaryAddress != null)
            {
                return primaryAddress.ID;
            }

            UserAddress fallbackAddress = AppConnect.model01.UserAddresses
                .FirstOrDefault(a => a.UserID == userId);
            return fallbackAddress?.ID;
        }

        private void CreateOrderAndPayment(List<Product> products, string paymentMethod)
        {
            decimal totalAmount = products.Sum(p => p.Price ?? 0m);

            Order order = new Order
            {
                UserID = AppSession.CurrentUser.UserID,
                AddressID = EnsurePrimaryAddressId(AppSession.CurrentUser.UserID),
                OrderDate = DateTime.Now,
                Status = "Создан",
                TotalAmount = totalAmount
            };

            AppConnect.model01.Orders.Add(order);
            AppConnect.model01.SaveChanges();

            List<OrderItem> orderItems = products.Select(product => new OrderItem
            {
                OrderID = order.ID,
                ProductID = product.ID,
                Quantity = 1,
                UnitPrice = product.Price ?? 0m
            }).ToList();
            AppConnect.model01.OrderItems.AddRange(orderItems);
            UpdateInventoryBalances(orderItems);

            AppConnect.model01.PaymentTransactions.Add(new PaymentTransaction
            {
                OrderID = order.ID,
                PaymentMethod = paymentMethod,
                Amount = totalAmount,
                PaymentStatus = "Оплачен",
                PaidAt = DateTime.Now
            });

            DeliveryMethod deliveryMethod = ResolveDeliveryMethod();
            AppConnect.model01.Shipments.Add(new Shipment
            {
                OrderID = order.ID,
                DeliveryMethodID = deliveryMethod.ID,
                TrackingNumber = $"AUTO-{order.ID:000000}",
                ShipmentStatus = "Создана"
            });

            AppConnect.model01.SaveChanges();
        }

        private DeliveryMethod ResolveDeliveryMethod()
        {
            DeliveryMethod pickupMethod = AppConnect.model01.DeliveryMethods
                .FirstOrDefault(dm => dm.MethodName == "Пункт выдачи");
            if (pickupMethod != null)
            {
                return pickupMethod;
            }

            DeliveryMethod firstMethod = AppConnect.model01.DeliveryMethods
                .OrderBy(dm => dm.ID)
                .FirstOrDefault();
            if (firstMethod != null)
            {
                return firstMethod;
            }

            throw new InvalidOperationException("В базе нет способов доставки. Проверьте заполнение таблицы DeliveryMethods.");
        }

        private void UpdateInventoryBalances(List<OrderItem> orderItems)
        {
            foreach (OrderItem item in orderItems)
            {
                InventoryBalance stock = AppConnect.model01.InventoryBalances
                    .Where(i => i.ProductID == item.ProductID)
                    .OrderByDescending(i => i.Quantity)
                    .ThenBy(i => i.ID)
                    .FirstOrDefault();

                if (stock == null)
                {
                    throw new InvalidOperationException($"Для товара ID={item.ProductID} не найден остаток на складе.");
                }

                if (stock.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Недостаточно остатка для товара ID={item.ProductID}. Доступно: {stock.Quantity}.");
                }

                stock.Quantity -= item.Quantity;
                stock.UpdatedAt = DateTime.Now;
            }
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (AppSession.CurrentUser == null)
            {
                MessageBox.Show("Сначала выполните вход.");
                return;
            }

            if (!AppSession.CartProducts.Any())
            {
                MessageBox.Show("Корзина пока пуста.");
                return;
            }

            try
            {
                CreateOrderAndPayment(AppSession.CartProducts.ToList(), "СБП");
                AppSession.CartProducts.Clear();
                LoadPurchaseHistory();
                RefreshLists();
                MessageBox.Show("Покупка товаров из корзины завершена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка оформления заказа: {ex.Message}");
            }
        }

        private void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            if (AppSession.CurrentUser == null)
            {
                MessageBox.Show("Сначала выполните вход.");
                return;
            }

            AppSession.CurrentUser.FullName = tbEditName.Text.Trim();
            AppSession.CurrentUser.Email = tbEditEmail.Text.Trim();
            AppSession.CurrentUser.Phone = tbEditPhone.Text.Trim();

            try
            {
                AppConnect.model01.SaveChanges();
                LoadProfile();
                    MessageBox.Show("Данные профиля сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения профиля: {ex.Message}");
            }
        }

        private void btnBackToCatalog_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frmMain.Navigate(new PageTask());
        }
    }
}
