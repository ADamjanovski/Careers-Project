using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalProject.Models.Filters
{
    public class RoleSessionFilter : ActionFilterAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = filterContext.HttpContext.User.Identity.GetUserId();
                // Check if the user is logged in
                var user = db.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Roles)
                .Include(u => u.CustomUser)
                .FirstOrDefault();

                if (user != null)
                {
                    // Retrieve the role name
                    var userRoleId = user.Roles.FirstOrDefault()?.RoleId;
                    var roleName = db.Roles
                        .Where(r => r.Id == userRoleId)
                        .Select(r => r.Name)
                        .FirstOrDefault();

                    // Store the role in session
                    HttpContext.Current.Session["UserRole"] = roleName;

                    // Store the CustomUserId in session
                    if (user.CustomUser != null)
                    {
                        HttpContext.Current.Session["CustomUserId"] = user.CustomUser.Id;
                    }
                }
                else
                {
                    // Clear session variables if user or custom user is not found
                    HttpContext.Current.Session["UserRole"] = null;
                    HttpContext.Current.Session["CustomUserId"] = null;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}