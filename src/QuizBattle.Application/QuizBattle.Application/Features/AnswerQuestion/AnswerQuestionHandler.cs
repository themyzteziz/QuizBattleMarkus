using QuizBattle.Application.Interfaces;
using QuizBattle.Domain;

namespace QuizBattle.Application.Features.AnswerQuestion;

public sealed class AnswerQuestionHandler
{
    private readonly ISessionRepository _sessions;
    private readonly IQuestionRepository _questions;

    public AnswerQuestionHandler(
        ISessionRepository sessions,
        IQuestionRepository questions)
    {
        _sessions = sessions;
        _questions = questions;
    }

    public async Task<AnswerQuestionResult> HandleAsync(
        AnswerQuestionCommand cmd,
        CancellationToken ct = default)
    {
        // 1. Hämta session
        var session = await _sessions.GetByIdAsync(cmd.SessionId, ct);
        if (session is null)
            throw new InvalidOperationException($"Session '{cmd.SessionId}' not found.");

        // 2. Hämta fråga
        var question = await _questions.GetByCodeAsync(cmd.QuestionCode, ct);
        if (question is null)
            throw new InvalidOperationException($"Question '{cmd.QuestionCode}' not found.");

        // 3. Registrera svar (domänlogik)
        session.SubmitAnswer(question, cmd.SelectedChoiceCode, DateTime.UtcNow);

        // 4. Spara
        await _sessions.SaveAsync(session, ct);

        // 5. Avgör om svaret var rätt (senaste svaret)
        var lastAnswer = session.Answers.Last(a =>
            a.Question.Code.Equals(cmd.QuestionCode, StringComparison.OrdinalIgnoreCase));

        return new AnswerQuestionResult
        {
            IsCorrect = lastAnswer.IsCorrect
        };
    }
}
