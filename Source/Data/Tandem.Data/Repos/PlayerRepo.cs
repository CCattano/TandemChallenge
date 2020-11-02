using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Common.DataProxy;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data.Repos
{
    public class PlayerRepo : BaseRepository, IPlayerRepo
    {
        public PlayerRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        protected override string DataFileName => "Player.json";

        public async Task<bool> InsertAsync(PlayerEntity entity)
        {
            PlayerEntity lastPlayer = (await GetAsync())?.OrderByDescending(p => p.PlayerID)?.FirstOrDefault();
            entity.PlayerID = (lastPlayer?.PlayerID ?? 0) + 1;
            bool response = await base.InsertAsync(entity);
            return response;
        }

        public async Task<PlayerEntity> GetByNameHashAsync(string playerNameHash)
        {
            List<PlayerEntity> players = await GetAsync();
            PlayerEntity player = players?.SingleOrDefault(player => player.NameHash == playerNameHash);
            return player;
        }

        public async Task<PlayerEntity> GetByPlayerIDAsync(int playerID)
        {
            List<PlayerEntity> players = await GetAsync();
            PlayerEntity player = players?.SingleOrDefault(player => player.PlayerID == playerID);
            return player;
        }

        public async Task<List<PlayerEntity>> GetAsync()
        {
            List<PlayerEntity> response = await base.GetAsync<PlayerEntity>();
            return response;
        }

        public async Task<bool> UpdateAsync(PlayerEntity entity)
        {
            List<PlayerEntity> players = await GetAsync();
            PlayerEntity player = players.SingleOrDefault(player => player.PlayerID == entity.PlayerID);

            //PlayerEntity fields that support manipulation
            player.Name = entity.Name;
            player.NameHash = entity.NameHash;
            player.PasswordHash = entity.PasswordHash;
            player.PasswordSalt = entity.PasswordSalt;
            player.PasswordPepper = entity.PasswordPepper;
            player.LoginTokenExpireDateTime = entity.LoginTokenExpireDateTime;
            player.LastModifiedBy = entity.LastModifiedBy;
            player.LastModifiedDateTime = entity.LastModifiedDateTime;

            bool response = await base.UpdateAsync(players);
            return response;
        }
    }
}