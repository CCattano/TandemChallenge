﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tandem.Common.StatusResponse.Model.Contracts;
using Tandem.Common.StatusResponse.Model.Impl;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Facade.Contracts;
using static Tandem.Common.StatusResponse.Infrastructure.Status;
using TokenMan = Tandem.Web.Apps.Trivia.Infrastructure.Managers.TokenManager;

namespace Tandem.Web.Apps.Trivia.Adapter.Impl
{
    public class PlayerAdapter : BaseAdapter<IPlayerFacade>, IPlayerAdapter
    {
        public PlayerAdapter(IPlayerFacade facade, IStatusResponse statusResp) : base(facade, statusResp)
        {
        }

        public async Task<bool> NameIsAvailable(string playerName)
        {
            PlayerBE playerBE = await GetPlayerByName(playerName);
            bool isAvailable = playerBE == null;
            return isAvailable;
        }

        public async Task<string> CreateAccount(string username, string password)
        {
            string salt = Guid.NewGuid().ToString("N");
            string pepper = Guid.NewGuid().ToString("N");
            string passwordHash = GetPasswordHash(password, salt, pepper);
            PlayerBE newPlayer = new PlayerBE()
            {
                PasswordSalt = salt,
                PasswordPepper = pepper,
                PasswordHash = passwordHash,
                NameHash = GetUsernameHash(username),
                Name = username,
                LoginTokenExpireDateTime = DateTime.UtcNow.AddDays(14)
            };
            await base.Facade.InsertNewPlayer(newPlayer);

            string loginToken =
                TokenMan.GenerateLoginToken(newPlayer.PlayerID, newPlayer.LoginTokenExpireDateTime);

            return loginToken;
        }

        public async Task<string> Login(string username, string password)
        {
            //The most common validation that will be violated in this method is the IncorrectUsernamePassword rule
            //Preset that statusDetail in preparation for if we have to set it
            List<StatusDetail> statusDetails = new List<StatusDetail>()
            {
                new StatusDetail()
                {
                    Code = Status400.IncorrectUsernamePassword.ToInt32(),
                    Desc = StatusMessage.IncorrectUsernamePassword.GetValue()
                }
            };

            PlayerBE player = await GetPlayerByName(username);

            //Player validation
            if (player == null)
            {
                base.StatusResp.SetStatusResponse(Status500.BusinessError, StatusMessage.BusinessError, statusDetails);
                return null;
            }

            //Password validation
            string passwordHash = GetPasswordHash(password, player.PasswordSalt, player.PasswordPepper);
            if (!passwordHash.Equals(player.PasswordHash))
            {
                base.StatusResp.SetStatusResponse(Status500.BusinessError, StatusMessage.BusinessError, statusDetails);
                return null;
            }

            //Made it this far then the login is valid.
            //Create a new login token, update the user with related token details and return the new token
            player.LoginTokenExpireDateTime = DateTime.UtcNow.AddDays(14);

            //Start this now while we generate the token, make sure to wait on it before leaving
            Task<PlayerBE> updatePlayerTask = Facade.UpdatePlayer(player);

            string jwtToken = TokenMan.GenerateLoginToken(player.PlayerID, player.LoginTokenExpireDateTime);

            //Update validation
            PlayerBE updatedPlayer = await updatePlayerTask;
            if (updatedPlayer == null)
            {
                statusDetails = new List<StatusDetail>()
                {
                    new StatusDetail()
                    {
                        Code = Status700.DbUpdateError.ToInt32(),
                        Desc = StatusMessage.DbUpdateError.GetValue()
                    }
                };
                base.StatusResp.SetStatusResponse(Status500.RetryError, StatusMessage.RetryError, statusDetails);
                return null;
            }

            return jwtToken;
        }

        public async Task<string> ChangeUsername(int playerID, string newUsername)
        {
            PlayerBE playerBE = await base.Facade.GetPlayerByPlayerID(playerID);
            playerBE.Name = newUsername;
            playerBE.NameHash = GetUsernameHash(newUsername);
            playerBE.LoginTokenExpireDateTime = DateTime.UtcNow.AddDays(14);
            await base.Facade.UpdatePlayer(playerBE);
            string newToken = TokenMan.GenerateLoginToken(playerID, playerBE.LoginTokenExpireDateTime);
            return newToken;
        }

        public async Task ChangePassword(int playerID, string currentPassword, string newPassword)
        {
            List<StatusDetail> statusDetails;

            PlayerBE playerBE = await base.Facade.GetPlayerByPlayerID(playerID);
            string newPassHash = GetPasswordHash(newPassword, playerBE.PasswordSalt, playerBE.PasswordPepper);
            //New password cannot be old password
            if(newPassHash == playerBE.PasswordHash)
            {
                statusDetails = new List<StatusDetail>()
                {
                    new StatusDetail()
                    {
                        Code = Status400.NewPasswordSamePassword.ToInt32(),
                        Desc = StatusMessage.NewPasswordSamePassword.GetValue()
                    }
                };
                base.StatusResp.SetStatusResponse(Status500.BusinessError, StatusMessage.BusinessError, statusDetails);
                return;
            }
            string currentPassHash = GetPasswordHash(currentPassword, playerBE.PasswordSalt, playerBE.PasswordPepper);
            //Current password was incorrect
            if(currentPassHash != playerBE.PasswordHash)
            {
                statusDetails = new List<StatusDetail>()
                {
                    new StatusDetail()
                    {
                        Code = Status400.IncorrectPassword.ToInt32(),
                        Desc = StatusMessage.IncorrectPassword.GetValue()
                    }
                };
                base.StatusResp.SetStatusResponse(Status500.BusinessError, StatusMessage.BusinessError, statusDetails);
                return;
            }
            //Everthing is good, perform the update
            string salt = Guid.NewGuid().ToString("N");
            string pepper = Guid.NewGuid().ToString("N");
            string passwordHash = GetPasswordHash(newPassword, salt, pepper);
            playerBE.PasswordSalt = salt;
            playerBE.PasswordPepper = pepper;
            playerBE.PasswordHash = passwordHash;
            await base.Facade.UpdatePlayer(playerBE);
        }

        public async Task<PlayerBE> GetPlayerByID(int playerID)
        {
            PlayerBE playerBE = await base.Facade.GetPlayerByPlayerID(playerID);
            if (playerBE == null)
            {
                List<StatusDetail> statusDetails = new List<StatusDetail>()
                {
                    new StatusDetail()
                    {
                        Code = Status300.PlayerNotFoundByID.ToInt32(),
                        Desc = StatusMessage.PlayerNotFoundByID.GetValue()
                    }
                };
                base.StatusResp.SetStatusResponse(Status500.BadRequest, StatusMessage.BadRequest, statusDetails);
            }
            return playerBE;
        }

        public async Task SavePlayerAnswer(PlayerAnswerBE playerAnswer)
        {
            await base.Facade.InsertPlayerAnswer(playerAnswer);
        }

        public async Task MarkRoundCompleted(int playerHistoryID)
        {
            PlayerHistoryBE historyBE = await base.Facade.GetPlayerHistory(playerHistoryID);
            historyBE.CompletedDateTime = DateTime.UtcNow;
            await base.Facade.UpdatePlayerHistory(historyBE);
        }

        public async Task<List<PlayerHistoryBE>> GetAllPlayerHistory(int playerID)
        {
            List<PlayerHistoryBE> historyBEs = await base.Facade.GetAllPlayerHistory(playerID);
            return historyBEs;
        }

        #region PRIVATE HELPER METHODS
        private async Task<PlayerBE> GetPlayerByName(string playerName)
        {
            string nameHash = GetUsernameHash(playerName);
            PlayerBE playerBE = await base.Facade.GetPlayerByNameHash(nameHash);
            return playerBE;
        }

        private string GetPasswordHash(string password, string salt, string pepper)
        {
            string passwordHash = string.Empty;
            foreach (string value in new[] { password, salt, pepper })
            {
                passwordHash += value;
                string hash = GetHash(passwordHash);
                passwordHash = hash;
            }
            return passwordHash;
        }

        private string GetUsernameHash(string username) => GetHash(username.ToLower());

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
        #endregion
    }
}