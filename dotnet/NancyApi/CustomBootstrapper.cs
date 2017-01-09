using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy.Configuration;
using Nancy.Diagnostics;

namespace NancyApi
{
    public class CustomBootstrapper: DefaultNancyBootstrapper
    {
        protected override IRootPathProvider RootPathProvider
        {
            get
            {
                return new CustomRootProv();
            }
        }

        public override void Configure(Nancy.Configuration.INancyEnvironment environment)
        {
            environment.Diagnostics(password: "nancy");
        }
        
    }

    public class CustomRootProv : IRootPathProvider
    {
        public string GetRootPath()
        {
            return "./";
        }
    }
}
