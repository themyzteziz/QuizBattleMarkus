using QuizBattle.Application.Interfaces;
using QuizBattle.Domain;

namespace QuizBattle.Application.Features.StartSession;

public sealed class StartSessionHandler
{
    private readonly IQuestionRepository _questions;
    private readonly ISessionRepository _sessions;

    public StartSessionHandler(IQuestionRepository questions, ISessionRepository sessions)
    {
        _questions = questions;
        _sessions = sessions;
    }

    public async Task<StartSessionResult> HandleAsync(StartSessionCommand cmd, CancellationToken ct = default)
    {
        // 1) Hämta frågor (repo sköter random/filter)
        var qs = await _questions.GetRandomAsync(cmd.Category, cmd.Difficulty, cmd.QuestionCount, ct);

        // 2) Skapa session (domänen)
        var session = QuizSession.Create(cmd.QuestionCount);

        // 3) Spara session
        await _sessions.SaveAsync(session, ct);

        // 4) Returnera
        return new StartSessionResult
        {
            SessionId = session.Id,
            Questions = qs
        };
    }
}
