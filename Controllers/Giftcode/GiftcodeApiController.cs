using PortalManagement.Models.Bussiness.Giftcode;
using PortalManagement.Models.DataAccess.Game;
using PortalManagement.Models.DataAccess.Giftcode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.Giftcode
{
    public class GiftcodeApiController : ApiController
    {
        [HttpGet]
        public dynamic GiftcodeSummary()
        {
            try
            {
                return GCDAO.GetGCSummary();
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public dynamic GetEventInfo()
        {
            try
            {
                List<object> objs = new List<object>();

                var evtList = GCDAO.GetAllEvent();
                var status = GCDAO.GetAllEventStatus();

                foreach(var e in evtList)
                {
                    var s = (status as IEnumerable).Cast<dynamic>().Where(x => x.EventID == e.ID).ToList();
                    var s1 = s.FirstOrDefault(x => x.Status == true);
                    if (s1 == null)
                        s1 = new
                        {
                            Total = 0
                        };
                    var s2 = s.FirstOrDefault(x => x.Status == false);
                    if (s2 == null)
                        s2 = new
                        {
                            Total = 0
                        };

                    objs.Add(new
                    {
                        ID = e.ID,
                        Name = e.Name,
                        Price = e.Price,
                        Total = s1.Total + s2.Total,
                        Used = s1.Total
                    });
                }

                return objs;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public int Gen(int total, string prefix, string name, long price, string expired, int type, int? agencyId, bool isUseFund = false)
        {
            try
            {
                if (!Regex.IsMatch(prefix, @"^[a-zA-Z0-9]+$") || prefix.Length > 5)
                    return -3;

                if (total <= 0 || price <= 0 || string.IsNullOrEmpty(expired) || string.IsNullOrEmpty(name))
                    return -2;

                if (isUseFund)
                {
                    var status = GameDAO.UpdateMinusGameFund(price * total, 1);
                    if (status < 0) return status;
                }

                int eventId = GCDAO.CreateEvent(name, price, UserContext.UserInfo.AccountID, type, (agencyId.HasValue) ? agencyId.Value : UserContext.UserInfo.AccountID);
                if (eventId > 0)
                {
                    var gc = new PortalManagement.Models.Bussiness.Giftcode.Giftcode(total, prefix.ToUpper(), name, price, expired, eventId);
                    GCDAO.BuildGiftcode(gc.ToString());
                    if (isUseFund)
                    {
                        var status = GameDAO.UpdateMinusGameFund(price * total, 1);
                        if (status < 0) return status;
                    }
                    return eventId;
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return -1;
        }

        [HttpGet]
        public dynamic GetEvent(int id, int page, int type)
        {
            try
            {
                var skip = page * 30;

                return GCDAO.GetGiftCode(id, type, skip, 30);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public List<GiftcodeModel> Search(string code)
        {
            try
            {
                return GCDAO.Search(code);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new List<GiftcodeModel>();
        }

        [HttpGet]
        public List<GCStatistic> GetStatistic(int month, int year)
        {
            try
            {
                DateTime start = new DateTime(year, month, 1);
                DateTime end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

                var curDt = DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day;
                var data = GCDAO.GetStatisticGC(start, end);                
                return data.GroupBy(x => x.Date).Select(t => new GCStatistic {
                    Date = t.Key.Date,
                    AgencyExported = t.Count(x => x.Type == 1),
                    AgencyUsed = t.Count(x => x.Type == 1 && x.Status),
                    AgencyExpired = t.Count(x => x.Type == 1 && curDt > x.Expired),
                    MKTExported = t.Count(x => x.Type == 2),
                    MKTUsed = t.Count(x => x.Type == 2 && x.Status),
                    MKTExpired = t.Count(x => x.Type == 2 && curDt > x.Expired),
                    TestExported = t.Count(x => x.Type == 0),
                    TestUsed = t.Count(x => x.Type == 0 && x.Status),
                    TestExpired = t.Count(x => x.Type == 0 && curDt > x.Expired)
                }).OrderByDescending(m => m.Date).ToList();
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return new List<GCStatistic>();
        }

        [HttpGet]
        public dynamic GetEvents(int type, int page)
        {
            try
            {
                var skip = (page - 1) * 30;
                return GCDAO.GetEvents(type, skip, 30);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }

        [HttpGet]
        public dynamic DeleteEvent(int id)
        {
            try
            {
                var status = GCDAO.DeleteEvent(id);
                NLogManager.LogMessage("Delete Giftcode EventID: " + id + "| AccountId: " + UserContext.UserInfo.AccountID + "| Account: " + UserContext.UserInfo.FullName);
                return status;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public dynamic ExtendEvent(int id, string newDt)
        {
            try
            {
                var status = GCDAO.ExtendGC(id, newDt);
                NLogManager.LogMessage("Extend Giftcode EventID: " + id + "| AccountId: " + UserContext.UserInfo.AccountID + "| Account: " + UserContext.UserInfo.FullName);
                return status;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return false;
        }

        [HttpGet]
        public dynamic GCStatisticAgency(int month, int year)
        {
            try
            {
                DateTime start = new DateTime(year, month, 1);
                DateTime end = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

                return GCDAO.GCStatisticAgency(start, end);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }
    }
}
