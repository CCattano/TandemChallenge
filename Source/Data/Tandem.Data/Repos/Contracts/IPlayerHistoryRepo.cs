using System.Collections.Generic;
using System.Threading.Tasks;
using Tandem.Common.DataProxy.Contracts;
using Tandem.Web.Apps.Trivia.Data.Entities;

namespace Tandem.Web.Apps.Trivia.Data.Repos.Contracts
{
    public interface IPlayerHistoryRepo : IBaseRepository<PlayerHistoryEntity>
    {
        Task<PlayerHistoryEntity> GetByIDAsync(int playerHistoryID);
        Task<List<PlayerHistoryEntity>> GetAllByPlayerIDAsync(int playerID);
    }
}
