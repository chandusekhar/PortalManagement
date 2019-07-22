using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalManagement.Models.Bussiness.ConfigCard
{
    public class CardConfig
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public int Prize { get; set; }
        public bool Enable { get; set; }
        public int Promotion { get; set; }
        public int CashoutRate { get; set; }
        public bool EnableCashout { get; set; }
        public int TopupRate { get; set; }
        public int PromotionCashout { get; set; }
        private string _PayOrderConfig;
        public string PayOrderConfig
        {
            get
            {
                return _PayOrderConfig;
            }
            set
            {
                _PayOrderConfig = value;
                if (!string.IsNullOrEmpty(_PayOrderConfig))
                {
                    var spl = _PayOrderConfig.Split('|');
                    int pay1 = 0;
                    int pay2 = 0;
                    int pay3 = 0;
                    int pay4 = 0;
                    int pay5 = 0;
                    int pay6 = 0;
                    int pay7 = 0;
                    int pay8 = 0;
                    int.TryParse(spl[0], out pay1);
                    if (spl.Length > 1)
                        int.TryParse(spl[1], out pay2);
                    if (spl.Length > 2)
                        int.TryParse(spl[2], out pay3);
                    if (spl.Length > 3)
                        int.TryParse(spl[3], out pay4);
                    if (spl.Length > 4)
                        int.TryParse(spl[4], out pay5);
                    if (spl.Length > 5)
                        int.TryParse(spl[5], out pay6);
                    if (spl.Length > 6)
                        int.TryParse(spl[6], out pay7);
                    if (spl.Length > 7)
                        int.TryParse(spl[7], out pay8);
                    if (Pay1 == 0)
                        Pay1 = pay1;
                    if (Pay2 == 0)
                        Pay2 = pay2;
                    if (Pay3 == 0)
                        Pay3 = pay3;
                    if (Pay4 == 0)
                        Pay4 = pay4;
                    if (Pay5 == 0)
                        Pay5 = pay5;
                    if (Pay6 == 0)
                        Pay6 = pay6;
                    if (Pay7 == 0)
                        Pay7 = pay7;
                    if (Pay8 == 0)
                        Pay8 = pay8;
                }
            }
        }
        public int Pay1 { get; set; }
        public int Pay2 { get; set; }
        public int Pay3 { get; set; }
        public int Pay4 { get; set; }
        public int Pay5 { get; set; }
        public int Pay6 { get; set; }
        public int Pay7 { get; set; }
        public int Pay8 { get; set; }
    }
}