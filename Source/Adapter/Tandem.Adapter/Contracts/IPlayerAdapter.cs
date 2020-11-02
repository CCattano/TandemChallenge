using System.Collections.Generic;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;

namespace Tandem.Web.Apps.Trivia.Adapter.Contracts
{
    public interface IPlayerAdapter
    {
        Task<bool> NameIsAvailable(string playerName);
        Task<string> CreateAccount(string username, string password);
        Task<string> Login(string username, string password);
        Task<PlayerBE> GetPlayerByID(int playerID);
        Task SavePlayerAnswer(PlayerAnswerBE playerAnswer);
        Task MarkRoundCompleted(int playerHistoryID);
        Task<List<PlayerHistoryBE>> GetAllPlayerHistory(int playerID);
        Task<string> ChangeUsername(int playerID, string newUsername);
        Task ChangePassword(int playerID, string currentPassword, string newPassword);
    }
}
