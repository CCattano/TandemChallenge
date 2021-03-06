﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Common.DataProxy;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data.Repos
{
    public class AnswerRepo : BaseRepository, IAnswerRepo
    {
        public AnswerRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        protected override string DataFileName => "Answer.json";

        public async Task<bool> InsertAsync(AnswerEntity entity)
        {
            AnswerEntity lastAnswer = (await GetAsync())?.OrderByDescending(a => a.AnswerID)?.FirstOrDefault();
            entity.AnswerID = (lastAnswer?.AnswerID ?? 0) + 1;
            bool response = await base.InsertAsync(entity);
            return response;
        }

        public async Task<List<AnswerEntity>> GetAsync()
        {
            List<AnswerEntity> response = await base.GetAsync<AnswerEntity>();
            return response;
        }

        public async Task<bool> UpdateAsync(AnswerEntity entity)
        {
            List<AnswerEntity> answers = await GetAsync();
            bool response = await base.UpdateAsync(answers);
            return response;
        }
    }
}
