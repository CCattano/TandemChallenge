using System.Threading.Tasks;
using Tandem.Web.App.ViewModels.ViewModels;
using Tandem.Web.Apps.Facade.Contracts;

namespace Tandem.Web.Apps.Facade.Impl
{
    public class CommonFacade : ICommonFacade
    {
        public CommonFacade()
        {
        }

        public async Task<BaseViewModel> E2ETest() =>
            await Task.FromResult(new BaseViewModel() { Success = true });
    }
}