    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBattle.Application.Features.AnswerQuestion;

public sealed record AnswerQuestionResult
{
    public bool IsCorrect { get; init; }

}

