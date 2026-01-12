
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Repos;

public interface IItemRepository
{
    IEnumerable<Item> GetAll();
    Item? GetById(int id);
    bool ExistsByName(string name);
    Item Add(Item item);
}
