using QuizBattle.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBattle.Application.Features.StartSession
{
    public class StartSessionResult
    {
        public Guid SessionId { get; init; }
        public IReadOnlyList<Question> Questions { get; init; }
    }
}
