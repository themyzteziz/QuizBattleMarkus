using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizBattle.Application.Interfaces;
using QuizBattle.Domain;

namespace QuizBattle.Application.Features.FinishSession
{
    public sealed class FinishSessionHandler
    {
        private readonly ISessionRepository _sessions;

        public FinishSessionHandler(ISessionRepository sessions)
        {
            _sessions = sessions;
        }

        public async Task<FinishSessionResult> HandleAsync(FinishSessionCommand cmd, CancellationToken ct = default)
        {
            // 1. Hämta session
            var session = await _sessions.GetByIdAsync(cmd.SessionId, ct);

            // 2. Avsluta sessionen (domänen bestämmer allt)
            session.Finish(DateTime.UtcNow);

            // 3. Spara
            await _sessions.SaveAsync(session, ct);

            // 4. Skicka tillbaka resultat
            return new FinishSessionResult
            {
                TotalScore = session.Score,
                CorrectAnswers = session.Answers.Count(a => a.IsCorrect),
                TotalQuestions = session.QuestionCount
            };
        }
    }
}