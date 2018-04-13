using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models
{
    public class TileTemplate
    {
        private BitVector32 Flags;
        public bool Attackable { get => Flags[1 << 0]; set => Flags[1 << 0] = value; }
        public bool Passable { get => Flags[1 << 1]; set => Flags[1 << 1] = value; }
        public bool Door { get => Flags[1 << 2]; set => Flags[1 << 2] = value; }
        public bool Start { get => Flags[1 << 3]; set => Flags[1 << 3] = value; }

        public static TileTemplate EmptyTile = new TileTemplate { Passable = true };
        public static TileTemplate DoorTile = new TileTemplate { Door = true };
        public static TileTemplate WallTile = new TileTemplate { };
        public static TileTemplate StartTile = new TileTemplate { Start = true };

        public override int GetHashCode() => Flags.GetHashCode();

        public override bool Equals(object _obj)
        {
            if (_obj is TileTemplate obj)
                return obj.Flags.Data == Flags.Data;
            else
                return false;
        }
    }
}
