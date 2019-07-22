using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness
{
    public class ModelHelper
    {
        private static readonly Lazy<ModelHelper> _instance = new Lazy<ModelHelper>(() => new ModelHelper());
        public static ModelHelper Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public string GetTopupName(int? type)
        {
            if (!type.HasValue) return "";
            switch (type.Value)
            {
                case 1:
                    return "MOBILE_CARD";
                case 2:
                    return "IAP";
                case 3:
                    return "BANK";
                case 4:
                    return "SMS";
                default:
                    return "Other";
            }
        }

        public string GetCardTypeName(int? type)
        {
            if (!type.HasValue) return "";
            switch (type.Value)
            {
                case 1:
                    return "Viettel";
                case 2:
                    return "Mobi";
                case 3:
                    return "Vina";
                default:
                    return "";
            }
        }

        public string GetStatus(bool? status)
        {
            if (!status.HasValue) return "";
            else if (status.Value) return "Đã duyệt thẻ";
            else return "Chưa duyệt thẻ";
        }
    }
}