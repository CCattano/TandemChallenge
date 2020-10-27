using AutoMapper;
using System;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Data;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Facade.Contracts;
using Tandem.Web.Apps.Trivia.Infrastructure;

namespace Tandem.Web.Apps.Trivia.Facade.Impl
{
    public class PlayerFacade : BaseFacade, IPlayerFacade
    {
        public PlayerFacade(ITriviaDataService dataSvc, IMapper mapper) : base(dataSvc, mapper)
        {
        }

        public async Task<PlayerBE> GetPlayerByNameHash(string nameHash)
        {
            PlayerEntity playerEntity = await base.DataSvc.PlayerRepo.GetByNameHashAsync(nameHash);
            PlayerBE playerBE = playerEntity != null ? Mapper.Map<PlayerBE>(playerEntity) : null;
            return playerBE;
        }

        public async Task<PlayerBE> GetPlayerByPlayerID(int playerID)
        {
            PlayerEntity playerEntity = await base.DataSvc.PlayerRepo.GetByPlayerIDAsync(playerID);
            PlayerBE playerBE = playerEntity != null ? Mapper.Map<PlayerBE>(playerEntity) : null;
            return playerBE;
        }

        public async Task InsertNewPlayer(PlayerBE newPlayer)
        {
            PlayerEntity playerEntity = base.Mapper.Map<PlayerEntity>(newPlayer);
            playerEntity.CreatedBy = playerEntity.LastModifiedBy = SystemConstants.DefaultUser;
            playerEntity.CreatedDateTime = playerEntity.LastModifiedDateTime = DateTime.UtcNow;

            await base.DataSvc.PlayerRepo.InsertAsync(playerEntity);

            newPlayer.PlayerID = playerEntity.PlayerID;
        }

        public async Task<PlayerBE> UpdatePlayer(PlayerBE playerBE)
        {
            PlayerEntity playerEntity = base.Mapper.Map<PlayerEntity>(playerBE);
            playerEntity.LastModifiedBy = SystemConstants.DefaultUser;
            playerEntity.LastModifiedDateTime = DateTime.UtcNow;

            bool updatedSuccessfully = await base.DataSvc.PlayerRepo.UpdateAsync(playerEntity);

            if (!updatedSuccessfully) return null;

            PlayerBE updatedPlayerBE = base.Mapper.Map<PlayerBE>(playerEntity);
            return updatedPlayerBE;
        }
    }
}