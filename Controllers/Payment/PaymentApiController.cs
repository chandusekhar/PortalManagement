using Newtonsoft.Json;
using PortalManagement.Models.DataAccess.Payment;
using PortalManagement.Models.DataAccess.UserInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.Payment
{
    public class PaymentApiController : ApiController
    {
        [HttpGet]
        public int AddMoney(string accountName, long amount, int type, string reason)
        {
            try
            {
                if (type <= 0 || type > 4)
                    return -99;

                var user = UserDAO.GetInfo(accountName);
                if (user == null) return -1;
                long reference = PayDAO.AddLog(user.AccountID, user.DisplayName, type, amount, reason, UserContext.UserInfo.AccountID, UserContext.UserInfo.FullName);
                PayDAO.AddGold(user.AccountID, amount, "Nạp tiền", reference, 1004, 1);
                return 1;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return -99;
        }

        [HttpGet]
        public dynamic Analytic(int month, int year)
        {
            try
            {
                int start = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}01");
                DateTime _end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                int end = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}{_end.Day.ToString("D2")}");

                var sum = PayDAO.Sumary(start, end);

                List<dynamic> objs = new List<dynamic>();

                if (sum != null)
                {
                    var groupByDay = ((IEnumerable)sum).Cast<dynamic>().GroupBy(x => x.CreatedTimeInt);

                    foreach (var d in groupByDay)
                    {
                        var momo = d.FirstOrDefault(x => x.Type == 2)?.Total;
                        var card = d.FirstOrDefault(x => x.Type == 1)?.Total;
                        var agency = d.FirstOrDefault(x => x.Type == 3)?.Total;
                        var err = d.FirstOrDefault(x => x.Type == 4)?.Total;

                        objs.Add(new
                        {
                            Days = d.FirstOrDefault().CreatedTimeInt,
                            Momo = momo == null ? 0 : momo,
                            Card = card == null ? 0 : card,
                            Agency = agency == null ? 0 : agency,
                            ErrGame = err == null ? 0 : err,
                        });
                    }

                    return objs;
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }
        [HttpGet]
        public dynamic GetLog(int start, int end, int page)
        {
            try
            {
                var skip = page * 30;
                return PayDAO.GetLog(start, end, skip, 30);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }


        [HttpGet]
        public dynamic GetCurrentLog()
        {
            try
            {
                return PayDAO.GetLog(UserContext.UserInfo.AccountID);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetUnverifyPayment()
        {
            try
            {
                return PayDAO.GetUnverifyPayment();
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public bool AcceptCard(long cardId)
        {
            try
            {
                return PayDAO.AcceptCard(cardId);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public bool DeleteCard(long cardId)
        {
            try
            {
                return PayDAO.DeleteCard(cardId);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public dynamic GetCardInBank()
        {
            try
            {
                return PayDAO.GetCardInBank();
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic DeleteCardInBank(long cardId)
        {
            try
            {
                return PayDAO.DeleteCardInBank(cardId);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public dynamic GetErrorCardTransaction()
        {
            try
            {
                return PayDAO.GetErrorCardTransaction();
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic Charge(int cardType, int amount, int total)
        {
            try
            {
                if (cardType != 1 && cardType != 2 && cardType != 3)
                {
                    return null;
                }

                int successtransaction = 0;
                int errortransaction = 0;

                try
                {
                    string serviceCode = string.Empty;

                    if (cardType == 1)
                    {
                        serviceCode = "VTT";
                    }
                    else if (cardType == 2)
                    {
                        serviceCode = "VMS";
                    }
                    else if (cardType == 3)
                    {
                        serviceCode = "VNP";
                    }

                    for (int i = 0; i < total; i++)
                    {
                        long transactionId = DateTime.Now.Ticks;

                        int outRes = 0;
                        string requestId = string.Empty;

                        var service = new muathe24h.MechantServicesSoapClient();
                        string email = "abcdefgh@gmail.com";
                        string pass = "123456";

                        var res = service.BuyCards(new muathe24h.UserCredentials { userName = email, pass = pass }
                          , transactionId.ToString(), serviceCode, amount, 1);

                        NLogManager.LogMessage(JsonConvert.SerializeObject(res));
                        string resultCode = res?.RepCode.ToString();


                        if (res != null && res.RepCode == 0)
                        {
                            var seri = JsonConvert.DeserializeObject<List<CardObject>>(res.Data.ToString());
                            if (PayDAO.InsertCard(seri[0].PinCode, seri[0].Serial, amount, string.Empty, cardType, serviceCode, "muathe24h", DateTime.Now, transactionId.ToString(), resultCode, true))
                            {
                                successtransaction++;
                            }
                            else
                            {
                                PayDAO.InsertCard(string.Empty, string.Empty, amount, string.Empty, cardType, serviceCode, "muathe24h", DateTime.Now, requestId, resultCode, false);
                                errortransaction++;
                            }
                        }
                        else
                        {
                            PayDAO.InsertCard(string.Empty, string.Empty, amount, string.Empty, cardType, serviceCode, "muathe24h", DateTime.Now, requestId, resultCode, false);

                            errortransaction++;
                        }
                    }

                    return new
                    {
                        suc = successtransaction,
                        err = errortransaction
                    };
                }
                catch (Exception ex)
                {
                    NLogManager.PublishException(ex);
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetTopupBilling(int month, int year)
        {
            try
            {
                int start = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}01");
                DateTime _end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                int end = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}{_end.Day.ToString("D2")}");
                return PayDAO.GetTopupBilling(start, end);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic GetCashoutBilling(int month, int year)
        {
            try
            {
                int start = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}01");
                DateTime _end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
                int end = int.Parse($"{year.ToString("D4")}{month.ToString("D2")}{_end.Day.ToString("D2")}");
                return PayDAO.GetCashoutBilling(start, end);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }
    }

    public class CardObject
    {
        public string ProviderCode { get; set; }
        public string Serial { get; set; }
        public string PinCode { get; set; }
        public int Amount { get; set; }
    }
}
