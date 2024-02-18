using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ml_Start.MakeStory;

public class Classes
{
    public abstract class Entity
    {
        public string Name;

        public Entity(string name)
        {
            Name = name;
        }
    }

    public record class Item
    {
        public string Name;

        public Item(string name)
        {
            Name = name;
        }
    }

    public class Bank
    {
        public string MoneyExchange(Person person)
        {
            return $"{person.Name} обменял мелкие деньги на крупные";
        }
    }

    public class Storage
    {
        public string Name;

        public List<Item> StorageItems;

        public Storage(string name)
        {
            Name = name;
            StorageItems = new List<Item>();
        }

        public string AddItems(Person person, List<Item> items)
        {
            foreach (var item in items)
            {
                StorageItems.Add(item);
            }

            return $"{person.Name} добавил {items[0].Name} в {Name}";
        }

        public void AddItems(List<Item> items)
        {
            foreach (var item in items)
            {
                StorageItems.Add(item);
            }
        }

        public string SellAllItems()
        {
            Item item = StorageItems[0];

            int count = StorageItems.Count;

            StorageItems = new List<Item>();

            return $"{count} {item.Name} было распродано из {Name}";
        }
    }

    public class Crowd : Entity
    {
        public Crowd(string name) : base(name) { }

        public string PerformAction(string action)
        {
            return $"{Name} {action}";
        }
    }

    public class Person : Entity, IFinanceOperations, IItemProcessor
    {
        public Person(string name) : base(name) { }

        public string BuyItem(Item item)
        {
            string action = ConvertAction(Actions.BuyItem);

            return ProcessItem(item, action);
        }

        public string SellItem(Item item)
        {
            string action = ConvertAction(Actions.SellItem);

            return ProcessItem(item, action);
        }

        public string ProcessItem(Item item, string action)
        {
            return $"{Name} {action} {item.Name}";
        }

        public string ConvertAction(Actions action)
        {
            switch (action)
            {
                case Actions.BuyItem:
                    return "покупает";
                case Actions.SellItem:
                    return "продает";
                case Actions.Departure:
                    return "удалился";
                case Actions.GoToBank:
                    return "ездит в банк";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        public string PerformAction(Actions action)
        {
            string stringActions = ConvertAction(action);

            return $"{Name} {stringActions}";
        }

    }

    public enum Actions
    {
        BuyItem,
        SellItem,
        Departure,
        GoToBank
    }
}