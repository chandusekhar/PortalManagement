using ExcelLibrary.SpreadSheet;
using PortalManagement.Models.DataAccess.Agency;
using PortalManagement.Models.DataAccess.Transaction;
using PortalManagement.Models.DataAccess.UserInfo;
using PortalManagement.Models.Utilities.ConnectionString;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using Utilities.Database;

namespace PortalManagement.Models.Bussiness.Transaction
{
    public class Reporter
    {
        private static readonly string _conStrPortalDbRoot = ConnectionStringUtil.Decrypt(ConfigurationManager.ConnectionStrings["portal_db_root"]?.ToString());

        Workbook _workbook;
        int _start;
        int _end;
        int _type;
        int _id;

        public Reporter(int id, int start, int end, int type)
        {
            _id = id;
            _start = start;
            _end = end;
            _type = type;
            _workbook = new Workbook();
            _workbook.Worksheets.Clear(); // remove all worksheets
            GetData();
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

        private void GetData()
        {
            DBHelper helper = null;
            helper = new DBHelper(_conStrPortalDbRoot);

            DataTable dt;
            long accID = 0;
            var query = "";

            if (_id > 0)
            {
                var account = AgencyDAO.GetById(_id);
                if (UserContext.UserInfo.GroupID == 0)
                {
                    if (account != null)
                        if (account.IsAgency == true)
                            accID = account.ID;
                }
                else
                {
                    if (account != null)
                    {
                        if (account.IsAgency == true)
                        {
                            var agencyInfo = TransactionDAO.GetAgencyInfo(account.GameAccountId);
                            if (agencyInfo != null && agencyInfo.Creator == UserContext.UserInfo.AccountID)
                                accID = account.GameAccountId;
                        }
                    }

                }
            }
            else
            {
                var agency = AgencyDAO.GetById(UserContext.UserInfo.AccountID);
                accID = agency.GameAccountId;
            }

            if (_type == 0)
                query = $"select top 1000 * from ag.[Transaction] where {accID} in (SenderID, RecvID) and CreatedTimeInt >= {_start} and CreatedTimeInt <= {_end} order by id desc";
            else if (_type == 1)
                query = $"select top 1000 * from ag.[Transaction] where RecvID = {accID} and CreatedTimeInt >= {_start} and CreatedTimeInt <= {_end} order by id desc";
            else
                query = $"select top 1000 * from ag.[Transaction] where SenderID = {accID} and CreatedTimeInt >= {_start} and CreatedTimeInt <= {_end} order by id desc";
            dt = helper.GetDataTable(query);
            CreateSheet("Lịch sử giao dịch", dt);
        }
    }
}