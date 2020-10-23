using System.Threading.Tasks;
using Tandem.Web.App.ViewModels.ViewModels;

namespace Tandem.Web.Apps.Facade.Contracts
{
    public interface ICommonFacade
    {
        Task<BaseViewModel> E2ETest();
    }
}
