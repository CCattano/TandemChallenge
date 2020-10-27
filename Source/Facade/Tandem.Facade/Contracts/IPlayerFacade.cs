using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;

namespace Tandem.Web.Apps.Trivia.Facade.Contracts
{
    public interface IPlayerFacade
    {
        Task<PlayerBE> GetPlayerByNameHash(string nameHash);
        Task<PlayerBE> GetPlayerByPlayerID(int playerID);
        Task InsertNewPlayer(PlayerBE newPlayer);
        Task<PlayerBE> UpdatePlayer(PlayerBE playerBE);
    }
}
