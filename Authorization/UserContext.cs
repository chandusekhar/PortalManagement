using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class UserContext
{
    private static readonly ConcurrentDictionary<string, UserInfo> _context;

    static UserContext()
    {
        _context = new ConcurrentDictionary<string, UserInfo>();
    }

    public static void Initialize()
    {

    }

    public static void Set(UserInfo info)
    {
        if (HttpContext.Current.Session != null)
        {
            _context.AddOrUpdate(HttpContext.Current.Session.SessionID, info, (k, v) => v = info);
        }
    }

    public static long AccountID
    {
        get
        {
            long userId = 0;

            if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var s = HttpContext.Current.User.Identity.Name.Split('|');
                if (s.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(s[0]))
                    {
                        userId = Convert.ToInt64(s[0]);
                    }
                }
            }

            return userId;
        }
    }

    public static string AccountName
    {
        get
        {
            string userName = string.Empty;

            if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                var s = HttpContext.Current.User.Identity.Name.Split('|');
                if (s != null && s.Length > 1)
                    userName = s[1];
            }

            return userName;
        }
    }

    public static UserInfo UserInfo
    {
        get
        {
            return new UserInfo
            {
                AccountID = (int)AccountID,
                FullName = AccountName
            };
            UserInfo info;

            if (HttpContext.Current.Session != null)
            {
                if (_context.TryGetValue(HttpContext.Current.Session.SessionID, out info))
                {
                    return info;
                }

                //Set(info = new UserInfo());

                return null;
            }

            return null;
        }
    }
}

public class UserInfo
{
    public bool IsAuthenticated
    {
        get
        {
            return AccountID > 0;
        }
    }
    public int AccountID { get; set; }
    public string FullName { get; set; }
    public int GroupID { get; set; }
    public DateTime CreatedTime { get; private set; }

    public UserInfo()
    {
        CreatedTime = DateTime.Now;
    }
}