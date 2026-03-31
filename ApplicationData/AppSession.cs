using EpsteinsMarket.Models;

namespace EpsteinsMarket.ApplicationData
{
    public static class AppSession
    {
        public static User CurrentUser { get; set; }

        public static bool IsAdmin => CurrentUser != null
            && string.Equals(CurrentUser.Role, "Администратор", System.StringComparison.OrdinalIgnoreCase);
    }
}
