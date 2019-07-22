using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Encryption;

namespace PortalManagement.Models.Utilities.ConnectionString
{
    public static class ConnectionStringUtil
    {
        public static string Decrypt(string connectionString)
        {
            return string.IsNullOrEmpty(connectionString) ? string.Empty : new RijndaelEnhanced("rongclub88Key", "@1B2c3D4e5F6g7H8").Decrypt(connectionString);
        }
    }
}