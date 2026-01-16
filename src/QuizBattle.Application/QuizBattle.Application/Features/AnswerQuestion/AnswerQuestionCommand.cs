using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBattle.Application.Features.AnswerQuestion
{
    public sealed record AnswerQuestionCommand
    {
        public AnswerQuestionCommand(Guid sessionId, string questionCode, string selectedChoiceCode)
        {
            SessionId = sessionId;
            QuestionCode = questionCode;
            SelectedChoiceCode = selectedChoiceCode;
        }

        public Guid SessionId { get; init; }
        public string QuestionCode { get; init; }
        public string SelectedChoiceCode { get; init; }
        public object QuestionId { get; internal set; }
    }
}
