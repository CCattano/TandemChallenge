using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Tandem.Common.StatusResponse.Infrastructure;
using Tandem.Common.StatusResponse.Model.Contracts;
using Tandem.Common.StatusResponse.Model.Impl;
using SRR = Tandem.Common.StatusResponse.Infrastructure.StatusResponseResources;

namespace Tandem.Common.StatusResponse.Services
{
    public class StatusResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public StatusResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStatusResponse statusResponse)
        {
            //Set IStatusResponse on response headers at end of request session
            context.Response.OnStarting(state =>
            {
                string statusRespStr = JsonSerializer.Serialize(statusResponse);
                context.Response.Headers.Add(SRR.HeaderKey, statusRespStr);
                return Task.CompletedTask;
            }, context);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //Typically would log whatever exception was thrown to a permanent place
                //Low-key just didn't want to impl a Logger NuGet .csproj that worked with a Logger.json file
                List<StatusDetail> details = new List<StatusDetail>()
                {
                    new StatusDetail()
                    {
                        Code = Status.Status900.UnknownCode.ToInt32(),
                        Desc = Status.StatusMessage.UnknownCode.GetValue()
                    },
                    new StatusDetail()
                    {
                        Code = Status.Status900.UnknownCode.ToInt32(),
                        Desc = ex.Message
                    }
                };
                statusResponse.SetStatusResponse(Status.Status500.FatalError, Status.StatusMessage.FatalError, details);
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                context.Response.ContentLength = 0;
                context.Response.Body = Stream.Null;
            }
        }
    }
}
