using QuizBattle.Console;
using QuizBattle.Domain;

var repo = new InMemoryQuestionRepository();
var service = new QuestionService(repo);
var questions = service.GetRandomQuestions();

Console.Title = "QuizBattle – Konsol (v.2 dag 1–2)";
Console.WriteLine("Välkommen till QuizBattle!");
Console.WriteLine("Detta är en minimal code‑along‑loop för dag 1–2 (3 frågor).");
Console.WriteLine("Tryck valfri tangent för att starta...");

Console.ReadKey(true);
Console.WriteLine();

var score = 0;
var asked = 0;

foreach (Question question in questions.Take(3))
{
    asked++;
    QuestionUtils.DisplayQuestion();
    int answer = QuestionUtils.GetAnswer();

    QuestionUtils.CheckAnswer(answer);

    QuestionUtils.WriteStatus();
}

Console.WriteLine("Tack för att du spelade Quiz Battle!");
