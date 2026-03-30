using System;
using EpsteinsMarket.Models;

namespace EpsteinsMarket.ApplicationData
{
    public static class AppConnect
    {
        private static model01Entities _model01;

        public static model01Entities model01 => _model01 ?? (_model01 = CreateConnection());

        private static model01Entities CreateConnection()
        {
            return new model01Entities();
        }

        public static bool TryReconnect(out string error)
        {
            try
            {
                _model01?.Dispose();
                _model01 = CreateConnection();

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
    }
}
