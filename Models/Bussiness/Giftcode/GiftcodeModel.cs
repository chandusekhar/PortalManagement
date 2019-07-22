using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Giftcode
{
    public class GiftcodeModel
    {
        public string Code { get; set; }
        public long Gold { get; set; }
        public long Coin { get; set; }
        public DateTime CreatedTime { get; set; }
        [JsonIgnore]
        public bool Status { get; set; }
        public string StatusStr {
            get { return ModelHelper.Instance.GetStatus(Status); }
        }
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public int EventID { get; set; }
        public DateTime Expired { get; set; }
        public DateTime? UsedTime { get; set; }
    }
}