

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using StatType = System.SByte;

namespace dmr.Models.General
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Stats
    {
		// base stats
        public StatType Strength, Agility, Intellect, Wisdom, WeaponExpertise, Resolve;

		// derived stats
					public StatType BaseBlockChance;
			public StatType BlockChance => BaseBlockChance + .2f + (Strength / 3f + Agility + Resolve * 2) / 100f;
					public StatType BaseBlockValue;
			public StatType BlockValue => BaseBlockValue + 1.3f * Strength + Resolve;
					public StatType BaseDodgeChance;
			public StatType DodgeChance => BaseDodgeChance + .02f + (Agility * 3 + Resolve) / 100f;
					public StatType BaseCounterChance;
			public StatType CounterChance => BaseCounterChance + (Agility / 2f + Resolve) / 100f;
					public StatType BaseHitPoints;
			public StatType HitPoints => BaseHitPoints + 3 * Strength + 6 * Resolve;
					public StatType BaseSpellPower;
			public StatType SpellPower => BaseSpellPower + 2 * Intellect;
					public StatType BaseMaxMana;
			public StatType MaxMana => BaseMaxMana + 4 * Intellect + 5 * Wisdom;
					public StatType BaseManaRegeneration;
			public StatType ManaRegeneration => BaseManaRegeneration + Wisdom;
					public StatType BaseSpellCriticalChance;
			public StatType SpellCriticalChance => BaseSpellCriticalChance + (Intellect * 3 + Wisdom * 4) / 100f;
					public StatType BaseSpellCriticalDamageMultiplier;
			public StatType SpellCriticalDamageMultiplier => BaseSpellCriticalDamageMultiplier + 1.5f + Intellect / 100f + Wisdom / 20f;
					public StatType BaseBrutePhysicalPowerMultiplier;
			public StatType BrutePhysicalPowerMultiplier => BaseBrutePhysicalPowerMultiplier + Strength / 20f;
					public StatType BaseFinessePhyiscalPowerMultiplier;
			public StatType FinessePhyiscalPowerMultiplier => BaseFinessePhyiscalPowerMultiplier + Agility / 20f;
					public StatType BasePhysicalCriticalChance;
			public StatType PhysicalCriticalChance => BasePhysicalCriticalChance + (Agility * 3 + WeaponExpertise * 4) / 100f;
					public StatType BasePhysicalCriticalDamageMultiplier;
			public StatType PhysicalCriticalDamageMultiplier => BasePhysicalCriticalDamageMultiplier + 1.5f + Agility / 100f + WeaponExpertise / 20f;
					public StatType BaseDodgeReduction;
			public StatType DodgeReduction => BaseDodgeReduction + (Agility / 3f + WeaponExpertise * 2) / 100f;
		
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
									result.BaseBlockValue += item.BaseBlockValue;
									result.BaseDodgeChance += item.BaseDodgeChance;
									result.BaseCounterChance += item.BaseCounterChance;
									result.BaseHitPoints += item.BaseHitPoints;
									result.BaseSpellPower += item.BaseSpellPower;
									result.BaseMaxMana += item.BaseMaxMana;
									result.BaseManaRegeneration += item.BaseManaRegeneration;
									result.BaseSpellCriticalChance += item.BaseSpellCriticalChance;
									result.BaseSpellCriticalDamageMultiplier += item.BaseSpellCriticalDamageMultiplier;
									result.BaseBrutePhysicalPowerMultiplier += item.BaseBrutePhysicalPowerMultiplier;
									result.BaseFinessePhyiscalPowerMultiplier += item.BaseFinessePhyiscalPowerMultiplier;
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
									case "block_value":
						BaseBlockValue = StatType.Parse(value);
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
						BaseBrutePhysicalPowerMultiplier = StatType.Parse(value);
						break;
									case "finesse_physical_power_mult":
						BaseFinessePhyiscalPowerMultiplier = StatType.Parse(value);
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
