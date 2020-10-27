using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;

namespace Tandem.Web.Apps.Trivia.Adapter.Contracts
{
    public interface IPlayerAdapter
    {
        Task<PlayerBE> GetPlayerByName(string playerName);
        Task<bool> NameIsAvailable(string playerName);
        Task<PlayerBE> CreateAccount(string username, string password);
    }
}
