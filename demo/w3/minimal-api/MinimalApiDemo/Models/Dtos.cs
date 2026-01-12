
namespace MinimalApiDemo.Models;

public record CreateItemRequest(string Name, bool IsDone);

public record ItemResponse(int Id, string Name, bool IsDone)
{
    public static ItemResponse FromModel(Item m) => new(m.Id, m.Name, m.IsDone);
}
