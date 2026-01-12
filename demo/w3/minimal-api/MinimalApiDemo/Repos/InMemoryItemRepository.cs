
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Repos;

public class InMemoryItemRepository : IItemRepository
{
    private readonly List<Item> _items = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public InMemoryItemRepository()
    {
        // Seed
        _items.AddRange(new[]
        {
            new Item { Id = _nextId++, Name = "Läsa dokumentation", IsDone = false },
            new Item { Id = _nextId++, Name = "Skriva klient", IsDone = false },
            new Item { Id = _nextId++, Name = "Testa felkoder", IsDone = false }
        });
    }

    public IEnumerable<Item> GetAll()
    {
        lock (_lock) return _items.ToList();
    }

    public Item? GetById(int id)
    {
        lock (_lock) return _items.FirstOrDefault(i => i.Id == id);
    }

    public bool ExistsByName(string name)
    {
        lock (_lock) return _items.Any(i => string.Equals(i.Name, name, StringComparison.CurrentCultureIgnoreCase));
    }

    public Item Add(Item item)
    {
        lock (_lock)
        {
            item.Id = _nextId++;
            _items.Add(item);
            return item;
        }
    }
}
