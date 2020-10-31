using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.Data;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Facade.Contracts;

namespace Tandem.Web.Apps.Trivia.Facade.Impl
{
    public class TriviaFacade : BaseFacade, ITriviaFacade
    {
        public TriviaFacade(ITriviaDataService dataSvc, IMapper mapper) : base(dataSvc, mapper)
        {
        }

        public async Task<List<QuestionBE>> GetAllQuestions()
        {
            List<QuestionEntity> questionEntities = await DataSvc.QuestionRepo.GetAsync();
            List<QuestionBE> questionBEs = questionEntities?.Select(q => Mapper.Map<QuestionBE>(q)).ToList();
            return questionBEs;
        }

        public async Task<List<AnswerBE>> GetAllAnswers()
        {
            List<AnswerEntity> answerEntities = await DataSvc.AnswerRepo.GetAsync();
            List<AnswerBE> answerBEs = answerEntities?.Select(a => Mapper.Map<AnswerBE>(a)).ToList();
            return answerBEs;
        }
    }
}