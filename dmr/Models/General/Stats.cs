

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using StatType = System.Int16;

namespace dmr.Models.General
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Stats
    {
		// base stats
        public StatType Strength, Agility, Intellect, Wisdom, WeaponExpertise, Resolve;

		// derived stats
					public StatType BaseBlockChance;
			public StatType BlockChance
			{
				get
				{
					var val = BaseBlockChance + Strength + Agility + Resolve;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseDodgeChance;
			public StatType DodgeChance
			{
				get
				{
					var val = BaseDodgeChance + 2 + Agility * 3 + Resolve;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseCounterChance;
			public StatType CounterChance
			{
				get
				{
					var val = BaseCounterChance + Agility + Resolve;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseHitPoints;
			public StatType HitPoints
			{
				get
				{
					var val = BaseHitPoints + 3 * Strength + 6 * Resolve;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseSpellPower;
			public StatType SpellPower
			{
				get
				{
					var val = BaseSpellPower + 2 * Intellect;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseMaxMana;
			public StatType MaxMana
			{
				get
				{
					var val = BaseMaxMana + 4 * Intellect + 5 * Wisdom;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseManaRegeneration;
			public StatType ManaRegeneration
			{
				get
				{
					var val = BaseManaRegeneration + Wisdom;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseSpellCriticalChance;
			public StatType SpellCriticalChance
			{
				get
				{
					var val = BaseSpellCriticalChance + Intellect * 3 + Wisdom * 4;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseSpellCriticalDamageMultiplier;
			public StatType SpellCriticalDamageMultiplier
			{
				get
				{
					var val = BaseSpellCriticalDamageMultiplier + 150 + Intellect / 3.0 + Wisdom;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseBrutePhysicalPower;
			public StatType BrutePhysicalPower
			{
				get
				{
					var val = BaseBrutePhysicalPower + Strength;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseFinessePhyiscalPower;
			public StatType FinessePhyiscalPower
			{
				get
				{
					var val = BaseFinessePhyiscalPower + Agility;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BasePhysicalCriticalChance;
			public StatType PhysicalCriticalChance
			{
				get
				{
					var val = BasePhysicalCriticalChance + Agility * 3 + WeaponExpertise * 4;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BasePhysicalCriticalDamageMultiplier;
			public StatType PhysicalCriticalDamageMultiplier
			{
				get
				{
					var val = BasePhysicalCriticalDamageMultiplier + 150 + Agility / 3.0 + WeaponExpertise;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
					public StatType BaseDodgeReduction;
			public StatType DodgeReduction
			{
				get
				{
					var val = BaseDodgeReduction + Agility / 3.0 + WeaponExpertise * 2;
					if(val < StatType.MinValue)
						return StatType.MinValue;
					else if(val > StatType.MaxValue)
						return StatType.MaxValue;
					return (StatType)val;
				}
			}
		
        public static Stats Aggregate(IEnumerable<Stats> source)
        {
            var result = new Stats();
            foreach(var item in source)
			{
				// base stats
									result.Strength += item.Strength;
									result.Agility += item.Agility;
									result.Intellect += item.Intellect;
									result.Wisdom += item.Wisdom;
									result.WeaponExpertise += item.WeaponExpertise;
									result.Resolve += item.Resolve;
				
				// derived stats
									result.BaseBlockChance += item.BaseBlockChance;
									result.BaseDodgeChance += item.BaseDodgeChance;
									result.BaseCounterChance += item.BaseCounterChance;
									result.BaseHitPoints += item.BaseHitPoints;
									result.BaseSpellPower += item.BaseSpellPower;
									result.BaseMaxMana += item.BaseMaxMana;
									result.BaseManaRegeneration += item.BaseManaRegeneration;
									result.BaseSpellCriticalChance += item.BaseSpellCriticalChance;
									result.BaseSpellCriticalDamageMultiplier += item.BaseSpellCriticalDamageMultiplier;
									result.BaseBrutePhysicalPower += item.BaseBrutePhysicalPower;
									result.BaseFinessePhyiscalPower += item.BaseFinessePhyiscalPower;
									result.BasePhysicalCriticalChance += item.BasePhysicalCriticalChance;
									result.BasePhysicalCriticalDamageMultiplier += item.BasePhysicalCriticalDamageMultiplier;
									result.BaseDodgeReduction += item.BaseDodgeReduction;
							}

			return result;
        }

		public void UpdateFromConfigurationPair(string statname, string value)
		{
			switch(statname)
			{
				// base stats
									case "strength":
						Strength = StatType.Parse(value);
						break;
									case "agility":
						Agility = StatType.Parse(value);
						break;
									case "intellect":
						Intellect = StatType.Parse(value);
						break;
									case "wisdom":
						Wisdom = StatType.Parse(value);
						break;
									case "weapon_expertise":
						WeaponExpertise = StatType.Parse(value);
						break;
									case "resolve":
						Resolve = StatType.Parse(value);
						break;
				
				// derived stats
									case "block_chance":
						BaseBlockChance = StatType.Parse(value);
						break;
									case "dodge_chance":
						BaseDodgeChance = StatType.Parse(value);
						break;
									case "counter_chance":
						BaseCounterChance = StatType.Parse(value);
						break;
									case "hit_points":
						BaseHitPoints = StatType.Parse(value);
						break;
									case "spell_power":
						BaseSpellPower = StatType.Parse(value);
						break;
									case "max_mana":
						BaseMaxMana = StatType.Parse(value);
						break;
									case "mana_regen":
						BaseManaRegeneration = StatType.Parse(value);
						break;
									case "spell_crit_chance":
						BaseSpellCriticalChance = StatType.Parse(value);
						break;
									case "spell_crit_mult":
						BaseSpellCriticalDamageMultiplier = StatType.Parse(value);
						break;
									case "brute_physical_power_mult":
						BaseBrutePhysicalPower = StatType.Parse(value);
						break;
									case "finesse_physical_power_mult":
						BaseFinessePhyiscalPower = StatType.Parse(value);
						break;
									case "physical_crit_chance":
						BasePhysicalCriticalChance = StatType.Parse(value);
						break;
									case "physical_crit_mult":
						BasePhysicalCriticalDamageMultiplier = StatType.Parse(value);
						break;
									case "dodge_reduction":
						BaseDodgeReduction = StatType.Parse(value);
						break;
								default: throw new InvalidOperationException();
			}
		}
    }
}
