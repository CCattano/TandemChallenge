using Tandem.Common.DataProxy;
using Tandem.Web.Apps.Trivia.Data.Repos;
using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data
{
    public class TriviaDataService : BaseDataService, ITriviaDataService
    {
        private IQuestionRepo _questionRepo;
        private IAnswerRepo _answerRepo;
        private IPlayerRepo _playerRepo;
        private IPlayerAnswerRepo _playerAnswerRepo;
        private IPlayerQuestionRepo _playerQuestionRepo;
        private IPlayerHistoryRepo _playerHistoryRepo;

        public TriviaDataService(string dataFileLoc) : base(dataFileLoc)
        {
        }

        public IQuestionRepo QuestionRepo { get => _questionRepo ??= new QuestionRepo(this); }
        public IAnswerRepo AnswerRepo { get => _answerRepo ??= new AnswerRepo(this); }
        public IPlayerRepo PlayerRepo { get => _playerRepo ??= new PlayerRepo(this); }
        public IPlayerAnswerRepo PlayerAnswerRepo { get => _playerAnswerRepo ??= new PlayerAnswerRepo(this); }
        public IPlayerQuestionRepo PlayerQuestionRepo { get => _playerQuestionRepo ??= new PlayerQuestionRepo(this); }
        public IPlayerHistoryRepo PlayerHistoryRepo { get => _playerHistoryRepo ??= new PlayerHistoryRepo(this); }
    }
}