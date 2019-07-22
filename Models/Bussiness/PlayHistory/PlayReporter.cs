using ExcelLibrary.SpreadSheet;
using PortalManagement.Models.Utilities.ConnectionString;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Utilities.Database;

namespace PortalManagement.Models.Bussiness.PlayHistory
{
    public class PlayReporter
    {
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        Workbook _workbook;

        public PlayReporter(long accountId)
        {
            _workbook = new Workbook();
            _workbook.Worksheets.Clear(); // remove all worksheets
            GetData(accountId);
        }

        public void AppendToStream(MemoryStream stream)
        {
            _workbook.SaveToStream(stream);
        }

        private void CreateSheet(string sheetname, DataTable dt)
        {
            var workSheet = new Worksheet(sheetname);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                // Add column header
                workSheet.Cells[0, i] = new Cell(dt.Columns[i].ColumnName);

                // Populate row data
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    workSheet.Cells[j + 1, i] = new Cell(DBNull.Value.Equals(dt.Rows[j][i]) ? String.Empty : Convert.ToString(dt.Rows[j][i]));
                }
            }
            _workbook.Worksheets.Add(workSheet);
        }

        private void GetData(long accountId)
        {
            DBHelper helper = null;
            helper = new DBHelper(_conStrPortalDbRoot);

            DataTable dt;
            var query = $"SELECT * FROM (SELECT * FROM [GamePortal].[log].[GameGoldTransactionFull] WHERE AccountId = {accountId}";
            query += $" UNION SELECT * FROM [log].[V_GameGoldTransaction] WITH (nolock) WHERE AccountId = {accountId}";
            query += $") A ORDER BY CreatedTime DESC";
            dt = helper.GetDataTable(query);
            CreateSheet("Lịch sử chơi", dt);
        }
    }
}