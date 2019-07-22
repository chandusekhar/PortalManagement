using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using PortalManagement.Models.Bussiness.Giftcode;
using PortalManagement.Models.Utilities.ConnectionString;
using Utilities.Database;
using Utilities.Log;

namespace PortalManagement.Models.DataAccess.Giftcode
{
    public class GCDAO
    {
        private static readonly string _conStr = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["cms_db"]?.ToString());
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        public static int CreateEvent(string name, long price, int creator, int type, int? agencyId)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                var response = connection.QueryFirst($"insert into dbo.EventGiftcode (Name, Created, Price, Type, AgencyId) VALUES (N'{name}', {creator}, {price}, {type}, {agencyId}); select @@IDENTITY ID");
                return Convert.ToInt32(response.ID);
            }
        }

        public static void BuildGiftcode(string query)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                connection.Execute(query);
            }
        }

        public static dynamic GetAllEvent()
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"select * from dbo.EventGiftcode order by CreatedTime desc");
            }
        }

        public static dynamic GetAllEventStatus()
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query("SELECT count(1) Total ,[Status] ,[EventID] FROM [dbo].[Giftcode] group by [Status], [EventID]");
            }
        }

        public static dynamic GetGiftCode(int id, int type, int skip, int itemPerpage)
        {
            string more = string.Empty;
            if (type == 2)
                more = "and Status = 1";
            else if (type == 3)
                more = "and Status = 0";

            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"select * from [dbo].[Giftcode] where EventID = {id} {more} order by createdtime desc offset {skip} rows fetch next {itemPerpage} rows only");
            }
        }

        public static List<GiftcodeModel> Search(string code)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot); ;
            var query = $"SELECT * FROM [dbo].[Giftcode] WHERE Code LIKE '%{code}%'";
            return helper.GetList<GiftcodeModel>(query);
        }

        public static bool DeleteEvent(int id)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                var response = connection.QueryFirst($"DELETE FROM dbo.EventGiftcode WHERE ID = {id}" +
                    $" DELETE FROM dbo.Giftcode WHERE EventID = {id}" +
                    $" SELECT @@ROWCOUNT Response");
                return Convert.ToInt32(response.Response) > 0;
            }
        }

        public static bool ExtendGC(int id, string newDt)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                var response = connection.QueryFirst($"UPDATE [dbo].[Giftcode] SET Expired = '{newDt}' WHERE EventID = {id}" +
                    $" SELECT @@ROWCOUNT Response");
                return Convert.ToInt32(response.Response) > 0;
            }
        }

        public static dynamic GetEvents(int type, int skip, int itemPerPage)
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"select * from dbo.EventGiftcode WHERE Type = {type} order by CreatedTime desc offset {skip} rows fetch next {itemPerPage} rows only");
            }
        }

        public static dynamic GCStatisticAgency(DateTime start, DateTime end)
        {
            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                return connection.Query($"exec [dbo].[SP_GCStatisticAgency] @_From = '{start}', @_To = '{end}'");
            }
        }

        public static dynamic GetGCSummary()
        {
            using (SqlConnection connection = new SqlConnection(_conStrPortalDbRoot))
            {
                return connection.Query($"SELECT A.Type, COUNT(B.Code) Count, SUM(B.Gold) TotalAmount FROM [dbo].[EventGiftcode] A " +
                    $"INNER JOIN [dbo].[Giftcode] B ON A.ID = B.EventID " +
                    $"GROUP BY A.Type");
            }
        }

        public static List<StatisticGC> GetStatisticGC(DateTime start, DateTime end)
        {
            DBHelper helper = new DBHelper(_conStrPortalDbRoot); ;
            var query = $"SELECT A.Type, Convert(Date, A.CreatedTime) Date, B.Status, CAST(Convert(varchar(8),B.Expired,112) as int) Expired FROM [GamePortal].[dbo].[EventGiftcode] A " +
                $"INNER JOIN [GamePortal].[dbo].[Giftcode] B ON A.ID = B.EventID " +
                $"WHERE A.CreatedTime >= '{start}' AND A.CreatedTime <= '{end}'";
            return helper.GetList<StatisticGC>(query);
        }
    }
}