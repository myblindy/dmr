using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models.Maps
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TileTemplate
    {
        private BitVector32 Flags;
        public bool Attackable { get => Flags[1 << 0]; set => Flags[1 << 0] = value; }
        public bool Passable { get => Flags[1 << 1]; set => Flags[1 << 1] = value; }
        public bool Door { get => Flags[1 << 2]; set => Flags[1 << 2] = value; }
        public bool Start { get => Flags[1 << 3]; set => Flags[1 << 3] = value; }
        public bool Void { get => !Flags[1 << 4]; set => Flags[1 << 4] = !value; }

        public static readonly TileTemplate VoidTile = new TileTemplate { Void = true };
        public static readonly TileTemplate EmptyTile = new TileTemplate { Void = false, Passable = true };
        public static readonly TileTemplate DoorTile = new TileTemplate { Void = false, Door = true };
        public static readonly TileTemplate WallTile = new TileTemplate { Void = false };
        public static readonly TileTemplate StartTile = new TileTemplate { Void = false, Start = true, Passable = true };

        public override int GetHashCode() => Flags.GetHashCode();

        public override bool Equals(object _obj)
        {
            if (_obj is TileTemplate obj)
                return obj.Flags.Data == Flags.Data;
            else
                return false;
        }

        public static bool operator ==(TileTemplate x, TileTemplate y) => x.Flags.Data == y.Flags.Data;

        public static bool operator !=(TileTemplate x, TileTemplate y) => x.Flags.Data != y.Flags.Data;
    }
}
