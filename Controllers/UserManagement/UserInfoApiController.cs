using PortalManagement.Models;
using PortalManagement.Models.Bussiness.UserInfo;
using PortalManagement.Models.DataAccess.Game;
using PortalManagement.Models.DataAccess.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using Utilities.Log;

namespace PortalManagement.Controllers.UserManagement
{
    public class UserInfoApiController : ApiController
    {
        [HttpGet]
        public dynamic Search(string displayname, string username, string id, string phone)
        {
            try
            {
                SearchUserFilter filter = new SearchUserFilter();
                filter.SetDisplayName(displayname);
                filter.SetUserName(username);
                filter.SetID(id);
                filter.SetPhone(phone);
                return UserDAO.ExecuteQuery(filter.ToString());
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public dynamic GetBalanceHistory(long accountId, int page = 0)
        {
            try
            {
                var gameLst = SimpleCache.Game;
                var skipItem = 20 * page;
                var transaction = GameDAO.GetBalanceHistory(accountId, skipItem, 20);

                return transaction;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public dynamic ResetPassword(long accountId)
        {
            try
            {
                UserDAO.ChangePassword(accountId, Utilities.Encryption.Security.MD5Encrypt("123456"));
                NLogManager.LogMessage("AccountID: " + UserContext.AccountID + "|AccountName: " + UserContext.AccountName + " reset password accountId: " + accountId);
                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return false;
        }

        [HttpGet]
        public dynamic GetTopupHistory(long accountId, int page = 0)
        {
            try
            {
                var gameLst = SimpleCache.Game;
                var skipItem = 20 * page;
                var transaction = GameDAO.GetTopupHistory(accountId, skipItem, 20);

                return transaction;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public dynamic GetCashoutHistory(long accountId, int page = 0)
        {
            try
            {
                var gameLst = SimpleCache.Game;
                var skipItem = 20 * page;
                var transaction = GameDAO.GetCashoutHistory(accountId, skipItem, 20);

                return transaction;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public dynamic GetLoginLog(long accountId, int page = 0)
        {
            try
            {
                var skipItem = 20 * page;
                return UserDAO.GetLoginLog(accountId, skipItem, 20);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public bool BlockLogin(long accountId, string reason, int state)
        {
            try
            {
                UserDAO.BlockLogin(accountId, reason, state, UserContext.UserInfo.AccountID);
                return true;

            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public bool OffLoginOTP(long accountId)
        {
            try
            {
                UserDAO.OffLoginOTP(accountId);
                return true;

            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public bool BlockChat(long accountId, int state)
        {
            try
            {
                UserDAO.BlockChat(accountId, state, UserContext.UserInfo.AccountID);
                return true;

            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }


        [HttpGet]
        public dynamic ListBlockLogin(int page = 0)
        {
            try
            {
                var skipItem = 20 * page;
                return UserDAO.GetListBlockLogin(skipItem, 20);

            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic ListBlockChat(int page = 0)
        {
            try
            {
                var skipItem = 20 * page;
                return UserDAO.GetListBlockChat(skipItem, 20);

            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetNRU(int month, int year)
        {
            try
            {
                DateTime start = new DateTime(year, month, 1);
                DateTime end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

                var result = new List<NRUDay>();

                var items = UserDAO.GetNRU(start, end);
                var data = items.GroupBy(x => x.CreatedDate).ToList();
                foreach (var item in data)
                {
                    var daily = new NRUDay
                    {
                        CreatedDate = item.Key,
                        Android = item.Where(x => x.DeviceType == 1).Sum(x => x.TotalAccount),
                        IOS = item.Where(x => x.DeviceType == 2).Sum(x => x.TotalAccount),
                        EXE = item.Where(x => x.DeviceType == 3).Sum(x => x.TotalAccount),
                        OSX = item.Where(x => x.DeviceType == 4).Sum(x => x.TotalAccount),
                        Web = item.Where(x => x.DeviceType == 5).Sum(x => x.TotalAccount)
                    };

                    result.Add(daily);
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
        public dynamic GetDAU(int month, int year)
        {
            try
            {
                int start = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}01");
                DateTime _end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                int end = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}{_end.Day.ToString("D2")}");
                var result = new List<NRUDay>();

                var items = UserDAO.GetDAU(start, end);
                var data = items.GroupBy(x => x.CreatedDate).ToList();
                foreach (var item in data)
                {
                    var daily = new NRUDay
                    {
                        CreatedDate = item.Key,
                        Android = item.Where(x => x.DeviceType == 1).Sum(x => x.TotalAccount),
                        IOS = item.Where(x => x.DeviceType == 2).Sum(x => x.TotalAccount),
                        EXE = item.Where(x => x.DeviceType == 3).Sum(x => x.TotalAccount),
                        OSX = item.Where(x => x.DeviceType == 4).Sum(x => x.TotalAccount),
                        Web = item.Where(x => x.DeviceType == 5).Sum(x => x.TotalAccount)
                    };

                    result.Add(daily);
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
        public dynamic GetPU(int month, int year)
        {
            try
            {
                int start = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}01");
                DateTime _end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                int end = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}{_end.Day.ToString("D2")}");
                return UserDAO.GetPU(start, end);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetTopbalance(int page = 0)
        {
            try
            {
                var skipItem = 20 * page;

                return UserDAO.GetTopBalance(skipItem, 20);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public List<TransferLog> GetTransferLog(long accountId, int page = 0)
        {
            try
            {
                var skipItem = 20 * page;
                return UserDAO.GetTransferLog(accountId, skipItem, 20);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetPlayLog(int gameId, long sessionId)
        {
            try
            {
                return UserDAO.GetPlayLog(gameId, sessionId);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetVIPInfo(long accountId)
        {
            if (UserContext.UserInfo.AccountID < 1)
                return null;

            try
            {
                return UserDAO.GetVIPInfo(accountId);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }
    }
}
