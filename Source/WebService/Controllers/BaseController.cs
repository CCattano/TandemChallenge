using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers
{
    public abstract class BaseController<TAdapter> : Controller where TAdapter : class
    {
        protected BaseController(TAdapter adapter, IMapper mapper)
        {
            Adapter = adapter;
            Mapper = mapper;
        }

        protected readonly TAdapter Adapter;
        protected readonly IMapper Mapper;
    }
}