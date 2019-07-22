using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class PayLog
    {
        public string TransactionID { get; set; }
        public int Amount { get; set; }        
        public string Serial { get; set; }

        public string CardCode { get; set; }

        public string Pin { get; set; }
        public int CardType { get; set; }
        public DateTime CreatedTime { get; set; }
        [JsonIgnore]
        public int Status { get; set; }
        [JsonIgnore]
        public int PayId { get; set; }

        [JsonIgnore]
        public long AccountId { get; set; }
        public string StatusStr
        {
            get
            {
                if (Status != 1)
                    return "Thất bại";
                else if (Status == 1)
                    return "Thành công";
                else if (Status == 0)
                    return "Đang xử lý";
                else
                    return string.Empty;
            }
        }
    }
}