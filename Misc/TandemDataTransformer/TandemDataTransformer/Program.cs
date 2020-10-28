using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TandemDataTransformer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Setup constants
            string cwd = Directory.GetCurrentDirectory(); //where we are
            const string relativePath = "..\\..\\..\\..\\..\\..\\Source\\Data\\Tandem.Data\\DataFiles"; //where we're going
            string dataFileDir = $"{cwd}\\{relativePath}"; //full path

            string rawDataFileName = "Apprentice_TandemFor400_Data.json"; //file to work off of
            string QuestionDataFileName = "Question.json"; //file to store question data
            string AnswerDataFileName = "Answer.json"; //file to store answer data

            //Read file
            string rawFileContents = await File.ReadAllTextAsync($"{dataFileDir}\\{rawDataFileName}");

            //Parse file
            List<FlatData> unprocessedData = JsonSerializer.Deserialize<List<FlatData>>(rawFileContents);

            //Transform data
            int questionID = 0;
            int answerID = 0;
            const string defaultUser = "DataTransform";
            List<QuestionEntity> questionEntities = new List<QuestionEntity>();
            List<AnswerEntity> answerEntities = new List<AnswerEntity>();
            foreach (FlatData flatQuestion in unprocessedData)
            {
                //Process question data
                QuestionEntity newQuestion = new QuestionEntity()
                {
                    QuestionID = ++questionID,
                    Text = flatQuestion.question
                };
                newQuestion.CreatedBy = newQuestion.LastModifiedBy = defaultUser;
                newQuestion.CreatedDateTime = newQuestion.LastModifiedDateTime = DateTime.UtcNow;
                questionEntities.Add(newQuestion);

                //process answer data
                //right answer
                AnswerEntity rightAnswer = new AnswerEntity()
                {
                    AnswerID = ++answerID,
                    QuestionID = newQuestion.QuestionID,
                    Text = flatQuestion.correct,
                    IsCorrect = true
                };
                rightAnswer.CreatedBy = rightAnswer.LastModifiedBy = defaultUser;
                rightAnswer.CreatedDateTime = rightAnswer.LastModifiedDateTime = DateTime.UtcNow;
                answerEntities.Add(rightAnswer);
                //wrong answers
                foreach (string wrongAnswer in flatQuestion.incorrect)
                {
                    AnswerEntity newWrongAnswer = new AnswerEntity()
                    {
                        AnswerID = ++answerID,
                        QuestionID = newQuestion.QuestionID,
                        Text = wrongAnswer,
                        IsCorrect = false
                    };
                    newWrongAnswer.CreatedBy = newWrongAnswer.LastModifiedBy = defaultUser;
                    newWrongAnswer.CreatedDateTime = newWrongAnswer.LastModifiedDateTime = DateTime.UtcNow;
                    answerEntities.Add(newWrongAnswer);
                }
            }

            #region DIAGNOSTICS
            //List<DiagnosticCompositeEntity> diagnosticData = questionEntities.Select(q => new DiagnosticCompositeEntity()
            //{
            //    Question = q,
            //    Answers = answerEntities.Where(a => a.QuestionID == q.QuestionID).OrderBy(a => a.AnswerID).ToList()
            //}).ToList();
            //Console.WriteLine(JsonSerializer.Serialize(diagnosticData, new JsonSerializerOptions() { WriteIndented = true }));
            #endregion

            //Write data
            string questionData = JsonSerializer.Serialize(questionEntities);
            string answerData = JsonSerializer.Serialize(answerEntities);

            await File.WriteAllTextAsync($"{dataFileDir}\\{QuestionDataFileName}", questionData);
            await File.WriteAllTextAsync($"{dataFileDir}\\{AnswerDataFileName}", answerData);

        }

    }

    public class FlatData
    {
        public string question { get; set; }
        public List<string> incorrect { get; set; }
        public string correct { get; set; }
    }

    public class QuestionEntity : BaseEntity
    {
        public int QuestionID { get; set; }
        public string Text { get; set; }
    }

    public class AnswerEntity : BaseEntity
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }

    public class DiagnosticCompositeEntity
    {
        public QuestionEntity Question { get; set; }
        public List<AnswerEntity> Answers { get; set; }
    }
}
