
using System.Net.Http.Json;
using System.Text.Json;

using HttpClientDemo.Models;

// Enkla inställningar
// Bas-URL för API:et kan sättas via miljövariabeln DEMO_API_BASE
// Om ej satt används http://localhost:5280 (matchar API:ets http-port i launchSettings.json)
var baseUrl = Environment.GetEnvironmentVariable("DEMO_API_BASE") ?? "http://localhost:5280"; // matchar API:ets http-port
var http = new HttpClient { BaseAddress = new Uri(baseUrl) };

Console.WriteLine($"Bas-URL: {http.BaseAddress}");
Console.WriteLine(@"""
Meny:
1) Hämta alla items (GET /items)
2) Hämta item som ej finns (GET /items/9999)
3) Skapa item (POST /items)
4) Skapa duplikat (POST /items med redan taget namn)
5) Timeout-demo (GET /items/slow?ms=5000) med 2s timeout
6) Cancellation-demo (avbryt pågående request)
0) Avsluta""");

while (true)
{
    Console.Write(@"""
Val: """);
    var key = Console.ReadLine();
    if (key == "0" || key?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true) break;

    try
    {
        switch (key)
        {
            case "1":
                await GetAll();
                break;
            case "2":
                await GetNotFound();
                break;
            case "3":
                await PostCreate("Min nya uppgift " + DateTime.Now.ToString("HHmmss"));
                break;
            case "4":
                await PostDuplicate("Läsa dokumentation"); // finns i seed
                break;
            case "5":
                await GetWithTimeout();
                break;
            case "6":
                await GetWithCancellation();
                break;
            default:
                Console.WriteLine("Okänt val.");
                break;
        }
    }
    catch (HttpRequestException ex) // nätverksrelaterade fel
    {
        Console.WriteLine($"HTTP-fel: {ex.Message}");
    }
    catch (TaskCanceledException ex) // timeout eller avbrutet
    {
        Console.WriteLine($"Avbrutet/Timeout: {ex.Message}");
    }
    catch (Exception ex) // alla andra typer av oväntade fel
    {
        Console.WriteLine($"Oväntat fel: {ex.Message}");
    }
}

/// Hämta alla items (standard GET)
async Task GetAll()
{
    Console.WriteLine("→ GET /items");
    var resp = await http.GetAsync("/items");
    await Print(resp);
}

// Hämta ett item som inte finns
async Task GetNotFound()
{
    Console.WriteLine("→ GET /items/9999");

    // Skicka anropet
    var resp = await http.GetAsync("/items/9999");
    await Print(resp);
}

/// Skapa ett nytt item
async Task PostCreate(string name)
{
    Console.WriteLine("→ POST /items (skapa nytt)");

    // Skapa ett object som innehåller svaret på frågan:
    // - namn
    // - isDone = false
    //
    // Detta objekt kommer att omvandlas till JSON innan -det skickas
    var payload = new { name, isDone = false };
    var resp = await http.PostAsJsonAsync("/items", payload);
    await Print(resp);
}

/// Skapa ett item med namn som redan finns
async Task PostDuplicate(string name)
{
    Console.WriteLine("→ POST /items (duplikat)");

    // Skicka samma namn som redan finns
    var payload = new { name, isDone = false };
    var resp = await http.PostAsJsonAsync("/items", payload);
    await Print(resp);
}


/// Timeout-exempel: Avbryt anropet efter 2 sekunder
async Task GetWithTimeout()
{
    Console.WriteLine("→ GET /items/slow?ms=5000 med 2s timeout");

    // Skapa en CancellationTokenSource som avbryts efter 2 sekunder
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

    // Skicka anropet med token
    var resp = await http.GetAsync("/items/slow?ms=5000", cts.Token);
    await Print(resp);
}

/// Avbryt anropet när användaren trycker en tangent
/// Om användaren inte trycker på en tangent, vänta på svaret tills timeout
async Task GetWithCancellation()
{
    Console.WriteLine("→ GET /items/slow?ms=5000 — tryck valfri tangent för att avbryta…");

    // Skapa en CancellationTokenSource som kan avbrytas manuellt
    using var cts = new CancellationTokenSource();

    // Starta anropet och en task som väntar på tangenttryckning
    var requestTask = http.GetAsync("/items/slow?ms=5000", cts.Token);

    // Vänta på antingen anropet eller tangenttryckningen
    var cancelTask = Task.Run(() => { Console.ReadKey(true); cts.Cancel(); });

    // Vänta på den första som blir klar
    var completed = await Task.WhenAny(requestTask, cancelTask);

    if (completed == cancelTask)
    {
        Console.WriteLine("Anrop avbrutet av användaren.");
        return;
    }

    var resp = await requestTask; // om inte avbrutet, hämta resultat
    await Print(resp);
}

// Hjälpmetod för att skriva ut responsinfo. Formaterar JSON-svar fint, och undviker kodupprepning.
static async Task Print(HttpResponseMessage resp)
{
    Console.WriteLine($"Status: {(int)resp.StatusCode} {resp.StatusCode}");
    string body = string.Empty;
    try
    {
        body = await resp.Content.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(body))
        {
            using var jdoc = JsonDocument.Parse(body);
            body = JsonSerializer.Serialize(jdoc, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    catch { }

    if (!string.IsNullOrWhiteSpace(body))
    {
        Console.WriteLine(@$"Body:
{body}");
    }

    Console.WriteLine("Rekommenderad handling på klienten:");
    if ((int)resp.StatusCode >= 200 && (int)resp.StatusCode < 300)
        Console.WriteLine("✓ Lyckat – uppdatera UI/data.");
    else if ((int)resp.StatusCode == 404)
        Console.WriteLine("✗ NotFound – visa saknad resurs och föreslå åtgärd.");
    else if ((int)resp.StatusCode == 409)
        Console.WriteLine("⚠ Conflict – visa duplikatkonflikt och be om annat namn.");
    else if ((int)resp.StatusCode >= 400 && (int)resp.StatusCode < 500)
        Console.WriteLine("⚠ Klientfel – visa felmeddelande och validera indata.");
    else if ((int)resp.StatusCode >= 500)
        Console.WriteLine("⚠ Serverfel – visa ursäkt och försök igen senare.");
}
