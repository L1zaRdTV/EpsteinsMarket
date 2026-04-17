using System.Globalization;
using System.Text.RegularExpressions;

namespace EpsteinMarket.ApplicationData
{
    public static class InputValidationHelper
    {
        private static readonly Regex LoginRegex = new Regex(@"^[A-Za-z0-9._-]{3,50}$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex PhoneRegex = new Regex(@"^\+7\d{10}$", RegexOptions.Compiled);
        private static readonly Regex IntegerRegex = new Regex(@"^\d+$", RegexOptions.Compiled);
        private static readonly Regex DecimalRegex = new Regex(@"^\d+([\.,]\d{1,2})?$", RegexOptions.Compiled);

        public static string ValidateLogin(string login)
        {
            if (!LoginRegex.IsMatch(login))
            {
                return "Неправильный формат логина. Причина: допустимы только буквы, цифры и символы ._- (3-50 символов). Решение: например user_01.";
            }

            return null;
        }

        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            if (!EmailRegex.IsMatch(email))
            {
                return "Неправильный формат email. Причина: адрес не соответствует шаблону почты. Решение: например name@example.com.";
            }

            return null;
        }

        public static string ValidatePhone(string phone)
        {
            if (!PhoneRegex.IsMatch(phone))
            {
                return "Неправильный формат телефона. Причина: номер должен начинаться с +7 и содержать 11 цифр. Решение: попробуйте +79990009900.";
            }

            return null;
        }

        public static bool TryParsePrice(string rawPrice, out decimal price, out string errorMessage)
        {
            price = 0;
            errorMessage = null;

            if (!DecimalRegex.IsMatch(rawPrice))
            {
                errorMessage = "Неправильный формат цены. Причина: разрешены только цифры и десятичный разделитель (до 2 знаков после запятой). Решение: например 1499.90.";
                return false;
            }

            string normalized = rawPrice.Replace(',', '.');
            if (!decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out price))
            {
                errorMessage = "Цена введена неверно. Решение: введите число, например 1499.90.";
                return false;
            }

            if (price < 0)
            {
                errorMessage = "Неправильная цена. Причина: значение не может быть отрицательным. Решение: введите значение больше или равное 0.";
                return false;
            }

            return true;
        }

        public static bool TryParseQuantity(string rawQuantity, out int quantity, out string errorMessage)
        {
            quantity = 0;
            errorMessage = null;

            if (!IntegerRegex.IsMatch(rawQuantity))
            {
                errorMessage = "Неправильный формат количества. Причина: разрешены только целые числа. Решение: например 15.";
                return false;
            }

            if (!int.TryParse(rawQuantity, out quantity))
            {
                errorMessage = "Количество введено неверно. Решение: введите целое число, например 15.";
                return false;
            }

            return true;
        }

        public static bool TryParseDate(string rawDate, out System.DateTime date, out string errorMessage)
        {
            date = default(System.DateTime);
            errorMessage = null;

            if (!System.DateTime.TryParseExact(rawDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                errorMessage = "Неправильный формат даты. Причина: ожидается формат дд.ММ.гггг. Решение: например 20.10.2009.";
                return false;
            }

            return true;
        }
    }
}
