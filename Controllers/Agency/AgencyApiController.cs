using PortalManagement.Financial;
using PortalManagement.Models.Bussiness.Agency;
using PortalManagement.Models.Bussiness.Transaction;
using PortalManagement.Models.DataAccess.Agency;
using PortalManagement.Models.DataAccess.Transaction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.Agency
{
    public class AgencyApiController : ApiController
    {
        [HttpOptions, HttpGet]
        public dynamic GetAgencyInfos(int from, int to)
        {
            return AgencyDAO.GetAgencies(from, to);
        }

        [HttpOptions, HttpGet]
        public dynamic GetAllAgencies()
        {
            var level = UserContext.UserInfo.GroupID;
            if (level == 0)
                return AgencyDAO.GetAllAgencies();

            return null;
        }

        [HttpOptions, HttpGet]
        public dynamic GetAll()
        {
            try
            {
                return AgencyDAO.GetAll();
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpOptions, HttpGet]
        public dynamic Management(int month, int year)
        {
            try
            {
                DateTime start = new DateTime(year, month, 1);
                DateTime end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

                return AgencyDAO.Management(start, end);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public bool DeleteAgency(long id)
        {
            try
            {
                AgencyDAO.DeleteAgency(id);

                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public bool HideAgency(long id, int display)
        {
            try
            {
                AgencyDAO.HideAgency(id, display);

                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public bool AddAgency(string display, string tel, string fb, string tele, string address, string gameName)
        {
            try
            {
                if (string.IsNullOrEmpty(display) || string.IsNullOrEmpty(gameName) || UserContext.UserInfo.GroupID > 1)
                    return false;

                AgencyDAO.AddAgency(UserContext.UserInfo.AccountID, UserContext.UserInfo.FullName, display, Utilities.Encryption.Security.MD5Encrypt(gameName),
                    tel, fb, tele, address, gameName, UserContext.UserInfo.GroupID + 1);

                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public bool AuthorizeAgency(long gameAccountId, string gameName, string display, string tel, string fb, string tele, string address)
        {
            try
            {
                if (string.IsNullOrEmpty(display) || UserContext.UserInfo.GroupID > 1)
                    return false;

                AgencyDAO.AuthorizeAgency(gameAccountId, UserContext.UserInfo.AccountID, UserContext.UserInfo.FullName, display, Utilities.Encryption.Security.MD5Encrypt(gameName),
                   tel, fb, tele, address, gameName, UserContext.UserInfo.GroupID + 1);

                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public dynamic AgencyTransaction(int id, string start, string end, int type, int page = 0)
        {
            try
            {
                DateTime from = new DateTime(Convert.ToInt32(start.Substring(0, 4)), Convert.ToInt32(start.Substring(4, 2)), Convert.ToInt32(start.Substring(6, 2)));
                DateTime to = new DateTime(Convert.ToInt32(end.Substring(0, 4)), Convert.ToInt32(end.Substring(4, 2)), Convert.ToInt32(end.Substring(6, 2)));

                var skip = page * 50;
                long totalMoney = 0, totalFee = 0;
                if (id > 0)
                {
                    var account = AgencyDAO.GetById(id);
                    if (UserContext.UserInfo.GroupID == 0)
                    {
                        if (account != null)
                        {
                            if (account.IsAgency == true)
                            {
                                var _data = TransactionDAO.TransactionHistory(account.GameAccountId, from, to, type, skip, 50, out totalMoney, out totalFee);
                                return new
                                {
                                    TotalMoney = totalMoney,
                                    TotalFee = totalFee,
                                    Data = _data
                                };
                            }
                        }
                    }
                    else
                    {
                        if (account != null)
                        {
                            if (account.IsAgency == true)
                            {
                                var agencyInfo = TransactionDAO.GetAgencyInfo(account.GameAccountId);
                                if (agencyInfo != null && agencyInfo.Creator == UserContext.UserInfo.AccountID)
                                {
                                    var _data = TransactionDAO.TransactionHistory(account.GameAccountId, from, to, type, skip, 50, out totalMoney, out totalFee);
                                    return new
                                    {
                                        TotalMoney = totalMoney,
                                        TotalFee = totalFee,
                                        Data = _data
                                    };
                                }
                            }
                        }
                    }
                }
                else
                {
                    var agency = AgencyDAO.GetById(UserContext.UserInfo.AccountID);
                    if (agency != null)
                    {
                        var _data = TransactionDAO.TransactionHistory(agency.GameAccountId, from, to, type, skip, 50, out totalMoney, out totalFee);
                        return new
                        {
                            TotalMoney = totalMoney,
                            TotalFee = totalFee,
                            Data = _data
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet, HttpOptions]
        public dynamic GetDashboard(int month, int year)
        {
            try
            {
                var result = new List<AgencyCashFlow>();
                var refundLogs = new List<RefundLog>();
                var transactionLogs = new List<TransactionLog>();

                int from = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}01");
                DateTime _end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                int to = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}{_end.Day.ToString("D2")}");

                var agencies = new List<Ag>();

                var curLevel = UserContext.UserInfo.GroupID;
                if (curLevel == 0)
                {
                    agencies = AgencyDAO.GetAgencies(null);
                    refundLogs = TransactionDAO.GetRefundLogs(null, from, to);
                    transactionLogs = TransactionDAO.GetTransactionLogs(null, from, to);
                }
                else
                {
                    var curId = UserContext.UserInfo.AccountID;
                    agencies = AgencyDAO.GetAgencies(curId);
                    refundLogs = TransactionDAO.GetRefundLogs(curId, from, to);
                    transactionLogs = TransactionDAO.GetTransactionLogs(curId, from, to);
                }

                var rootAgenciesId = agencies.Select(x => x.GameAccountId);
                foreach (var item in agencies)
                {
                    var trf = refundLogs.Where(x => x.AccountId == item.ID).Sum(x => x.TotalRefund);
                    var totalGold = transactionLogs.Where(x => (x.SenderID == item.GameAccountId && !rootAgenciesId.Contains(x.RecvID)) || (x.RecvID == item.GameAccountId && !rootAgenciesId.Contains(x.SenderID))).Sum(x => x.Amount);
                    var transactionCount = transactionLogs.Where(x => (x.SenderID == item.GameAccountId && !rootAgenciesId.Contains(x.RecvID)) || (x.RecvID == item.GameAccountId && !rootAgenciesId.Contains(x.SenderID))).Count();

                    var statisticItem = new AgencyCashFlow
                    {
                        Displayname = item.Displayname,
                        Gold = item.Gold,
                        LockedGold = item.LockedGold,
                        TotalGold = totalGold,
                        TRF = trf,
                        TRF2 = 0,
                        TransactionCount = transactionCount,
                        Level = item.Level
                    };

                    result.Add(statisticItem);
                }

                return result;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public List<AgencyTransAnalytic> GetAgencyTransactionsAnalytic(int month, int year)
        {
            try
            {
                int start = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}01");
                DateTime _end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                int end = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}{_end.Day.ToString("D2")}");

                return TransactionDAO.GetAgencyTransactionsAnalytic(start, end);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return new List<AgencyTransAnalytic>();
        }

        [HttpOptions, HttpGet]
        public async Task<dynamic> GetListTransAccount(long gameAccountId, int from, int to)
        {
            try
            {
                var data = new List<RewardInfoItemVM>();

                // Rate giao dich cua tung tai khoan voi dai ly hien tai
                var rates = AgencyDAO.GetAccountTransactionRateDetail(gameAccountId, from, to);
                if (!rates.Any())
                    return data;

                string list = string.Empty;
                foreach (var item in rates)
                {
                    list += item.AccountId.ToString() + ";" + item.CreatedDateInt.ToString() + ",";
                }
                list = list.TrimEnd(',');
                // Luong tien cua cac tai khoan giao dich voi dai ly hien tai
                var accountMoney = new List<AccountMoneyFlow>();
                using (var financial = new FinancialSoapClient())
                {
                    var response = await financial.GetAccountsMoneyStreamAsync(list);
                    var accountMoneyStreams = response.Body.GetAccountsMoneyStreamResult;

                    if (accountMoneyStreams.Length < 1)
                    {
                        return data;
                    }
                    foreach (var i in accountMoneyStreams)
                    {
                        accountMoney.Add(new AccountMoneyFlow
                        {
                            AccountId = i.AccountId,
                            CreatedDateInt = i.CreatedDateInt,
                            Money = i.Money
                        });
                    }
                }

                var linq = (from rate in rates
                            join account in accountMoney on new { rate.AccountId, rate.CreatedDateInt } equals new { account.AccountId, account.CreatedDateInt }
                            into accountRates

                            from accountRate in accountRates
                            select new AccountMoneyStreamDetailVM
                            {
                                AccountId = rate.AccountId,
                                DisplayName = rate.DisplayName,
                                CreatedDateInt = rate.CreatedDateInt,
                                Money = (long)(rate.Rate * accountRate.Money),
                            });
                var result = linq.GroupBy(x => new { x.AccountId, x.DisplayName }).Select(t => new AccountMoneyFlowDetail {
                    AccountId = t.Key.AccountId,
                    DisplayName = t.Key.DisplayName,
                    TotalMoney = t.Sum(i => i.Money)
                });

                return result;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

    }
}
