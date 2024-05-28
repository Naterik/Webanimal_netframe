using NetFramwork_WildNature.Db;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    private readonly string[] allowedRoles;

    public RoleAuthorizeAttribute(params string[] roles)
    {
        this.allowedRoles = roles;
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        if (httpContext.Session["user"] == null)
        {
            return false;
        }

        var user = (Account)httpContext.Session["user"];
        return allowedRoles.Contains(user.RoleID.ToString());
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        var user = (Account)filterContext.HttpContext.Session["user"];

        if (user != null && user.RoleID == 2 &&
    (filterContext.HttpContext.Request.Url.AbsolutePath.Contains("/Admin/Account") ||
    filterContext.HttpContext.Request.Url.AbsolutePath.Contains("/Admin/Role")))
        {
            // Tạo một ViewResult mới và trỏ đến View "NoPermission"
            filterContext.Result = new ViewResult
            {
                ViewName = "401", // Tên của View bạn muốn hiển thị
                ViewData = new ViewDataDictionary { { "Layout", "~/Areas/Admin/Views/Shared/_AdminLayout.cshtml" } }
            };
            return;
        }

        if (filterContext.HttpContext.Session["user"] != null)
        {
            filterContext.Result = new RedirectResult("/Admin/Home");
        }
        else
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}