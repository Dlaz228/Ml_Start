using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ml_Start.MakeStory.Classes;

namespace Ml_Start.MakeStory
{
    public class Story
    {
        public List<string> GetStory(double num)
        {
            List<string> story = [];
            double number = num;

            Person korotyshka = new("Коротышка");
            Person neznayka = new("Незнайка");
            Person kozlik = new("Козлик");
            Person miga = new("Мига");

            Crowd interestedParties = new("Желающиe");
            Crowd population = new("Население");
            Crowd passersby = new("Прохожие");

            Bank bank = new();

            Storage safe = new("Несгораемый шкаф");
            Storage firstChest = new("Первый несгораемый сундук");
            Storage secondChest = new("Второй несгораемый сундук");

            firstChest.AddItems(FillStorage(1000000));
            secondChest.AddItems(FillStorage(1000000));

            story.Add(korotyshka.BuyItem(new Item("Aкции")));
            story.Add(korotyshka.PerformAction(Actions.Departure));

            story.Add(interestedParties.PerformAction("приобрести акции с каждым днем становилось все больше"));

            story.Add(neznayka.SellItem(new Item("Aкции")));
            story.Add(kozlik.SellItem(new Item("Aкции")));
            story.Add(miga.PerformAction(Actions.GoToBank));

            story.Add(bank.MoneyExchange(miga));
            story.Add(safe.AddItems(miga, FillStorage(new Random().Next(5, 20))));

            story.Add(interestedParties.PerformAction("толклись на улице, дожидаясь открытия конторы"));
            story.Add(passersby.PerformAction("заинтересовались"));
            story.Add(population.PerformAction("узнало что акции Общества гигантских растений пользуются большим спросом"));
            story.Add(population.PerformAction("спешило накупить гигантских акций, с тем чтоб продать их, как только они повысятся в цене"));

            story.Add(firstChest.SellAllItems());
            story.Add(secondChest.SellAllItems());
            story.Add(AddNarrativeLine(number));

            return story;
        }

        public List<Item> FillStorage(int num)
        {
            List<Item> items = [];

            for (int i = 0; i < num; i++)
            {
                items.Add(new Item("Деньги"));
            }

            return items;
        }

        public string AddNarrativeLine(double number)
        {
            return $"Все акции общества были распроданы со средней стоимостью {number}";
        }
    }
}
