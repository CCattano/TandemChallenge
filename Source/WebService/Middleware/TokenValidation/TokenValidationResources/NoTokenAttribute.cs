using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation.TokenValidationResources
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class NoToken : Attribute
    {
        public NoToken([CallerMemberName]string endpointName = null)
        {
            EndpointName = endpointName;
        }
        public readonly string EndpointName;
        /*
         * Placed on Controller endpoints that do not require a Token
         * to be in the HttpContext.Request.Header
         * The TokenValidationMiddleware will be registered with the IAppBuilder
         * to only invoke the middleware for endpoints that are not afixed
         * with the [NoToken] attribute
         */
    }
}