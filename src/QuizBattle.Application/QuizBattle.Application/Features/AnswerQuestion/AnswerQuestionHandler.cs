using QuizBattle.Application.Interfaces; // där ISessionRepository ligger
using QuizBattle.Domain;          // där QuizSession ligger (justera)

namespace QuizBattle.Application.Features.AnswerQuestion;

public sealed class AnswerQuestionHandler
{
    private readonly ISessionRepository _sessions;

    public AnswerQuestionHandler(ISessionRepository sessions)
    {
        _sessions = sessions;
    }

    public async Task<AnswerQuestionResult> HandleAsync(AnswerQuestionCommand cmd, CancellationToken ct = default)
    {
        // Steg 2–6 kommer här
        throw new NotImplementedException();
    }

    internal async Task<AnswerQuestionResult> Handle(AnswerQuestionCommand cmd, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
