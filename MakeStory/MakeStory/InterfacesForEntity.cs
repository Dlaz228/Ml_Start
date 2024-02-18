using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ml_Start.MakeStory.Classes;

namespace Ml_Start.MakeStory;

interface IFinanceOperations
{
    public string BuyItem(Item item);

    public string SellItem(Item item);
}

interface IItemProcessor
{
    public string ProcessItem(Item item, string action);
}

