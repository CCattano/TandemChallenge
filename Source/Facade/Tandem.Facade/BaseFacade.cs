using AutoMapper;
using Tandem.Web.Apps.Trivia.Data;

namespace Tandem.Web.Apps.Trivia.Facade
{
    public abstract class BaseFacade
    {
        protected BaseFacade(ITriviaDataService dataSvc, IMapper mapper)
        {
            DataSvc = dataSvc;
            Mapper = mapper;
        }

        protected readonly ITriviaDataService DataSvc;
        protected readonly IMapper Mapper;
    }
}
