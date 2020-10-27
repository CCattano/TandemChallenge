using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Common.StatusResponse.Model.Contracts;
using Tandem.Common.StatusResponse.Model.Impl;
using static Tandem.Common.StatusResponse.Infrastructure.Status;
using TokenMan = Tandem.Web.Apps.Trivia.Infrastructure.Managers.TokenManager;

namespace Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStatusResponse statusResp)
        {
            List<StatusDetail> statusDetails;
            //Get Token from header
            if (!context.Request.Headers.TryGetValue(TokenMan.RequestHeaderKey, out StringValues headerVal))
            {
                FailForMissingToken();
                return;
            }
            string token = headerVal.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(token))
            {
                FailForMissingToken();
                return;
            }

            //---Validations---
            //Signature validation
            bool isValid = TokenMan.ValidateTokenSignature(token);
            if (!isValid)
            {
                statusDetails = new List<StatusDetail>()
                {
                    new StatusDetail()
                    {
                        Code = Status300.TandemTokenNotValid.ToInt32(),
                        Desc = StatusMessage.TandemTokenNotValid.GetValue()
                    }
                };
                statusResp.SetStatusResponse(Status500.BadRequest, StatusMessage.BadRequest, statusDetails);
                return;
            }
            //Expired validation
            isValid = !TokenMan.TokenIsExpired(token);
            if (!isValid)
            {
                statusDetails = new List<StatusDetail>()
                {
                    new StatusDetail()
                    {
                        Code = Status300.TandemTokenNotValid.ToInt32(),
                        Desc = StatusMessage.TandemTokenNotValid.GetValue()
                    }
                };
                statusResp.SetStatusResponse(Status500.BadRequest, StatusMessage.BadRequest, statusDetails);
                return;
            }

            //Token valid, proceed
            await _next(context);

            //LOCAL HELPER FUNCTION
            void FailForMissingToken()
            {
                statusDetails = new List<StatusDetail>()
                {
                    new StatusDetail()
                    {
                        Code = Status300.TandemTokenNotFound.ToInt32(),
                        Desc = StatusMessage.TandemTokenNotFound.GetValue()
                    }
                };
                statusResp.SetStatusResponse(Status500.BadRequest, StatusMessage.BadRequest, statusDetails);
            }
        }
    }
}