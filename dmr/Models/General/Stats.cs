

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using BaseStatType = System.SByte;
using DerivedStatType = System.Single;

namespace dmr.Models.General
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Stats
    {
		// base stats
        public BaseStatType Strength, Agility, Intellect, Wisdom, WeaponExpertise, Resolve;

		// derived stats
					public DerivedStatType BaseBlockChance;
			public DerivedStatType BlockChance => BaseBlockChance + .2f + (Strength / 3f + Agility + Resolve * 2) / 100f;
					public DerivedStatType BaseBlockValue;
			public DerivedStatType BlockValue => BaseBlockValue + 1.3f * Strength + Resolve;
					public DerivedStatType BaseDodgeChance;
			public DerivedStatType DodgeChance => BaseDodgeChance + .02f + (Agility * 3 + Resolve) / 100f;
					public DerivedStatType BaseCounterChance;
			public DerivedStatType CounterChance => BaseCounterChance + (Agility / 2f + Resolve) / 100f;
					public DerivedStatType BaseHitPoints;
			public DerivedStatType HitPoints => BaseHitPoints + 3 * Strength + 6 * Resolve;
					public DerivedStatType BaseSpellPower;
			public DerivedStatType SpellPower => BaseSpellPower + 2 * Intellect;
					public DerivedStatType BaseMaxMana;
			public DerivedStatType MaxMana => BaseMaxMana + 4 * Intellect + 5 * Wisdom;
					public DerivedStatType BaseManaRegeneration;
			public DerivedStatType ManaRegeneration => BaseManaRegeneration + Wisdom;
					public DerivedStatType BaseSpellCriticalChance;
			public DerivedStatType SpellCriticalChance => BaseSpellCriticalChance + (Intellect * 3 + Wisdom * 4) / 100f;
					public DerivedStatType BaseSpellCriticalDamageMultiplier;
			public DerivedStatType SpellCriticalDamageMultiplier => BaseSpellCriticalDamageMultiplier + 1.5f + Intellect / 100f + Wisdom / 20f;
					public DerivedStatType BaseBrutePhysicalPowerMultiplier;
			public DerivedStatType BrutePhysicalPowerMultiplier => BaseBrutePhysicalPowerMultiplier + Strength / 20f;
					public DerivedStatType BaseFinessePhyiscalPowerMultiplier;
			public DerivedStatType FinessePhyiscalPowerMultiplier => BaseFinessePhyiscalPowerMultiplier + Agility / 20f;
					public DerivedStatType BasePhysicalCriticalChance;
			public DerivedStatType PhysicalCriticalChance => BasePhysicalCriticalChance + (Agility * 3 + WeaponExpertise * 4) / 100f;
					public DerivedStatType BasePhysicalCriticalDamageMultiplier;
			public DerivedStatType PhysicalCriticalDamageMultiplier => BasePhysicalCriticalDamageMultiplier + 1.5f + Agility / 100f + WeaponExpertise / 20f;
					public DerivedStatType BaseDodgeReduction;
			public DerivedStatType DodgeReduction => BaseDodgeReduction + (Agility / 3f + WeaponExpertise * 2) / 100f;
		
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
						Strength = BaseStatType.Parse(value);
						break;
									case "agility":
						Agility = BaseStatType.Parse(value);
						break;
									case "intellect":
						Intellect = BaseStatType.Parse(value);
						break;
									case "wisdom":
						Wisdom = BaseStatType.Parse(value);
						break;
									case "weapon_expertise":
						WeaponExpertise = BaseStatType.Parse(value);
						break;
									case "resolve":
						Resolve = BaseStatType.Parse(value);
						break;
				
				// derived stats
									case "block_chance":
						BaseBlockChance = DerivedStatType.Parse(value);
						break;
									case "block_value":
						BaseBlockValue = DerivedStatType.Parse(value);
						break;
									case "dodge_chance":
						BaseDodgeChance = DerivedStatType.Parse(value);
						break;
									case "counter_chance":
						BaseCounterChance = DerivedStatType.Parse(value);
						break;
									case "hit_points":
						BaseHitPoints = DerivedStatType.Parse(value);
						break;
									case "spell_power":
						BaseSpellPower = DerivedStatType.Parse(value);
						break;
									case "max_mana":
						BaseMaxMana = DerivedStatType.Parse(value);
						break;
									case "mana_regen":
						BaseManaRegeneration = DerivedStatType.Parse(value);
						break;
									case "spell_crit_chance":
						BaseSpellCriticalChance = DerivedStatType.Parse(value);
						break;
									case "spell_crit_mult":
						BaseSpellCriticalDamageMultiplier = DerivedStatType.Parse(value);
						break;
									case "brute_physical_power_mult":
						BaseBrutePhysicalPowerMultiplier = DerivedStatType.Parse(value);
						break;
									case "finesse_physical_power_mult":
						BaseFinessePhyiscalPowerMultiplier = DerivedStatType.Parse(value);
						break;
									case "physical_crit_chance":
						BasePhysicalCriticalChance = DerivedStatType.Parse(value);
						break;
									case "physical_crit_mult":
						BasePhysicalCriticalDamageMultiplier = DerivedStatType.Parse(value);
						break;
									case "dodge_reduction":
						BaseDodgeReduction = DerivedStatType.Parse(value);
						break;
								default: throw new InvalidOperationException();
			}
		}
    }
}
