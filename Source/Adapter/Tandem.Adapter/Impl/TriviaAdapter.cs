using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Common.StatusResponse.Model.Contracts;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player.Composite;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia.Composite;
using Tandem.Web.Apps.Trivia.Facade.Contracts;

namespace Tandem.Web.Apps.Trivia.Adapter.Impl
{
    public class TriviaAdapter : BaseAdapter<ITriviaFacade>, ITriviaAdapter
    {
        private readonly IPlayerFacade _playerFacade;
        private readonly IMapper _mapper;
        public TriviaAdapter
        (
            ITriviaFacade triviaFacade,
            IPlayerFacade playerFacade,
            IStatusResponse statusResp,
            IMapper mapper
        ) : base(triviaFacade, statusResp)
        {
            _playerFacade = playerFacade;
            _mapper = mapper;
        }

        public async Task<PlayerRoundBE> GetTriviaRound(int? playerID)
        {
            bool isGuestRound = playerID == null;
            //Get all questions
            List<QuestionBE> questions = await base.Facade.GetAllQuestions();

            //Pick 10 random to keep
            Random rng = new Random();
            HashSet<int> questionsToUse = new HashSet<int>();
            while (questionsToUse.Count < 10)
                questionsToUse.Add(rng.Next(1, questions.Count + 1));
            questions.RemoveAll(q => !questionsToUse.Contains(q.QuestionID));
            questions = questions.OrderBy(q => rng.Next()).ToList(); //Shuffle
            rng = null;

            //For each question get their answers
            /* ANSWER RETRIEVAL NOTES
             *
             * This isn't ideal, but given the nature of the data source
             * The way in which we retrieve a single data obj by a identifier
             *
             * i.e. GetAnswersByQuestionID
             *
             * Is to pull the whole Answer data source, parse it to a List<Answer>
             * Then filter the list by the ID
             *
             * Rather than pull the entire source 10 times, once per question
             * I'm just gonna grab it now and parse it here
             */
            List<AnswerBE> answers = await base.Facade.GetAllAnswers();
            answers.RemoveAll(a => !questionsToUse.Contains(a.QuestionID));

            //Construct the initial response obj
            PlayerRoundBE roundBE = new PlayerRoundBE()
            {
                StartedDateTime = DateTime.UtcNow,
                QuestionDetails = questions.Select((q, i) => new QuestionDetailBE()
                {
                    QuestionID = q.QuestionID,
                    QuestionSequence = i + 1,
                    Text = q.Text,
                    Answers = answers.Where(a => a.QuestionID == q.QuestionID).ToList()
                }).ToList()
            };

            int roundNumber = 1;
            if (!isGuestRound)
            {
                roundBE.PlayerID = playerID.Value;
                roundNumber = await _playerFacade.GetRoundNumberByPlayerID(playerID.Value);
                roundBE.RoundNumber = roundNumber;

                //Insert history
                PlayerHistoryBE newPlayerHistory = _mapper.Map<PlayerHistoryBE>(roundBE);
                await _playerFacade.InsertNewHistory(newPlayerHistory);
                roundBE.PlayerHistoryID = newPlayerHistory.PlayerHistoryID;

                //Insert Questions
                foreach (QuestionDetailBE question in roundBE.QuestionDetails)
                {
                    PlayerQuestionBE playerQuestionBE = new PlayerQuestionBE()
                    {
                        PlayerHistoryID = roundBE.PlayerHistoryID,
                        QuestionID = question.QuestionID,
                        QuestionSequence = question.QuestionSequence
                    };
                    await _playerFacade.InsertNewPlayerQuestion(playerQuestionBE);
                }
            }
            else
            {
                roundBE.RoundNumber = roundNumber;
            }

            return roundBE;
        }

        public async Task<PlayerRoundBE> GetExistingRound(int playerHistoryID)
        {
            //Start running concurrent requests on separate threads
            Task<PlayerHistoryBE> getHistoryTask = _playerFacade.GetPlayerHistory(playerHistoryID);
            Task<List<QuestionBE>> getQuestionsTask = base.Facade.GetAllQuestions();
            Task<List<AnswerBE>> getAnswersTask = base.Facade.GetAllAnswers();
            Task<List<PlayerQuestionBE>> getPlayerQuestionsTask = _playerFacade.GetPlayerQuestions(playerHistoryID);
            Task<List<PlayerAnswerBE>> getPlayerAnswersTask = _playerFacade.GetPlayerAnswers(playerHistoryID);

            //Wait for all threads to finish
            await Task.WhenAll(getHistoryTask, getQuestionsTask, getAnswersTask, getPlayerQuestionsTask, getPlayerAnswersTask);

            //Get responses from each thread
            PlayerHistoryBE history = await getHistoryTask;
            List<QuestionBE> questions = await getQuestionsTask;
            List<AnswerBE> answers = await getAnswersTask;
            List<PlayerQuestionBE> playerQuestions = await getPlayerQuestionsTask;
            List<PlayerAnswerBE> playerAnswers = await getPlayerAnswersTask;

            //Trim down to necessary values
            List<int> questionsUsed = playerQuestions.Select(pq => pq.QuestionID).ToList();
            questions.RemoveAll(q => !questionsUsed.Contains(q.QuestionID));
            answers.RemoveAll(a => !questionsUsed.Contains(a.QuestionID));

            PlayerRoundBE roundBE = new PlayerRoundBE()
            {
                QuestionDetails = new List<QuestionDetailBE>()
            };
            roundBE = _mapper.Map(history, roundBE);
            roundBE.PlayerAnswers = playerAnswers;

            playerQuestions = playerQuestions.OrderBy(q => q.QuestionSequence).ToList();
            foreach (PlayerQuestionBE question in playerQuestions)
            {
                QuestionDetailBE questionDetail = new QuestionDetailBE()
                {
                    QuestionID = question.QuestionID,
                    QuestionSequence = question.QuestionSequence,
                    Text = questions.FirstOrDefault(q => q.QuestionID == question.QuestionID).Text,
                    Answers = answers.Where(a => a.QuestionID == question.QuestionID).ToList()
                };
                roundBE.QuestionDetails.Add(questionDetail);
            }

            return roundBE;
        }
    }
}