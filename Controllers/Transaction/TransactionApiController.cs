using PortalManagement.Models.DataAccess.Agency;
using PortalManagement.Models.DataAccess.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.Transaction
{
    public class TransactionApiController : ApiController
    {
        [HttpGet]
        public int RefundTran(long id, string reason)
        {
            try
            {
                var response = TransactionDAO.RefundTransaction(id, UserContext.UserInfo.AccountID, UserContext.UserInfo.FullName, reason);
                return response;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return -99;  
        }

        [HttpGet]
        public dynamic GetRefundLog()
        {
            return TransactionDAO.GetRefundLog(UserContext.UserInfo.GroupID, UserContext.UserInfo.FullName);
        }

        [HttpGet]
        public dynamic Search(long id)
        {
            try
            {
                return TransactionDAO.Search(id);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetTransactionFees(int month, int year)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

            return TransactionDAO.GetTransactionFees(start, end);
        }

        [HttpGet]
        public dynamic GetTransactionByDate(string date, int page)
        {
            try
            {
                var skip = page * 30;
                return TransactionDAO.GetTransactionByDate(date, skip, 30);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetListTransactionFee(long id)
        {
            try
            {
                return TransactionDAO.Search(id);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public bool UpdateState(long transId, int state)
        {
            try
            {
                if (UserContext.UserInfo.GroupID != 0)
                {
                    var agencyAccount = AgencyDAO.GetById(UserContext.UserInfo.AccountID);
                    if (agencyAccount == null)
                        return false;
                    TransactionDAO.UpdateState(transId, state, agencyAccount.GameAccountId);
                }
                else TransactionDAO.UpdateStateAdmin(transId, state);
                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return false;
        }
    }
}