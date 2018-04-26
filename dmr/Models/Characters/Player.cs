using dmr.Models.General;
using dmr.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models.Characters
{
    public class Player : Character
    {
        public ItemTemplate[] ItemInSlot = new ItemTemplate[(int)ItemSlot.MaxItemSlot];

        public Player()
        {
            StatBuffs = new StatsCollection(true);
        }

        public void HoldItem(ItemTemplate item)
        {
            StatBuffs[item.Slot] = item.Stats;
            ItemInSlot[(int)item.Slot] = item;
        }
    }
}
