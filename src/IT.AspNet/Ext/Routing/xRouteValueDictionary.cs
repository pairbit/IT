using Microsoft.AspNetCore.Routing;
using System;

namespace IT.Ext
{
    public static class xRouteValueDictionary
    {
        public static String GetAction(this RouteValueDictionary values) => values["action"] as String;

        public static String GetController(this RouteValueDictionary values, Boolean postfix = false)
        {
            var controller = values["controller"] as String;
            if (postfix && controller != null) controller += "Controller";
            return controller;
        }
    }
}