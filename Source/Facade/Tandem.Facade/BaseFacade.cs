using Tandem.Web.Apps.Trivia.Data;

namespace Tandem.Web.Apps.Trivia.Facade
{
    public abstract class BaseFacade
    {
        protected BaseFacade(ITriviaDataService dataSvc)
        {
            DataSvc = dataSvc;
        }

        protected readonly ITriviaDataService DataSvc;
    }
}
