using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tandem.Common.StatusResponse.Model.Contracts;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Facade.Contracts;

namespace Tandem.Web.Apps.Trivia.Adapter.Impl
{
    public class PlayerAdapter : BaseAdapter<IPlayerFacade>, IPlayerAdapter
    {
        public PlayerAdapter(IPlayerFacade facade, IStatusResponse statusResp) : base(facade, statusResp)
        {
        }

        public async Task<PlayerBE> GetPlayerByName(string playerName)
        {
            string nameHash = GetHash(playerName.ToLower());
            PlayerBE playerBE = await Facade.GetPlayerByNameHash(nameHash);
            return playerBE;
        }

        public async Task<bool> NameIsAvailable(string playerName)
        {
            PlayerBE playerBE = await GetPlayerByName(playerName);
            bool isAvailable = playerBE == null;
            return isAvailable;
        }

        public async Task<PlayerBE> CreateAccount(string username, string password)
        {
            string salt = Guid.NewGuid().ToString("N");
            string pepper = Guid.NewGuid().ToString("N");
            string passwordHash = string.Empty;
            foreach(string value in new[] { password, salt, pepper })
            {
                passwordHash += value;
                string hash = GetHash(value);
                passwordHash = hash;
            }
            PlayerBE newPlayer = new PlayerBE()
            {
                PasswordSalt = salt,
                PasswordPepper = pepper,
                PasswordHash = passwordHash,
                NameHash = GetHash(username.ToLower()),
                Name = username,
                LoginToken = GenerateLoginToken(username),
                LoginTokenExpireDateTime = DateTime.UtcNow.AddDays(14)
            };
            await Facade.CreateAccount(newPlayer);
            return newPlayer;
        }

        public Task<PlayerBE> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        private string GenerateLoginToken(string username)
        {
            string loginToken = string.Empty;
            foreach (string value in new[] { username, DateTime.UtcNow.Ticks.ToString() })
            {
                loginToken+= value;
                string hash = GetHash(value);
                loginToken = hash;
            }
            return loginToken.ToLower();
        }

        private string GetHash(string value)
        {
            SHA256 encryptor = SHA256.Create();
            byte[] unencryptedBytes = Encoding.UTF8.GetBytes(value);
            byte[] encryptedBytes = encryptor.ComputeHash(unencryptedBytes);
            string hash = string.Empty;
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                hash += encryptedBytes[i].ToString("X2");
            }
            return hash;
        }
    }
}
