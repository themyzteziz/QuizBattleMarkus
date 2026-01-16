using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBattle.Application.Features.FinishSession
{
    public sealed record FinishSessionResult
    {
        public int TotalScore { get; init; }
        public int CorrectAnswers { get; init; }
        public int TotalQuestions { get; init; }
    }
}
