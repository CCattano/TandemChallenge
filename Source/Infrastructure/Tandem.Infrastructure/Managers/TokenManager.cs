using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Tandem.Web.Apps.Trivia.Infrastructure.Managers
{
    public static class TokenManager
    {

        private const string HMAC_SHA256_HashKey =
            @"DB63B443964E459B93628E4975BA0D13" +
            @"A97C8B1022CE40D58E0548CC61A4583D" +
            @"1BA1CA78ED3B448697FED78C7E77DE24" +
            @"E34596307C4A44FEA7EA7C59491F171A" +
            @"A86CC1B626E7453C9A532BE35569843C" +
            @"6781BB40D5E34541A706E46DE39372A2" +
            @"A643C2DC978D4AC4A25270793370CAB9" +
            @"A34584F6C1B24A12878A3429A91269FA" +
            @"CB2342339BB14D36ABF9D25CCBE9C144" +
            @"8673BEAC19434560ACEE37CB5CF1E96B";
        private static readonly byte[] HashingKey = Encoding.UTF8.GetBytes(HMAC_SHA256_HashKey);

        private const string SigningSecret =
            @"D273D5C032DA446AA28F77223592B735" +
            @"AAED30BB82F142199DA1C78C07CC46F7" +
            @"1D42F133403A42A6A79695FB686EE05D" +
            @"CB4A4E02D03844A5976D633B5CAFE8EF" +
            @"047BEC1ED02042A89011F491C408D2B2" +
            @"B3AADB6D6B3B4FEC92BA4D1CE48820DF" +
            @"15323C56B60344968F85105747690017" +
            @"2DE490ED20A8429C9C982B11725DF6BE" +
            @"7A31B395DDFF459E928F3A02D408AF2D" +
            @"D2DBFA03C3E1406E874641C7F913F129";

        public const string RequestHeaderKey = @"TandemToken";

        public static string GenerateLoginToken(int playerID, DateTime tokenTTL)
        {
            //Setup Header
            TokenHeader header = new TokenHeader();
            string jwtHeader = JsonSerializer.Serialize(header);
            byte[] headerBytes = Encoding.UTF8.GetBytes(jwtHeader);
            string encodedHeader = Base64UrlEncoder.Encode(headerBytes);

            //Setup data body
            TokenBody body = new TokenBody(playerID, tokenTTL);
            string jwtBody = JsonSerializer.Serialize(body);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(jwtBody);
            string encodedBody = Base64UrlEncoder.Encode(bodyBytes);

            string jwtToken = $"{encodedHeader}.{encodedBody}";

            string jwtSignature = GetTokenSignature(jwtToken);
            string encodedSignature = Base64UrlEncoder.Encode(jwtSignature);

            string signedJWTToken = $"{jwtToken}.{encodedSignature}";

            return signedJWTToken;
        }

        public static bool ValidateTokenSignature(string jwtToken)
        {
            //Token anatomy is "encodedHeader.encodedBody.encodedSignature"
            string[] tokenParts = jwtToken.Split('.');
            string encodedToken = $"{tokenParts[0]}.{tokenParts[1]}";
            string encodedSignature = tokenParts[2];

            string decodedSignature = Base64UrlEncoder.Decode(encodedSignature);

            string recreatedSignature = GetTokenSignature(encodedToken);

            bool validToken = recreatedSignature.Equals(decodedSignature);

            return validToken;
        }

        public static bool TokenIsExpired(string jwtToken)
        {
            //Token anatomy is "encodedHeader.encodedBody.encodedSignature"
            string[] tokenParts = jwtToken.Split('.');
            string encodedTokenBody = tokenParts[1];

            string decodedTokenBody = Base64UrlEncoder.Decode(encodedTokenBody);
            TokenBody jwtTokenBody = JsonSerializer.Deserialize<TokenBody>(decodedTokenBody);

            bool isExpired = DateTime.UtcNow > jwtTokenBody.ExpirationDateTime;
            return isExpired;
        }

        #region PRIVATE HELPER METHODS
        private static string GetTokenSignature(string token)
        {
            string jwtSignature = string.Empty;
            foreach (string value in new[] { token, SigningSecret })
            {
                jwtSignature += value;
                string hash = GetHash(value);
                jwtSignature = hash;
            }
            return jwtSignature;
        }

        private static string GetHash(string value)
        {
            HMACSHA256 encryptor = new HMACSHA256(HashingKey);
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

        #region PRIVATE CLASS RESOURCES

        private class TokenHeader
        {
            public string Algorithm { get; } = "SHA256";
            public string Type { get; } = "JWT";
        }

        private class TokenBody
        {
            public TokenBody()
            {
            }

            public TokenBody(int playerID, DateTime expirationDateTime) =>
                (PlayerID, ExpirationDateTime) = (playerID, expirationDateTime);

            public int PlayerID { get; set; }
            public DateTime ExpirationDateTime { get; set; }
        }
        #endregion
    }

}