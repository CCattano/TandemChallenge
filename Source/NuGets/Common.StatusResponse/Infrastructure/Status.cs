using System;

namespace Tandem.Common.StatusResponse.Infrastructure
{
    public static class Status
    {
        //SUCCESS RESPONSE CODES
        public enum Status200
        {
            Success = 200
        }

        //ERROR RESPONSE CODES
        public enum Status300
        {
            //300-305 reserved for Token errors
            TandemTokenNotFound = 300,
            TandemTokenNotValid = 301,
            TandemTokenExpired = 302,
            //306-311 reserved for Player errors
            PlayerNotFoundByID = 306
        }

        //BUSINESS RULE CODES
        public enum Status400
        {
            //400-410 reserved for Account Validations
            IncorrectUsernamePassword = 400,
        }

        //ERROR CODES
        public enum Status500
        {
            BadRequest = 530,
            BusinessError = 540,
            FatalError = 550,
            RetryError = 560
        }

        //PROCESS CODE
        public enum Status700
        {
            //700-710 reserved for Db processes
            DbUpdateError = 700
        }

        //MISC CODES
        public enum Status900
        {
            UnknownCode = 999
        }

        //STATUS CODE MESSAGES
        public enum StatusMessage
        {
            #region 200 CODE MESSAGES

            /// <summary> Success </summary>
            Success,

            #endregion

            #region 300 CODE MESSAGES
            /// <summary> A TandemToken was required for the request made and could not be found </summary>
            TandemTokenNotFound,
            /// <summary> The TandemToken provided was not valid </summary>
            TandemTokenNotValid,
            /// <summary> The TandemToken provided has expired. A new token needs to be requested via Login </summary>
            TandemTokenExpired,
            /// <summary> A player could not be found for the PlayerID provided </summary>
            PlayerNotFoundByID,
            #endregion

            #region 400 CODE MESSAGES

            /// <summary> Incorrect username or password specified </summary>
            IncorrectUsernamePassword,

            #endregion

            #region 500 CODE MESSAGES

            /// <summary> The request made was not a valid request </summary>
            BadRequest,
            /// <summary> A Business Validation Error Occurred </summary>
            BusinessError,
            /// <summary> A fatal error has occurred </summary>
            FatalError,
            /// <summary> An error has occurred processing a request. Please try again </summary>
            RetryError,

            #endregion

            #region 700 CODE MESSAGES

            /// <summary> An error occurred trying to update a Db entity </summary>
            DbUpdateError,

            #endregion

            #region 900 CODE MESSAGES

            /// <summary> An error occurred for which an internal code could not be identified </summary>
            UnknownCode

            #endregion
        }

        #region STATUS ENUM EXTENSIONS

        public static string GetValue(this StatusMessage statusMessage)
        {
            string message = statusMessage switch
            {
                //200 CODES
                StatusMessage.Success => "Success",

                //300 CODES
                StatusMessage.TandemTokenNotFound => "A TandemToken was required for the request made and could not be found",
                StatusMessage.TandemTokenNotValid => "The TandemToken provided was not valid",
                StatusMessage.TandemTokenExpired => " TandemToken provided has expired. A new token needs to be requested via Login",
                StatusMessage.PlayerNotFoundByID => "A player could not be found for the PlayerID provided",

                //400 CODES
                StatusMessage.IncorrectUsernamePassword => "Incorrect username or password specified",

                //500 CODES
                StatusMessage.BadRequest => "The request made was not a valid request",
                StatusMessage.BusinessError => "A Business Validation Error Occurred",
                StatusMessage.FatalError => "A fatal error has occurred",
                StatusMessage.RetryError => "An error has occurred processing a request. Please try again",

                //700 CODES
                StatusMessage.DbUpdateError => "An error occurred trying to update a Db entity",

                //900 CODES
                StatusMessage.UnknownCode => "An error occurred for which an internal code could not be identified",

                //DEFAULT CASE
                _ => null
            };
            return message;
        }

        public static int ToInt32(this Status200 @enum) => ToInt32<Status200>(@enum);
        public static int ToInt32(this Status300 @enum) => ToInt32<Status300>(@enum);
        public static int ToInt32(this Status400 @enum) => ToInt32<Status400>(@enum);
        public static int ToInt32(this Status500 @enum) => ToInt32<Status500>(@enum);
        public static int ToInt32(this Status700 @enum) => ToInt32<Status700>(@enum);
        public static int ToInt32(this Status900 @enum) => ToInt32<Status900>(@enum);
        private static int ToInt32<TEnum>(TEnum @enum) where TEnum : Enum =>
            (int)Convert.ChangeType(@enum, @enum.GetTypeCode());

        #endregion
    }
}