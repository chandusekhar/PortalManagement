using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Utilities;

namespace PortalManagement.Models.Bussiness.Giftcode
{
    public class Giftcode
    {
        readonly StringBuilder _query;
        private bool _start = false;
        private bool _end = false;

        public Giftcode(int total, string prefix, string name, long price, string expired, int eventId)
        {
            _query = new StringBuilder();
            StartQuery();
            Enumerable.Range(0, total).ToList().ForEach(x =>
            {
                string gc = prefix + RandomString(12 - prefix.Length);
                _query.AppendLine($"insert into dbo.Giftcode (Code, Gold, EventID, Expired, Status) values ('{gc}', {price}, {eventId}, '{expired}', 0)");
            });
            EndQuery();
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[RandomUtil.NextInt(s.Length)]).ToArray());
        }

        public void StartQuery()
        {
            _start = true;
            _query.AppendLine("begin transaction");
            _query.AppendLine("begin try");
        }

        public void EndQuery()
        {
            _end = true;
            _query.AppendLine("commit transaction");
            _query.AppendLine("end try");
            _query.AppendLine("begin catch");
            _query.AppendLine("if @@trancount > 0 begin rollback transaction end;");
            _query.AppendLine("throw 50000, 'sql exception', 1");
            _query.AppendLine("end catch");
        }

        public override string ToString()
        {
            if (!_start && !_end)
                throw new Exception("Query not started or already finished yet");

            return _query.ToString();
        }
    }
}