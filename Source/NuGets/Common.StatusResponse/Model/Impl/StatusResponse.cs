using System;
using System.Collections.Generic;
using Tandem.Common.StatusResponse.Infrastructure;
using Tandem.Common.StatusResponse.Model.Contracts;

namespace Tandem.Common.StatusResponse.Model.Impl
{
    public class StatusResponse : IStatusResponse
    {
        public StatusResponse()
        {
            StatusCode = (int)Status.Status200.Success;
            StatusDesc = Status.StatusMessage.Success.GetValue();
            StatusDetails = null;
        }

        public int StatusCode { get; set; }
        public string StatusDesc { get; set; }
        public List<StatusDetail> StatusDetails { get; set; }
    }
}
