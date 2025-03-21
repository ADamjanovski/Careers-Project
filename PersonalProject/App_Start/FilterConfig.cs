﻿using PersonalProject.Models.Filters;
using System.Web;
using System.Web.Mvc;

namespace PersonalProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RoleSessionFilter()); // Add the custom filter

        }
    }
}
