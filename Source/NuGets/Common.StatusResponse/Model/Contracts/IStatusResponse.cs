using System;
using System.Collections.Generic;
using Tandem.Common.StatusResponse.Infrastructure;
using Tandem.Common.StatusResponse.Model.Impl;

namespace Tandem.Common.StatusResponse.Model.Contracts
{
    public interface IStatusResponse
    {
        int StatusCode { get; set; }
        string StatusDesc { get; set; }
        List<StatusDetail> StatusDetails { get; set; }
    }

    public static class StatusResponseExtensions
    {
        /* IMPL NOTES
         *
         * The goal of these overrides is to add type safety to the way in which
         * int and string values are applied to the IStatusResponse obj by
         * leveraging enums and handling the conversion of enum values to ints/strings
         * in a centralized place so in the future you don't have diff devs doing conversions
         * in diff ways assigning random values that hold no meaning.
         *
         * This isolates the way in which status are set so they are set in a consistent
         * way time after time
         */
        //Status200 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status.Status200 status,
            Status.StatusMessage statusMsg,
            List<StatusDetail> statusDetails = null) => SetStatusResponse<Status.Status200>(statusResponse, status, statusMsg, statusDetails);

        //Status400 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status.Status400 status,
            Status.StatusMessage statusMsg,
            List<StatusDetail> statusDetails = null) => SetStatusResponse<Status.Status400>(statusResponse, status, statusMsg, statusDetails);

        //Status500 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status.Status500 status,
            Status.StatusMessage statusMsg,
            List<StatusDetail> statusDetails = null) => SetStatusResponse<Status.Status500>(statusResponse, status, statusMsg, statusDetails);

        //Status900 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status.Status900 status,
            Status.StatusMessage statusMsg,
            List<StatusDetail> statusDetails = null) => SetStatusResponse<Status.Status900>(statusResponse, status, statusMsg, statusDetails);

        #region PRIVATE HELPER METHOD
        private static void SetStatusResponse<TEnum>(IStatusResponse statusResponse,
            TEnum status, Status.StatusMessage statusMsg, List<StatusDetail> statusDetails = null) where TEnum : Enum
        {
            statusResponse.StatusCode = (int)Convert.ChangeType(status, status.GetTypeCode());
            statusResponse.StatusDesc = statusMsg.GetValue();
            statusResponse.StatusDetails = statusDetails;
        }
        #endregion
    }
}