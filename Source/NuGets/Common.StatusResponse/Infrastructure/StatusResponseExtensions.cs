using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tandem.Common.StatusResponse.Model.Contracts;
using Tandem.Common.StatusResponse.Services;
using SR = Tandem.Common.StatusResponse.Model.Impl;

namespace Tandem.Common.StatusResponse.Infrastructure
{
    public static class StatusResponseExtensions
    {
        public static void AddStatusResponse(this IServiceCollection services)
        {
            services.AddScoped<IStatusResponse, SR.StatusResponse>();
        }

        /// <summary>
        ///     Register this Middleware last so it is the first middleware to receive
        ///     a response from the service it is being added to.
        /// </summary>
        /// <param name="app"></param>
        public static void UseStatusResponseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<StatusResponseMiddleware>();
        }
    }
}
