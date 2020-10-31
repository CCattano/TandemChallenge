using AutoMapper;
using System;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.Data.Entities;

namespace Tandem.Web.Apps.Trivia.Facade.Translators.Trivia
{
    public class QuestionEntity_QuestionBE : ITypeConverter<QuestionEntity, QuestionBE>, ITypeConverter<QuestionBE, QuestionEntity>
    {
        public QuestionEntity Convert(QuestionBE source, QuestionEntity destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }

        public QuestionBE Convert(QuestionEntity source, QuestionBE destination, ResolutionContext context)
        {
            QuestionBE result = destination ?? new QuestionBE();
            result.QuestionID = source.QuestionID;
            result.Text = source.Text;
            return result;
        }
    }
}
