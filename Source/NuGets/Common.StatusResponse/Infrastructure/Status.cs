using System;

namespace Tandem.Common.StatusResponse.Infrastructure
{
    public static class Status
    {
        #region 200 SERIES RESPONSE CODES
        public enum Status200
        {
            Success = 200
        }
        #endregion

        #region 400-SERIES BUSINESS RULE CODES
        public enum Status400
        {
        }
        #endregion

        #region 500-SERIES ERROR CODES
        public enum Status500
        {
            FatalError = 555
        }
        #endregion

        #region 900-SERIES MISC CODES
        public enum Status900
        {
            UnknownCode = 999
        }
        #endregion

        public enum StatusMessage
        {
            //200 CODE MESSAGES
            /// <summary> Success </summary>
            Success,

            //500 CODE MESSAGES
            /// <summary> "A fatal error has occurred" </summary>
            FatalError,

            //900 CODE MESSAGES
            /// <summary> "An error occurred for which an internal code could not be identified" </summary>
            UnknownCode
        }

        public static string GetValue(this StatusMessage statusMessage)
        {
            string message = statusMessage switch
            {
                //200 CODES
                StatusMessage.Success => "Success",

                //500 CODES
                StatusMessage.FatalError => "A fatal error has occurred",

                //900 CODES
                StatusMessage.UnknownCode => "An error occurred for which an internal code could not be identified",

                //DEFAULT CASE
                _ => null
            };
            return message;
        }

        private static string GetName<TEnum>(this TEnum @enum) where TEnum : Enum
        {
            string name = Enum.GetName(typeof(TEnum), @enum);
            return name;
        }
    }
}