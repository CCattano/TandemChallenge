using AutoMapper;
using System;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Data;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Facade.Contracts;

namespace Tandem.Web.Apps.Trivia.Facade.Impl
{
    public class PlayerFacade : BaseFacade, IPlayerFacade
    {
        public PlayerFacade(ITriviaDataService dataSvc, IMapper mapper) : base(dataSvc, mapper)
        {
        }

        public async Task<PlayerBE> GetPlayerByNameHash(string nameHash)
        {
            PlayerEntity playerEntity = await base.DataSvc.PlayerRepo.GetPlayerByNameHash(nameHash);
            PlayerBE playerBE = playerEntity != null ? Mapper.Map<PlayerBE>(playerEntity) : null;
            return playerBE;
        }

        public async Task CreateAccount(PlayerBE newPlayer)
        {
            PlayerEntity playerEntity = base.Mapper.Map<PlayerEntity>(newPlayer);
            playerEntity.CreatedBy = playerEntity.LastModifiedBy = "";
            playerEntity.CreatedDateTime = playerEntity.LastModifiedDateTime = DateTime.UtcNow;
            await base.DataSvc.PlayerRepo.InsertAsync(playerEntity);
            newPlayer.PlayerID = playerEntity.PlayerID;
        }
    }
}