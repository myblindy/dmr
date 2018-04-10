using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models
{
    public class TileTemplate
    {
        public bool Attackable, Passable, Door;

        public static TileTemplate EmptyTile = new TileTemplate { Passable = true };
        public static TileTemplate DoorTile = new TileTemplate { Door = true };
        public static TileTemplate WallTile = new TileTemplate { };

        public override int GetHashCode() =>
            (Attackable.GetHashCode() * 23 + Passable.GetHashCode()) * 23 + Door.GetHashCode();

        public override bool Equals(object _obj)
        {
            if (_obj is TileTemplate obj)
                return obj.Attackable == Attackable && obj.Door == Door && obj.Passable == Passable;
            else
                return false;
        }
    }
}
