using AutoMapper;
using System;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.BusinessModels.Trivia;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Trivia
{
    public class Question_QuestionBE : ITypeConverter<Question, QuestionBE>, ITypeConverter<QuestionBE, Question>
    {
        public Question Convert(QuestionBE source, Question destination, ResolutionContext context)
        {
            Question result = destination ?? new Question();
            result.QuestionID = source.QuestionID;
            result.Text = source.Text;
            return result;
        }

        public QuestionBE Convert(Question source, QuestionBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
