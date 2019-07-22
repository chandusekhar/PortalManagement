using PortalManagement.Models.DataAccess.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.CardSearch
{
    public class CardSearchApiController : ApiController
    {
        [HttpGet]
        public dynamic SearchCard(int searchType, string serial, string pin)
        {
            try
            {

                return PayDAO.SearchCard(searchType, serial, pin);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
            return null;
        }
    }
}
