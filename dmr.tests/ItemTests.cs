using System;
using System.Linq;
using dmr.Loaders;
using dmr.Models.Characters;
using dmr.Models.General;
using dmr.Models.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dmr.tests
{
    [TestClass]
    public class ItemTests
    {
        [TestMethod]
        public void BasicItemParsing()
        {
            var items = ItemLoader.Load(@"
ranks = 5

name = Longsword
item_type = sword
item_slot = one_handed_melee_weapon

item_level =			1				| 3				| 5				| 7				| 9
attack =				3 slashing		| 4 slashing	| 5 slashing	| 6 slashing	| 7 slashing
attack =				1 fire		    | 1 fire	    | 2 fire	    | 2 fire	    | 3 fire
resists =               -1 shadow       | -1 shadow     | -2 shadow     | -2 shadow     | -3 shadow
resists =               0 crushing      | 1 crushing    | 1 crushing    | 2 crushing    | 2 crushing
stats.dodge_reduction =	10				| 12			| 15			| 17			| 20 
stats.block_chance =	10				| 13			| 17			| 20			| 22
stats.counter_chance =	15				| 17			| 20			| 23			| 26
".ToMemoryStream());

            Assert.IsTrue(items.Select(w => (int)w.rank).SequenceEqual(Enumerable.Range(1, 5)));
            Assert.IsTrue(items.All(w => w.itemtemplate.Name == "Longsword"));
            Assert.IsTrue(items.All(w => w.itemtemplate.Type == "sword"));
            Assert.IsTrue(items.All(w => w.itemtemplate.Slot == Models.Items.ItemSlot.OneHandedMeleeWeapon));
            Assert.IsTrue(items.Select(w => w.itemtemplate.Level).SequenceEqual(new byte[] { 1, 3, 5, 7, 9 }));

            Assert.IsTrue(items.Select(w => w.itemtemplate.Attack).SequenceEqual(new[]
            {
                new AttackTemplate { Slashing = 3, Fire = 1 },
                new AttackTemplate { Slashing = 4, Fire = 1 },
                new AttackTemplate { Slashing = 5, Fire = 2 },
                new AttackTemplate { Slashing = 6, Fire = 2 },
                new AttackTemplate { Slashing = 7, Fire = 3 },
            }));

            Assert.IsTrue(items.Select(w => w.itemtemplate.Resists).SequenceEqual(new[]
            {
                new ResistsTemplate { Shadow = -1, Crushing = 0 },
                new ResistsTemplate { Shadow = -1, Crushing = 1 },
                new ResistsTemplate { Shadow = -2, Crushing = 1 },
                new ResistsTemplate { Shadow = -2, Crushing = 2 },
                new ResistsTemplate { Shadow = -3, Crushing = 2 },
            }));

            Assert.IsTrue(items.Select(w => (int)w.itemtemplate.Stats.BaseDodgeReduction).SequenceEqual(new[] { 10, 12, 15, 17, 20 }));
            Assert.IsTrue(items.Select(w => (int)w.itemtemplate.Stats.BaseBlockChance).SequenceEqual(new[] { 10, 13, 17, 20, 22 }));
            Assert.IsTrue(items.Select(w => (int)w.itemtemplate.Stats.BaseCounterChance).SequenceEqual(new[] { 15, 17, 20, 23, 26 }));

            Assert.IsTrue(items.All(w => w.itemtemplate.Stats.BaseBrutePhysicalPower == 0));
            Assert.IsTrue(items.All(w => w.itemtemplate.Stats.BaseMaxMana == 0));
            Assert.IsTrue(items.All(w => w.itemtemplate.Stats.BaseHitPoints == 0));
            Assert.IsTrue(items.All(w => w.itemtemplate.Stats.BaseSpellCriticalDamageMultiplier == 0));
        }

        public void BasicItemHolding()
        {
            var items = ItemLoader.Load(@"
ranks = 5

name = Longsword
item_type = sword
item_slot = one_handed_melee_weapon

item_level =			1				| 3				| 5				| 7				| 9
attack =				3 slashing		| 4 slashing	| 5 slashing	| 6 slashing	| 7 slashing
attack =				1 fire		    | 1 fire	    | 2 fire	    | 2 fire	    | 3 fire
resists =               -1 shadow       | -1 shadow     | -2 shadow     | -2 shadow     | -3 shadow
resists =               0 crushing      | 1 crushing    | 1 crushing    | 2 crushing    | 2 crushing
stats.dodge_reduction =	10				| 12			| 15			| 17			| 20 
stats.block_chance =	10				| 13			| 17			| 20			| 22
stats.counter_chance =	15				| 17			| 20			| 23			| 26
".ToMemoryStream());

            var player = new Player();
            player.HoldItem(items[0].itemtemplate);

            //Assert.IsTrue(player.ItemInSlot[(int)ItemSlot. == "Longsword"));
            Assert.IsTrue(items.All(w => w.itemtemplate.Type == "sword"));
            Assert.IsTrue(items.All(w => w.itemtemplate.Slot == ItemSlot.OneHandedMeleeWeapon));
            Assert.IsTrue(items.Select(w => w.itemtemplate.Level).SequenceEqual(new byte[] { 1, 3, 5, 7, 9 }));

            Assert.IsTrue(items.Select(w => w.itemtemplate.Attack).SequenceEqual(new[]
            {
                new AttackTemplate { Slashing = 3, Fire = 1 },
                new AttackTemplate { Slashing = 4, Fire = 1 },
                new AttackTemplate { Slashing = 5, Fire = 2 },
                new AttackTemplate { Slashing = 6, Fire = 2 },
                new AttackTemplate { Slashing = 7, Fire = 3 },
            }));

            Assert.IsTrue(items.Select(w => w.itemtemplate.Resists).SequenceEqual(new[]
            {
                new ResistsTemplate { Shadow = -1, Crushing = 0 },
                new ResistsTemplate { Shadow = -1, Crushing = 1 },
                new ResistsTemplate { Shadow = -2, Crushing = 1 },
                new ResistsTemplate { Shadow = -2, Crushing = 2 },
                new ResistsTemplate { Shadow = -3, Crushing = 2 },
            }));

            Assert.IsTrue(items.Select(w => (int)w.itemtemplate.Stats.BaseDodgeReduction).SequenceEqual(new[] { 10, 12, 15, 17, 20 }));
            Assert.IsTrue(items.Select(w => (int)w.itemtemplate.Stats.BaseBlockChance).SequenceEqual(new[] { 10, 13, 17, 20, 22 }));
            Assert.IsTrue(items.Select(w => (int)w.itemtemplate.Stats.BaseCounterChance).SequenceEqual(new[] { 15, 17, 20, 23, 26 }));

            Assert.IsTrue(items.All(w => w.itemtemplate.Stats.BaseBrutePhysicalPower == 0));
            Assert.IsTrue(items.All(w => w.itemtemplate.Stats.BaseMaxMana == 0));
            Assert.IsTrue(items.All(w => w.itemtemplate.Stats.BaseHitPoints == 0));
            Assert.IsTrue(items.All(w => w.itemtemplate.Stats.BaseSpellCriticalDamageMultiplier == 0));
        }
    }
}
