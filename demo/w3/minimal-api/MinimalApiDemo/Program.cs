
using Microsoft.AspNetCore.Mvc;
using MinimalApiDemo.Models;
using MinimalApiDemo.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IItemRepository, InMemoryItemRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

// GET /items -> 200 OK
app.MapGet("/items", (
    IItemRepository repo) => Results.Ok(repo.GetAll().Select(ItemResponse.FromModel))
)
.WithName("GetItems")
.WithSummary("Hämta alla items")
.WithDescription("Returnerar alla items som 200 OK.");

// GET /items/{id} -> 200/404
app.MapGet("/items/{id:int}", (
    int id, IItemRepository repo) =>
{
    var item = repo.GetById(id);
    return item is null
        ? Results.NotFound(new { error = $"Item med id {id} hittades inte" })
        : Results.Ok(ItemResponse.FromModel(item));
})
.WithName("GetItemById")
.WithSummary("Hämta item per id")
.WithDescription("Returnerar 200 OK eller 404 NotFound.");

// POST /items -> 201/400/409
app.MapPost("/items", (
    CreateItemRequest req, IItemRepository repo, HttpContext ctx) =>
{
    var validation = Validate(req);
    if (!validation.IsValid)
        return Results.BadRequest(new { error = validation.Error });

    if (repo.ExistsByName(req.Name))
        return Results.Conflict(new { error = $"Item med namnet '{req.Name}' finns redan" });

    var created = repo.Add(new Item { Name = req.Name, IsDone = req.IsDone });
    var url = $"{ctx.Request.Scheme}://{ctx.Request.Host}/items/{created.Id}";
    return Results.Created(url, ItemResponse.FromModel(created));
})
.WithName("CreateItem")
.WithSummary("Skapa item")
.WithDescription("Returnerar 201 Created; 400 BadRequest vid ogiltig data; 409 Conflict vid duplikatnamn.");

// GET /items/slow?ms=5000 -> 200 OK efter fördröjning (för timeout-demo)
app.MapGet("/items/slow", async ([FromQuery] int ms, IItemRepository repo) =>
{
    if (ms < 0 || ms > 60000)
        return Results.BadRequest(new { error = "Parametern ms måste vara 0–60000" });

    await Task.Delay(ms);
    return Results.Ok(repo.GetAll().Select(ItemResponse.FromModel));
})
.WithName("GetItemsSlow")
.WithSummary("Sakta endpoint för timeout-demo")
.WithDescription("Använd för att demonstrera klientens timeout/cancellation.");

app.Run();

static (bool IsValid, string? Error) Validate(CreateItemRequest req)
{
    if (string.IsNullOrWhiteSpace(req.Name))
        return (false, "Name är obligatoriskt");
    if (req.Name.Length < 2)
        return (false, "Name måste vara minst 2 tecken");
    return (true, null);
}
