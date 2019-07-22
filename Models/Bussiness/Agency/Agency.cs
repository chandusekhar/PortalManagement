using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Agency
{
    public class Agency
    {
        public int ID { get; set; }
        public long GameAccountId { get; set; }
        public bool IsAgency { get; set; }
    }

    public class AgencyInfo
    {
        public long Gold { get; set; }
        public long TotalBuy { get; set; }
        public long TotalSell { get; set; }
        public long Profit {
            get {
                return TotalBuy - TotalSell;
            }
        }
    }

    public class Ag
    {
        public int ID { get; set; }
        public long GameAccountId { get; set; }
        public long Gold { get; set; }
        public long LockedGold { get; set; }
        public string Displayname { get; set; }
        public int Level { get; set; }
    }

    public class AccountRate
    {
        public long AccountId { get; set; }
        public int CreatedDateInt { get; set; }
        public decimal Rate { get; set; }
    }

    public class RewardInfoItem
    {
        public int CreatedTimeInt { get; set; }
        public DateTime CreatedTime
        {
            get
            {
                DateTime dt;
                DateTime.TryParseExact(CreatedTimeInt.ToString(), "yyyyMMdd",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dt);
                return dt;
            }
        }
        public decimal TotalMoney { get; set; }
        public long TotalTransaction { get; set; }
    }

    public class RewardInfo
    {
        public string WeekName { get; set; }
        public List<RewardInfoItemVM> Items { get; set; }
        public long Reward { get; set; }
        public long WeekTotalMoney { get; set; }
        public long WeekTotalTransaction { get; set; }
    }

    public class RewardInfoItemVM
    {
        public int CreatedDateInt { get; set; }
        public string CreatedDateStr { get; set; }
        public decimal TotalMoney { get; set; }
        public long TotalTransaction { get; set; }
    }

    public class AccountMoneyStreamVM
    {
        public long TotalTransaction { get; set; }
        public long TotalMoney { get; set; }
        public int CreatedDateInt { get; set; }
        public DateTime CreatedDate
        {
            get
            {
                DateTime dt;
                DateTime.TryParseExact(CreatedDateInt.ToString(), "yyyyMMdd",
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out dt);
                return dt;
            }
        }
    }

    public class AccountMoneyStreamDetailVM
    {
        public long AccountId { get; set; }
        public string DisplayName { get; set; }
        public long Money { get; set; }
        public int CreatedDateInt { get; set; }
    }

    public class AccountMoneyFlowDetail
    {
        public long AccountId { get; set; }
        public string DisplayName { get; set; }
        public long TotalMoney { get; set; }
    }

    public class AccountMoneyFlow
    {
        public long AccountId { get; set; }
        public int CreatedDateInt { get; set; }
        public long Money { get; set; }
    }

    public class AgencyTransAnalytic
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public long GameAccountId { get; set; }
        public long TotalRecv { get; set; }
        public long TotalSend { get; set; }
    }

    public class AccountRateDetail
    {
        public long AccountId { get; set; }
        public string DisplayName { get; set; }
        public int CreatedDateInt { get; set; }
        public decimal Rate { get; set; }
    }
}