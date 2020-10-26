using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data
{
    public interface ITriviaDataService
    {
        IQuestionRepo QuestionRepo { get; }
        IAnswerRepo AnswerRepo { get; }
        IPlayerRepo PlayerRepo { get; }
        IPlayerAnswerRepo PlayerAnswerRepo { get; }
        IPlayerHistory PlayerHistory { get; }
    }
}