using System.Collections.Generic;
using EpsteinsMarket.Models;

namespace EpsteinsMarket.ApplicationData
{
    public static class AppSession
    {
        public static User CurrentUser { get; set; }

        public static List<Product> CartProducts { get; } = new List<Product>();

        public static List<Product> PurchasedProducts { get; } = new List<Product>();

        public static bool IsAdmin => CurrentUser != null
            && string.Equals(CurrentUser.Role, "Администратор", System.StringComparison.OrdinalIgnoreCase);
    }
}
