using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.Payment
{
    public class TopupBilling
    {
        public int? Amount { get; set; }
        public DateTime? Date { get; set; }

        [JsonIgnore]
        public int? CardType { get; set; }
        public string CardTypeStr {
            get { return ModelHelper.Instance.GetCardTypeName(CardType); }
        }
    }
}