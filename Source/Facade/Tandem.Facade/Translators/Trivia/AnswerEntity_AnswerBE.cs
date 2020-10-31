using AutoMapper;
using System;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.Data.Entities;

namespace Tandem.Web.Apps.Trivia.Facade.Translators.Trivia
{
    public class AnswerEntity_AnswerBE : ITypeConverter<AnswerEntity, AnswerBE>, ITypeConverter<AnswerBE, AnswerEntity>
    {
        public AnswerEntity Convert(AnswerBE source, AnswerEntity destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }

        public AnswerBE Convert(AnswerEntity source, AnswerBE destination, ResolutionContext context)
        {
            AnswerBE result = destination ?? new AnswerBE();
            result.AnswerID = source.AnswerID;
            result.QuestionID = source.QuestionID;
            result.Text = source.Text;
            result.IsCorrect = source.IsCorrect;
            return result;
        }
    }
}