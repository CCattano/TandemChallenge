﻿using System.Threading.Tasks;
using Tandem.Common.DataProxy.Contracts;
using Tandem.Web.Apps.Trivia.Data.Entities;

namespace Tandem.Web.Apps.Trivia.Data.Repos.Contracts
{
    public interface IPlayerRepo : IBaseRepository<PlayerEntity>
    {
        Task<PlayerEntity> GetByNameHashAsync(string playerNameHash);
        Task<PlayerEntity> GetByPlayerIDAsync(int playerID);
    }
}
