

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using StatType = System.SByte;

namespace dmr.Models.Characters
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Stats
    {
        public StatType Strength, Agility, Intellect, Wisdom, Resolve;

        public static Stats Aggregate(IEnumerable<Stats> source)
        {
            var result = new Stats();
            foreach(var item in source)
			{
									result.Strength += item.Strength;
									result.Agility += item.Agility;
									result.Intellect += item.Intellect;
									result.Wisdom += item.Wisdom;
									result.Resolve += item.Resolve;
							}

			return result;
        }
    }
}
