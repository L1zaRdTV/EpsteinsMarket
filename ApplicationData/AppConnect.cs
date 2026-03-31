using System;
using System.Configuration;
using EpsteinsMarket.Models;

namespace EpsteinsMarket.ApplicationData
{
    public static class AppConnect
    {
        private static model01Entities _model01;

        public static model01Entities model01 => _model01 ?? (_model01 = CreateConnection());

        private static model01Entities CreateConnection()
        {
            string externalConnection = Environment.GetEnvironmentVariable("EPSTEINSMARKET_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(externalConnection))
            {
                return new model01Entities(externalConnection);
            }

            string configuredName = ConfigurationManager.AppSettings["ActiveConnectionStringName"];
            if (string.IsNullOrWhiteSpace(configuredName))
            {
                configuredName = "model01Entities";
            }

            return new model01Entities($"name={configuredName}");
        }

        public static void ResetContext()
        {
            _model01?.Dispose();
            _model01 = CreateConnection();
        }

        public static bool TryReconnect(out string error)
        {
            try
            {
                ResetContext();
                _model01.Database.Connection.Open();
                _model01.Database.Connection.Close();

                error = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public static bool TryExecute(Action action, out string error)
        {
            try
            {
                action();
                error = string.Empty;
                return true;
            }
            catch
            {
                if (TryReconnect(out _))
                {
                    try
                    {
                        action();
                        error = string.Empty;
                        return true;
                    }
                    catch (Exception retryEx)
                    {
                        error = retryEx.Message;
                        return false;
                    }
                }

                error = "Не удалось восстановить подключение к базе данных.";
                return false;
            }
        }
    }
}
