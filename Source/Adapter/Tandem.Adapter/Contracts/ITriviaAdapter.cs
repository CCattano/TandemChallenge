using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player.Composite;

namespace Tandem.Web.Apps.Trivia.Adapter.Contracts
{
    public interface ITriviaAdapter
    {
        Task<PlayerRoundBE> GetTriviaRound(int? playerID);
    }
}