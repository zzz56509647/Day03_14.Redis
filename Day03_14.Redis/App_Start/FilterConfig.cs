﻿using System.Web;
using System.Web.Mvc;

namespace Day03_14.Redis
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
