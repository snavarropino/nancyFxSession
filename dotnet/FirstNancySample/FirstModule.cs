using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstNancySample
{
    public class FirstModule: NancyModule
    {
        public FirstModule()
        {
            Get["/"] = _ => "I did it my way!";
            Get["/dashboard"] = _ => View["dashboard.html"];
        }
    }        
}