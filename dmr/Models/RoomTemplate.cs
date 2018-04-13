using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models
{
    public class RoomTemplate
    {
        public TileTemplate[,] Tiles;

        public void Place(Map map, int x, int y)
        {
            var mapcolsmax = map.Tiles.GetLength(1);

            var rowmax = Tiles.GetLength(0);
            var colsmax = Tiles.GetLength(1);
            for (int row = 0; row < rowmax; ++row)
                for (int col = 0; col < colsmax; ++col)
                {
                    var tile = Tiles[row, col];
                    if (tile != null)
                        map.Tiles[y + row, x + col] = tile;
                }
        }
    }
}
