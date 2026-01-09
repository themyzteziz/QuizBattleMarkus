using QuizBattle.Domain;

namespace QuizBattle.Console
{
    public interface IQuestionService
    {
        void DisplayQuestion(Question question, int number);
        Question GetRandomQuestion();
        List<Question> GetRandomQuestions(int count = 3);
        int PromptForAnswer(Question question);
    }
}