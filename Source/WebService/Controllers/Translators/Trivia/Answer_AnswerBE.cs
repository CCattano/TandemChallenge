using AutoMapper;
using System;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.BusinessModels.Trivia;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Trivia
{
    public class Answer_AnswerBE : ITypeConverter<Answer, AnswerBE>, ITypeConverter<AnswerBE, Answer>
    {
        public Answer Convert(AnswerBE source, Answer destination, ResolutionContext context)
        {
            Answer result = destination ?? new Answer();
            result.AnswerID = source.AnswerID;
            result.QuestionID = source.QuestionID;
            result.Text = source.Text;
            result.IsCorrect = source.IsCorrect;
            return result;
        }

        public AnswerBE Convert(Answer source, AnswerBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}