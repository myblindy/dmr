using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models
{
    public enum DoorwayDirection { North, South, East, West }

    public class Doorway
    {
        public DoorwayDirection Direction;
        public int X, Y;

        public Doorway(DoorwayDirection dir, int x, int y)
        {
            Direction = dir;
            X = x; Y = y;
        }
    }

    public class RoomTemplate
    {
        public string Name;
        public TileTemplate[,] Tiles;
        public List<Doorway> Doorways = new List<Doorway>();

        public void UpdateDoorways()
        {
            var rowsmax = Tiles.GetLength(0);
            var colsmax = Tiles.GetLength(1);
            for (int row = 0; row < rowsmax; ++row)
                for (int col = 0; col < colsmax; ++col)
                    if (Tiles[row, col] == TileTemplate.DoorTile)
                    {
                        // direction?
                        if (col > 0 && Tiles[row, col - 1]?.Passable == true)
                            Doorways.Add(new Doorway(DoorwayDirection.West, col, row));
                        else if (col < colsmax - 1 && Tiles[row, col + 1]?.Passable == true)
                            Doorways.Add(new Doorway(DoorwayDirection.East, col, row));
                        else if (row > 0 && Tiles[row - 1, col]?.Passable == true)
                            Doorways.Add(new Doorway(DoorwayDirection.South, col, row));
                        else if (row < rowsmax - 1 && Tiles[row + 1, col]?.Passable == true)
                            Doorways.Add(new Doorway(DoorwayDirection.North, col, row));
                        else
                            throw new InvalidOperationException();
                    }
        }

        public bool CanPlace(Map map, int x, int y)
        {
            var maprowsmax = map.Tiles.GetLength(0);
            var mapcolsmax = map.Tiles.GetLength(1);
            var rowsmax = Tiles.GetLength(0);
            var colsmax = Tiles.GetLength(1);

            // fits?
            if (rowsmax + y > maprowsmax || colsmax + x > mapcolsmax || y < 0 || x < 0)
                return false;

            for (int row = 0; row < rowsmax; ++row)
                for (int col = 0; col < colsmax; ++col)
                    if (Tiles[row, col] != null && map.Tiles[row + y, col + x] != null)
                        return false;

            return true;
        }

        public void Place(Map map, int x, int y)
        {
            var mapcolsmax = map.Tiles.GetLength(1);

            var rowsmax = Tiles.GetLength(0);
            var colsmax = Tiles.GetLength(1);
            for (int row = 0; row < rowsmax; ++row)
                for (int col = 0; col < colsmax; ++col)
                {
                    var tile = Tiles[row, col];
                    if (tile != null)
                        map.Tiles[y + row, x + col] = tile;
                }
        }
    }
}
