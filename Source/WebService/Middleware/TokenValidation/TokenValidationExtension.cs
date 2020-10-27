using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation.TokenValidationResources;

namespace Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation
{
    public static class TokenValidationExtension
    {
        private static readonly List<string> PathsToExclude = new List<string>()
        {
            @"^/swagger"
        };

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
