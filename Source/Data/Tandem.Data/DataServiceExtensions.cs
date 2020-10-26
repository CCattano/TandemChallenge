using Microsoft.Extensions.DependencyInjection;

namespace Tandem.Web.Apps.Trivia.Data
{
    public static class DataServiceExtensions
    {
        public static void AddTriviaDataService(this IServiceCollection services, string dataFileLocation)
        {
            services.AddScoped<ITriviaDataService, TriviaDataService>(provider => new TriviaDataService(dataFileLocation));
        }
    }
}