using PortalManagement.Models.Category;
using PortalManagement.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class RoleBuilder
{
    private static List<Api> _apis;
    private static List<Heading> _headings;
    private static List<SubHeading> _subHeadings;
    private static List<Role> _roles;

    public static void Init()
    {
        _apis = RoleDAO.GetApis().ToList();
        _headings = RoleDAO.GetHeadings().ToList();
        _subHeadings = RoleDAO.GetSubHeadings().ToList();
        _roles = RoleDAO.GetRoles().ToList();
    }

    public static List<string> GetAllowPath()
    {
        var roles = _roles.Where(x => x.AccountID == UserContext.UserInfo.AccountID).Select(x => x.SubID);
        var subs = _subHeadings.Where(x => roles.Contains(x.ID));
        var apis = _apis.Where(x => roles.Contains(x.SubID));
        List<string> paths = new List<string>(apis.Select(x => x.Path).Union(subs.Select(y => handleAction(y.Controller, y.Action))));
        return paths;
    }

    public static List<ActiveRole> GetRoles(int userId)
    {
        List<ActiveRole> roles = new List<ActiveRole>();
        var r = _roles.Where(x => x.AccountID == userId).Select(x => x.SubID);
        foreach(var sub in _subHeadings)
        {
            roles.Add(new ActiveRole
            {
                IsActive = r.Contains(sub.ID),
                SubID = sub.ID,
                Name = sub.Name
            });
        }
        return roles;
    }

    private static string handleAction(string controller, string action)
    {
        if(action.ToLower() == "index")
            return $"/{controller}";
        return $"/{controller}/{action}";
    }

    public static UserCategory GetUserCategories()
    {
        if (UserContext.UserInfo.AccountID <= 0)
            return new UserCategory() { Headings = new List<Heading>(), SubHeading = new List<SubHeading>()};

        var roles = _roles.Where(x => x.AccountID == UserContext.UserInfo.AccountID).Select(x => x.SubID);
        var subs = _subHeadings.Where(x => roles.Contains(x.ID));
        var lstHeadId = subs.Select(x => x.HeadID);
        var heads = _headings.Where(x => lstHeadId.Contains(x.ID));

        return new UserCategory
        {
            Headings = heads.ToList(),
            SubHeading = subs.ToList()
        };
    }
}

public class ActiveRole
{
    public int SubID { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

public class UserCategory
{
    public List<Heading> Headings { get; set; }
    public List<SubHeading> SubHeading { get; set; }
}
