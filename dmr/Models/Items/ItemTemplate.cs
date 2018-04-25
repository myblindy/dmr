using dmr.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models.Items
{
    public enum ItemSlot : byte
    {
        OneHandedMeleeWeapon,
        TwoHandedMeleeWeapon,
        Helmet
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ItemTemplate
    {
        public string Name;
        public string Type;
        public ItemSlot Slot;
        public byte Level;

        public Stats Stats;
        public AttackTemplate Attack;
        public ResistsTemplate Resists;
    }
}
