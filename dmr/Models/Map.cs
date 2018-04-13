using dmr.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models
{
    public class Map
    {
        public TileTemplate[,] Tiles;

        public Map(int w, int h, List<RoomTemplate> rooms, Random random)
        {
            Tiles = new TileTemplate[h, w];

            var bystart = rooms.GroupBy(r => r.Tiles.AsEnumerable().Any(tile => tile?.Start == true)).ToDictionary(ww => ww.Key, ww => (IEnumerable<RoomTemplate>)ww);

            var startroom = bystart[true].ChooseRandom(random);
            startroom.Place(this, w / 2 - startroom.Tiles.GetLength(1) / 2, h / 2 - startroom.Tiles.GetLength(0) / 2);
        }

        public Map(int w, int h) => Tiles = new TileTemplate[h, w];
    }
}
