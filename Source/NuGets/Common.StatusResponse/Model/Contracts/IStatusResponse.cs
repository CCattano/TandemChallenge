using System;
using System.Collections.Generic;
using Tandem.Common.StatusResponse.Infrastructure;
using Tandem.Common.StatusResponse.Model.Impl;
using static Tandem.Common.StatusResponse.Infrastructure.Status;

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
            Status200 status, StatusMessage statusMsg, List<StatusDetail> statusDetails = null) =>
            SetStatusResponse(statusResponse, status.ToInt32(), statusMsg, statusDetails);

        //Status300 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status300 status, StatusMessage statusMsg, List<StatusDetail> statusDetails = null) =>
            SetStatusResponse(statusResponse, status.ToInt32(), statusMsg, statusDetails);

        //Status400 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status400 status, StatusMessage statusMsg, List<StatusDetail> statusDetails = null) =>
            SetStatusResponse(statusResponse, status.ToInt32(), statusMsg, statusDetails);

        //Status500 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status500 status, StatusMessage statusMsg, List<StatusDetail> statusDetails = null) =>
            SetStatusResponse(statusResponse, status.ToInt32(), statusMsg, statusDetails);

        //Status700 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status700 status, StatusMessage statusMsg, List<StatusDetail> statusDetails = null) =>
            SetStatusResponse(statusResponse, status.ToInt32(), statusMsg, statusDetails);

        //Status900 override
        public static void SetStatusResponse(this IStatusResponse statusResponse,
            Status900 status, StatusMessage statusMsg, List<StatusDetail> statusDetails = null) =>
            SetStatusResponse(statusResponse, status.ToInt32(), statusMsg, statusDetails);

        #region PRIVATE HELPER METHOD
        private static void SetStatusResponse(IStatusResponse statusResponse,
            int statusCode, StatusMessage statusMsg, List<StatusDetail> statusDetails = null)
        {
            statusResponse.StatusCode = statusCode;
            statusResponse.StatusDesc = statusMsg.GetValue();
            statusResponse.StatusDetails = statusDetails;
        }
        #endregion
    }
}