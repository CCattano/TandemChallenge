using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Common.DataProxy;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data.Repos
{
    public class QuestionRepo : BaseRepository, IQuestionRepo
    {
        public QuestionRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        protected override string DataFileName => "Question.json";

        public async Task<bool> InsertAsync(QuestionEntity entity)
        {
            QuestionEntity lastQuestion = (await GetAsync())?.OrderByDescending(q => q.QuestionID)?.FirstOrDefault();
            entity.QuestionID = lastQuestion?.QuestionID ?? 0 + 1;
            bool response = await base.InsertAsync(entity);
            return response;
        }

        public async Task<List<QuestionEntity>> GetAsync()
        {
            List<QuestionEntity> response = await GetAsync<QuestionEntity>();
            return response;
        }

        public async Task<bool> UpdateAsync(QuestionEntity entity)
        {
            List<QuestionEntity> questions = await GetAsync();
            bool response = await base.UpdateAsync(questions);
            return response;
        }
    }
}
