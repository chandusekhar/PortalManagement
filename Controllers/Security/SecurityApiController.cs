using PortalManagement.Models.DataAccess;
using PortalManagement.Models.DataAccess.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Utilities.Log;

namespace PortalManagement.Controllers.Security
{
    public class SecurityApiController : ApiController
    {
        [HttpGet]
        public IEnumerable<dynamic> GetListUser()
        {
            try
            {
                return SecurityDAO.GetAccount();
            }catch(Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }

        [HttpGet]
        public bool AddAccount(string username, string password)
        {
            try
            {
                SecurityDAO.AddAccount(username, Utilities.Encryption.Security.MD5Encrypt(password), UserContext.UserInfo.AccountID);
                RoleBuilder.Init();
                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return false;
        }

        [HttpGet]
        public bool DeleteAccount(int accountId)
        {
            try
            {
                if (accountId == 1)
                    return false;

                SecurityDAO.DeleteAccount(accountId);
                RoleBuilder.Init();
                return true;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return false;
        }

        [HttpGet]
        public List<ActiveRole> GetActiveRoles(int user)
        {
            try
            {
                return RoleBuilder.GetRoles(user);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return null;
        }
        [HttpPost]
        public bool UpdateRole([FromBody] List<ActiveRole> activeRoles, int user)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine($"delete from dbo.Role where AccountID = {user}");
                foreach (var r in activeRoles)
                {
                    if(r.IsActive)
                        query.AppendLine($"insert into dbo.Role (AccountID, SubID) values ({user}, {r.SubID})");
                }
                RoleDAO.Execute(query.ToString());
                RoleBuilder.Init();
                return true;
            }catch(Exception ex)
            {
                NLogManager.PublishException(ex);
            }

            return false;
        }
    }
}
