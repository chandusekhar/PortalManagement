using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PortalManagement.Models.Bussiness.UserInfo
{
    public class SearchUserFilter
    {
        private StringBuilder _query;
        private string _displayName;
        private string _userName;
        private long _id;
        private string _phone;


        public void SetDisplayName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
                return;

            _displayName = displayName;
        }

        public void SetUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return;

            _userName = userName;
        }

        public void SetID(string idStr)
        {
            long id;

            if (!long.TryParse(idStr, out id))
                return;

            if (id <= 0)
                return;

            _id = id;
        }

        public void SetPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return;

            _phone = phone;
        }

        public override string ToString()
        {
            bool or = false;
            _query = new StringBuilder();
            _query.AppendLine("select top 50 AccountID, Username, UserType, DisplayName, Gold, Coin, LockedGold, CreatedTime, Tel, IsBlocked, IsMute, IsOTP from dbo.Account with (nolock) where ");
            if (!string.IsNullOrEmpty(_displayName))
            {
                _query.Append($"DisplayName like '%{_displayName}%'");
                or = true;
            }
            if (!string.IsNullOrEmpty(_userName))
            {
                if (or)
                    _query.Append(" or ");
                _query.Append($"Username like '%{_userName}%'");
                or = true;
            }
            if (_id > 0)
            {
                if (or)
                    _query.Append(" or ");
                _query.Append($"AccountID = {_id}");
                or = true;
            }
            if (!string.IsNullOrEmpty(_phone))
            {
                if (or)
                    _query.Append(" or ");
                _query.Append($"Tel like '%{_phone}%'");
                or = true;
            }

            return _query.ToString();
        }
    }
}