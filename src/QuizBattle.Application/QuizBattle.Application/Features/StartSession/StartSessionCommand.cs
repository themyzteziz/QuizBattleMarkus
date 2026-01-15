using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuizBattle.Application.Features.StartSession
{
    public sealed record StartSessionCommand
     {
        public int QuestionCount { get; init; }
        public string? Category { get; init; }
        public int? Difficulty { get; init; }
    }
}
