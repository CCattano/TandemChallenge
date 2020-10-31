using AutoMapper;
using System;
using System.Linq;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia.Composite;
using Tandem.Web.Apps.Trivia.BusinessModels.Trivia;
using Tandem.Web.Apps.Trivia.BusinessModels.Trivia.Composite;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player.Composite
{
    public class QuestionDetail_QuestionDetailBE : ITypeConverter<QuestionDetail, QuestionDetailBE>, ITypeConverter<QuestionDetailBE, QuestionDetail>
    {
        public QuestionDetail Convert(QuestionDetailBE source, QuestionDetail destination, ResolutionContext context)
        {
            QuestionDetail result = destination ?? new QuestionDetail();
            result.QuestionID = source.QuestionID;
            result.Text = source.Text;
            result.Answers = source.Answers?.Select(a => context.Mapper.Map<Answer>(a)).ToList();
            result.QuestionSequence = source.QuestionSequence;
            return result;
        }

        public QuestionDetailBE Convert(QuestionDetail source, QuestionDetailBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
