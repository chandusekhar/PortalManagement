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

namespace PortalManagement.Models.Bussiness.Giftcode
{
    public class GCReporter
    {
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        Workbook _workbook;
        
        public GCReporter(int eventId)
        {
            _workbook = new Workbook();
            _workbook.Worksheets.Clear(); // remove all worksheets
            GetData(eventId);
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

        private void GetData(int eventId)
        {
            DBHelper helper = null;
            helper = new DBHelper(_conStrPortalDbRoot);

            DataTable dt;
            var query = $"SELECT * FROM [dbo].[Giftcode] WHERE EventID = {eventId}";
            dt = helper.GetDataTable(query);
            CreateSheet("Danh sách giftcode", dt);
        }
    }
}