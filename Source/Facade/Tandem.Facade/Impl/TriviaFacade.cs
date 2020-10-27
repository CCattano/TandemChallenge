﻿using AutoMapper;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.Data;
using Tandem.Web.Apps.Trivia.Facade.Contracts;

namespace Tandem.Web.Apps.Trivia.Facade.Impl
{
    public class TriviaFacade : BaseFacade, ITriviaFacade
    {
        public TriviaFacade(ITriviaDataService dataSvc, IMapper mapper) : base(dataSvc, mapper)
        {
        }

        public async Task<bool> E2ETest() => await DataSvc.QuestionRepo.E2ETest(); //TEST
    }
}