using PortalManagement.Models.DataAccess.Payment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.CardConfig
{
    public class CardConfigApiController : ApiController
    {
        public object AccountSession { get; private set; }

        [HttpOptions, HttpGet]
        public IEnumerable<Models.Bussiness.ConfigCard.CardConfig> GetCards()
        {
            return PayDAO.GetCards();
        }

        [HttpOptions, HttpPost]
        public bool Update([FromBody] List<Models.Bussiness.ConfigCard.CardConfig> configs)
        {
            try
            {
                StringBuilder query = new StringBuilder();

                foreach (var c in configs)
                {
                    string payConfig = c.Pay1 + "|" + c.Pay2 + "|" + c.Pay3 + "|" + c.Pay4 + "|" + c.Pay5;
                    query.AppendLine($"update dbo.CardConfig set Enable = '{c.Enable}', Promotion = {c.Promotion}, CashoutRate = {c.CashoutRate}, EnableCashout = '{c.EnableCashout}', TopupRate = {c.TopupRate}" +
                        $", PromotionCashout = {c.PromotionCashout}, PayOrderConfig = N'{payConfig}' WHERE ID = {c.ID}");
                }

                PayDAO.Update(query.ToString());
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
