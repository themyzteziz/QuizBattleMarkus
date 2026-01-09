
using QuizBattle.Console;
using QuizBattle.Console.Extensions;
using QuizBattle.Domain;

const int numberOfQuestions = 3;

var repository = new InMemoryQuestionRepository();
IQuestionService service = new QuestionService(repository);
var questions = service.GetRandomQuestions();

Console.Title = "QuizBattle – Konsol (v.2 dag 1–2)";
Console.WriteLine("Välkommen till QuizBattle!");
Console.WriteLine($"Detta är en minimal code‑along‑loop för dag 1–2 ({numberOfQuestions} frågor).");
Console.WriteLine("Tryck valfri tangent för att starta...");

Console.ReadKey(intercept: true);
Console.WriteLine();

var score = 0;
var asked = 0;

foreach (var question in questions.Take(numberOfQuestions))
{
    asked++;
    service.DisplayQuestion(question, asked);

    var pick = service.PromptForAnswer(question);

    var selectedCode = question.GetChoiceAt(pick - 1).Code;
    var isCorrect = question.IsCorrect(selectedCode);

    Console.WriteLine(isCorrect ? "✔ Rätt!" : "✖ Fel.");

    if (isCorrect)
    {
        score++;
    }

    Console.WriteLine();
}

Console.WriteLine($"Klart! Poäng: {score}/{asked}");
Console.WriteLine("Tryck valfri tangent för att avsluta...");
Console.ReadKey(intercept: true);
