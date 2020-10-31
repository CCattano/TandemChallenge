using System.Collections.Generic;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;

namespace Tandem.Web.Apps.Trivia.Facade.Contracts
{
    public interface ITriviaFacade
    {
        Task<List<QuestionBE>> GetAllQuestions();
        Task<List<AnswerBE>> GetAllAnswers();
    }
}
