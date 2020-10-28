﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation.TokenValidationResources;

namespace Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation
{
    public static class TokenValidationExtension
    {
        //Contains default values that correspond to client-side endpoints
        //that http requests are made to the server for
        private static readonly List<string> PathsToExclude = new List<string>()
        {
            //Libraries
            @"^/swagger", @"^/sock",
            //File types
            @"\.js$", @"\.map$", @"\.css$", @"\.ico$",
            //Client-side routes
            @"^/mainmenu", @"^/account/create", @"^/account/login", @"^/play/\d", @"^/play/guest"
        };

        /// <summary>
        ///     Adds <see cref="TokenValidationMiddleware"/> to the request pipeline
        /// </summary>
        ///     <remarks>
        ///     The middleware looks for a "TandemTriviaToken" header in the incoming request
        ///     <br />and validates its contents for endpoints not marked with a [<see cref="NoToken"/>]
        ///     attribute<br />before proceeding to the Controller method for the endpoint requested
        ///     </remarks>
        /// <param name="app"></param>
        public static void UseTokenValidationMiddleware(this IApplicationBuilder app)
        {
            //When the request path is present in the PathsToExclude list
            //Use the TokenValidationMiddleware during the request
            app.UseWhen(ctx =>
            {
                if (ctx.Request.Path == "/") return false;
                bool useMiddleware = !PathsToExclude.Any(path =>
                {
                    Regex pattern = new Regex(path);
                    bool requestIsInExcludeList = pattern.IsMatch(ctx.Request.Path);
                    return requestIsInExcludeList;
                });
                return useMiddleware;
            },
            app => app.UseMiddleware<TokenValidationMiddleware>());
        }

        /// <summary>
        ///     Identifies server-side endpoints that are marked with the [<see cref="NoToken"/>]
        ///     attribute<br />and adds them to a list of routes to not run the
        ///     <see cref="TokenValidationMiddleware"/> against
        /// </summary>
        /// <param name="services"></param>
        public static void SetTokenValidationPathsToExclude(this IServiceCollection services)
        {
            //Set via Startup's ConfigureServices so it is calculated once on server spin up
            //And not on every single incoming request to the server
            IEnumerable<Type> controllerClasses = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.Namespace.StartsWith("Tandem") && type.Name.Contains("Controller"));

            foreach (Type ctrlClass in controllerClasses)
            {
                List<MethodInfo> methods = ctrlClass.GetMethods().ToList();
                methods.RemoveAll(method => method.DeclaringType.Namespace.StartsWith("Microsoft"));

                foreach (MemberInfo method in methods)
                {
                    NoToken tokenAttr = (NoToken)Attribute.GetCustomAttribute(method, typeof(NoToken));
                    if (tokenAttr != null)
                    {
                        string path = ctrlClass.Name[..ctrlClass.Name.IndexOf("Controller")];
                        PathsToExclude.Add($"^/{path}/{tokenAttr.EndpointName}$");
                    }
                }
            }

            //DIAGNOSTICS
            //System.Diagnostics.Debug.WriteLine(null);
            //System.Diagnostics.Debug.WriteLine("Found the following endpoints with the [NoToken] attr");
            //PathsToExclude.ToList().ForEach(pte => System.Diagnostics.Debug.WriteLine(pte));
            //System.Diagnostics.Debug.WriteLine(null);
        }
    }
}
